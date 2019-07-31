using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Embily.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EmbilyAdmin.ViewModels;
using AspNet.Security.OAuth.Validation;
using Newtonsoft.Json.Linq;

namespace EmbilyAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin,CustomerSupport")]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        private readonly EmbilyDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(
            EmbilyDbContext ctx,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public class RoleParams
        {
            public string roleName;
            public string userId;
        }


        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var roles = _roleManager.Roles;

            var roleList = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                roleList.Add(MapToViewModel(role));
            }

            return Json(roleList.OrderBy(r => r.Name));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            return Json(roles);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SetRole([FromBody] RoleParams data)
        { 
            var roleExists = await _roleManager.RoleExistsAsync(data.roleName);
            var user = await _userManager.FindByIdAsync(data.userId);

            if (roleExists)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, data.roleName);
                if (roleResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleParams data)
        {
            var roleExists = await _roleManager.RoleExistsAsync(data.roleName);
            var user = await _userManager.FindByIdAsync(data.userId);

            if (roleExists)
            {
                var roleResult = await _userManager.RemoveFromRoleAsync(user, data.roleName);
                if (roleResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /// <example>
        /// http://localhost:60001/api/roles/get?roleId=2E9198F8-A351-4A1F-8C4C-E99C17B916D6
        /// </example>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            RoleViewModel model = MapToViewModel(role);

            return Json(model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody] RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = "invalid ViewModel" });
            }

            var role = _ctx.Roles.Where(r => r.Id == model.Id).FirstOrDefault();

            if(role == null)
            {
                return BadRequest(new { error = "unable to update role" });
            }

            role.Name = model.Name;

            await _ctx.SaveChangesAsync();
            //var r = await _roleManager.UpdateAsync(new IdentityRole(model.Name));
            //if(!r.Succeeded)
            //{
            //    return BadRequest(new { error = "unable to update role"});
            //}

            return Ok(new { status = "success", Id = model.Id });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromBody] RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = "invalid ViewModel" });
            }

            var r = await _roleManager.CreateAsync(new IdentityRole(model.Name));
            if (!r.Succeeded)
            {
                return BadRequest(new { error = "unable to create role" });
            }

            return Ok(new { status = "success" });
        }

        private static RoleViewModel MapToViewModel(IdentityRole role)
        {
            return new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
            };
        }
    }
}
