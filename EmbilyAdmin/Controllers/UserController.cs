using System;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Embily.Models;
using Embily.Services;
using EmbilyAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmbilyAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        const string _testDestinatioEmail = "achabokha@hotmail.com";

        private readonly IHostingEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailQueueSender _emailSender;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IHostingEnvironment env,
            UserManager<ApplicationUser> userManager,
            IEmailQueueSender emailSender,
            ILogger<UserController> logger)
        {
            _env = env;
            _userManager = userManager;
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
    }
}