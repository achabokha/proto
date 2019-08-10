using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OAuth.Validation;

using Models;
using Server.ViewModels;


namespace Server.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin,CustomerSupport")]
    [Route("api/[controller]")]
    public class UsersController : Server.Controllers.BaseController
    {
        readonly Models.DbContext _ctx;
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(
            Models.DbContext ctx,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.FindByIdAsync(this.GetUserId());

            return Ok(new
            {
                Name = $"{user.FirstName} {user.LastName}",
                user,
                IsAdmin = await _userManager.IsInRoleAsync(user, "Admin")
            });
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var users = _ctx.Users.OrderByDescending(u => u.DateCreated);

            var userList = new List<UserViewModel>();
            foreach (var user in users)
            {
                userList.Add(MapToViewModel(user));
            }

            return Json(userList);
        }

        // TODO: use AutoMapper --
        private static UserViewModel MapToViewModel(ApplicationUser user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                DateLastAccessed = (user.DateLastAccessed == DateTime.MinValue) ? "never" : user.DateLastAccessed.ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <example>
        /// http://localhost:60001/api/users/get?userId=2E9198F8-A351-4A1F-8C4C-E99C17B916D6
        /// </example>
        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return Ok(user);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetDetails(string userId)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return Ok(new { user });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { status = "error", message = "Invalid ViewModel" });

            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == model.Id); //_ctx.Users.FindAsync();


            if (user == null)
            {
                return BadRequest(new { status = "error", message = "User not found" });
            }

            // TODO: should use AutoMapper -- 
            user.Email = model.Email;
            user.NormalizedEmail = model.Email;
            user.UserName = model.Email;
            user.NormalizedUserName = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.EmailConfirmed = model.EmailConfirmed;

            await _ctx.SaveChangesAsync();

            return Ok(new { status = "success", message = $"User details updated successfully!", user, UserId = model.Id });
        }


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
                LastName = model.LastName,
                FirstName = model.FirstName,
            };

            var r = await _userManager.CreateAsync(user, model.Password);
            if (!r.Succeeded)
            {
                AddErrors(r);
                return BadRequest(ModelState);
            }

            return Ok(new { Status = "Success", userId });
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
