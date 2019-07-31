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

namespace EmbilyAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin,CustomerSupport")]
    [Route("api/[controller]")]
    public class AddressesController : Controller
    {
        private readonly EmbilyDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AddressesController(
            EmbilyDbContext ctx,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("[action]/{addressId}")]
        public async Task<Address> Get(string addressId)
        {
            var address = await _ctx.Addresses.FindAsync(addressId);
            return address ?? new Address();
        }

        [HttpGet("[action]/{appId}")]
        public async Task<Address> GetAddress(string appId)
        {
            var application = await _ctx.Applications.FindAsync(appId);

            var address = await _ctx.Addresses.FindAsync(application.AddressId);
            return address ?? new Address();
        }
        [HttpGet("[action]/{appId}")]
        public async Task<Address> GetShippingAddress(string appId)
        {
            var application = await _ctx.Applications.FindAsync(appId);

            var address = await _ctx.Addresses.FindAsync(application.ShippingAddressId);
            return address ?? new Address();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody] Address model)
        {
            var address = await _ctx.Addresses.FindAsync(model.AddressId);
            if(address == null)
            {
                return BadRequest(new { status = "error", message = "Address not created by user." });
            }
            _ctx.Entry(address).CurrentValues.SetValues(model);

            await _ctx.SaveChangesAsync();

            return Ok(new { Status = "success", Message = $"Address updated successfully!" });
        }
    }
}
