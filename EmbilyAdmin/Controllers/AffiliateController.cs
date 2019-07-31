using AspNet.Security.OAuth.Validation;
using Embily.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin,CustomerSupport")]
    [Route("api/[controller]")]
    public class AffiliateController : Controller
    {
        private readonly EmbilyDbContext _ctx;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly ILogger<ApplicationsController> _logger;
        //private readonly CCSPrepayAPI _api;
        //readonly IEmailQueueSender _emailSender;

        public AffiliateController(
            EmbilyDbContext ctx
            //UserManager<ApplicationUser> userManager,
            //SignInManager<ApplicationUser> signInManager,
            //IEmailQueueSender emailSender,
            //ILogger<ApplicationsController> logger
            )
        {
            _ctx = ctx;
            //_userManager = userManager;
            //_signInManager = signInManager;
            //_emailSender = emailSender;
            //_logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAffiliateInvite()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

           var affiliateEmails = await _ctx.AffiliateEmails
                .Include(u => u.User)
                .OrderByDescending(ae => ae.DateCreated)
                .ToListAsync();

            return Ok(new {affiliateEmails, Message = $"complete successfully" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTokens()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tokenList = await _ctx.AffiliateTokens
               .Include(u => u.User)
               .OrderByDescending(at => at.DateCreated)
               .ToListAsync();         

            return Ok(new {tokenList, Message = $"complete successfully" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsers()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var users = await _ctx.Users
                .Join(_ctx.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new
                {
                    u.Id,
                    u.LastName,
                    u.FirstName,
                    u.DateCreated,
                    u.AffiliatedWithUser,
                    u.AffiliatedWithUserId,
                    u.AffiliateTokenUsed,
                    u.Email,
                    ur.RoleId
                })
                .Join(_ctx.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new
                {
                    ur.Id,
                    ur.LastName,
                    ur.FirstName,
                    ur.DateCreated,
                    ur.AffiliatedWithUser,
                    ur.AffiliatedWithUserId,
                    ur.AffiliateTokenUsed,
                    ur.Email,
                    r.Name
                })
                .Where(u => u.Name == "Affiliate")
                .ToListAsync();


            return Ok(new { users, Message = $"complete successfully" });
        }
    }
}