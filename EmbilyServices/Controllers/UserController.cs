using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AutoMapper;
using Embily.Models;
using Embily.Services;
using EmbilyServices.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmbilyServices.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        //const string _testDestinatioEmail = "achabokha@hotmail.com";

        private readonly EmbilyDbContext _ctx;
        private readonly IHostingEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailQueueSender _emailSender;
        private readonly ILogger<UserController> _logger;

        public UserController(
            EmbilyDbContext ctx,
            IHostingEnvironment env,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailQueueSender emailSender,
            ILogger<UserController> logger)
        {
            _ctx = ctx;
            _env = env;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSettings()
        {
            var user = await _userManager.FindByIdAsync(this.GetUserId());
            return Ok(new { user.FirstName, user.LastName, user.Email });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new ApplicationException($"View Model is invalid");
            }

            var user = await _userManager.FindByIdAsync(this.GetUserId());
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID [{GetUserId()}].");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(new { error = "unable to change password" });
            }

            _logger.LogInformation($"User with ID [{this.GetUserId()}] changed their password successfully.");

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok();
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink2(Request, code);

            await _emailSender.ResetPasswordAsync(user, callbackUrl);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                throw new ApplicationException(result.Errors.ToString());
            }

            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ConfirmPassword([FromBody] ConfirmPasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return BadRequest(new { status = "error", message = "User not found" });
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return BadRequest(new {status = "error", message = "The password is invalid."
                });
            }

            return Ok(new { status = "confirm", message = $"", user });
            //return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetInvitees()
        {
            var invitees = await _ctx.AffiliateEmails.Where(a => a.UserId == this.GetUserId()).OrderByDescending(o => o.DateCreated).ToListAsync();
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<AffiliateEmail, AffiliateInviteViewModels>());
            var customInvitees = Mapper.Map<List<AffiliateEmail>, List<AffiliateInviteViewModels>>(invitees);

            var inviteesReg = customInvitees.Where(i => i.HasRegistred).ToList();

            //??var invitees = await _ctx.Users.Where(u => u.AffiliatedWithUserId == this.GetUserId()).OrderByDescending(o => o.DateCreated).ToListAsync();
            var countInvite = customInvitees.Count;
            var countRegistered = inviteesReg.Count;
            var countApproved = 0;
            var countTransacting = 0;

            foreach (var invite in customInvitees)
            {
                invite.SatusColor = "purple";
                if (invite.HasRegistred)
                {
                    invite.SatusColor = "blue";
                    var appAproved = await _ctx.Applications.Where(app => app.UserId == invite.UserId).Where(s => s.Status == ApplicationStatus.Approved).FirstOrDefaultAsync();
                    if (appAproved != null)
                    {
                        invite.SatusColor = "green";
                        countApproved++;
                    }

                    var accounts = await _ctx.Accounts.Where(acc => acc.UserId == invite.UserId).ToArrayAsync();
                    foreach (var account in accounts)
                    {
                        var transactions = await _ctx.Transactions.Where(t => t.AccountId == account.AccountId).ToListAsync();
                        if (transactions != null || transactions.Count == 0)
                        {
                            invite.SatusColor = "turquoise";
                            countTransacting++;
                            break;
                        }
                    }
                }
            }
            if (customInvitees.Count == 0) customInvitees = null;

            return Ok(new { customInvitees, countInvite, countRegistered, countApproved, countTransacting });
        }
    }
}