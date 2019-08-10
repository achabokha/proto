using System;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AutoMapper;
using Models;
using Services;
using Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class SignUpController : BaseController
    {
        readonly IHostingEnvironment _env;
        readonly Models.DbContext _ctx;
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly IEmailQueueSender _emailSender;
        readonly IMapper _mapper;

        public SignUpController(
            IHostingEnvironment env,
            Models.DbContext ctx,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEmailQueueSender emailSender,
            IMapper mapper
            )
        {
            _env = env;
            _ctx = ctx;
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

            var userId = Guid.NewGuid().ToString();
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = model.Email,
                Email = model.Email,
                FirstName = "FirstName",
                LastName = "Last Name"
            };

            var r = await _userManager.CreateAsync(user, model.Password);
            if (!r.Succeeded)
            {
                AddErrors(r);
                return BadRequest(ModelState);
            }
            else
            {
                // send confirmation email 
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(Request, user.Id, code);

                await _emailSender.EmailConfirmationAsync(user, callbackUrl);
            }

            return Ok(new { Status = "Success", userId });
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