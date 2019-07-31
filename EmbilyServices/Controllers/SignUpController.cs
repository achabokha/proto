using System;
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

namespace EmbilyServices.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class SignUpController : BaseController
    {
        readonly IHostingEnvironment _env;
        readonly EmbilyDbContext _ctx;
        readonly IProgramService _programService;
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly IEmailQueueSender _emailSender;
        readonly IMapper _mapper;

        public SignUpController(
            IHostingEnvironment env,
            EmbilyDbContext ctx,
            IProgramService programService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEmailQueueSender emailSender,
            IMapper mapper
            )
        {
            _env = env;
            _ctx = ctx;
            _programService = programService;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string domain = Request.Host.Host.Replace("www.", "");
            Embily.Models.Program program = _programService.GetProgramByDomain(domain);

            var userId = Guid.NewGuid().ToString();
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = model.Email,
                Email = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                ProgramId = program.ProgramId,
            };

            var r = await _userManager.CreateAsync(user, model.Password);
            if (!r.Succeeded)
            {
                AddErrors(r);
                return BadRequest(ModelState);
            }
            else
            {
                // check if there is an invite 
                var affiliated = await _ctx.AffiliateEmails.FirstOrDefaultAsync(a => a.Email == user.NormalizedEmail);
                if (affiliated != null)
                {
                    await AffiliateByEmailAsync(affiliated, user);
                }

                // check token 
                if (!string.IsNullOrWhiteSpace(model.Token) && affiliated == null)
                {
                    var token = await _ctx.AffiliateTokens.FirstOrDefaultAsync(t => t.Token.ToUpperInvariant() == model.Token.ToUpperInvariant());
                    if (token != null)
                    {
                        await AffliteByTokenAsync(token, user);
                    }
                }

                // send confirmation email 
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(Request, user.Id, code);

                await _emailSender.EmailConfirmationAsync(user, callbackUrl);
            }

            return Ok(new { Status = "Success", userId });
        }

        private async Task AffliteByTokenAsync(AffiliateToken token, ApplicationUser user)
        {
            token.Counter++;
            user.AffiliateTokenUsed = token.Token;
            user.AffiliatedWithUserId = token.UserId;

            await _ctx.SaveChangesAsync();
        }

        private async Task AffiliateByEmailAsync(AffiliateEmail affiliated, ApplicationUser user)
        {
            affiliated.HasRegistred = true;
            affiliated.RegistredUserId = user.Id;
            user.AffiliatedWithUserId = affiliated.UserId;

            await _ctx.SaveChangesAsync();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{model.UserId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Code);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Error confirming email for user with ID '{model.UserId}':");
            }

            return Ok(new { UserId = user.Id });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}