using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Authorization;

using AspNet.Security.OAuth.Validation;

using Embily.Models;
using System.Threading;
using EmbilyAdmin.ViewModels;
using Embily.Gateways.CCSPrepay;
using Embily.Gateways;
using Microsoft.AspNetCore.Hosting;
using Embily.Services;

namespace EmbilyAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin,CustomerSupport")]
    [Route("api/[controller]")]
    public class ProgramsController : Controller
    {
        readonly IHostingEnvironment _env;
        readonly EmbilyDbContext _ctx;
        readonly ICardLoad _cardLoad;
        readonly ICryptoAddress _cryptoAddress;
        readonly IEmailQueueSender _emailSender;
        readonly IRefGen _refGen;

        public ProgramsController(
            IHostingEnvironment env,
            EmbilyDbContext ctx,
            ICardLoad card,
            ICryptoAddress cryptoAddress,
            IEmailQueueSender emailSender,
            IRefGen refGen
            )
        {
            _env = env;
            _ctx = ctx;
            _cardLoad = card;
            _cryptoAddress = cryptoAddress;
            _emailSender = emailSender;
            _refGen = refGen;
        }

        [HttpGet("[action]")]
        public IEnumerable<Embily.Models.Program> GetPrograms()
        {
            return _ctx.Programs
                .ToList().OrderByDescending(a => a.DateCreated);
        }

        [HttpGet("[action]/{programId}")]
        public async Task<Embily.Models.Program> Get(string programId)
        {
            var program = await _ctx.Programs
               .FirstOrDefaultAsync(a => a.ProgramId == programId);

            return program;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateProgram([FromBody] ProgramViewModel model)
        {
            var program = await _ctx.Programs.FirstOrDefaultAsync(a => a.ProgramId == model.ProgramId);
            if (program == null)
            {
                return BadRequest(new { status = "error", message = "Program not found." });
            }
            try
            {
                program.Domain = model.Domain;
                program.Title = model.Title;
                program.Settings = model.Settings;

                await _ctx.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                return BadRequest(new { status = "error", message = "Program was not saved." });
            }

            return Ok(new { status = "success", message = $"Program details updated successfully!"});
        }       
    }
}
