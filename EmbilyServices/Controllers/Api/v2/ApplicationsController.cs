using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;
using EmbilyServices.Controllers.Api.v2.Models;
using AutoMapper;
using Embily.Services;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;

namespace EmbilyServices.Controllers.Api.v2
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/v2/[controller]"), Produces("application/json"), ApiController, SwaggerTag]
    public class ApplicationsController : ApiBaseController
    {
        private readonly Embily.Models.EmbilyDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRefGen _refGen;

        public ApplicationsController(Embily.Models.EmbilyDbContext context, IMapper mapper, IRefGen refGen, IProgramService programService)
            : base(programService)
        {
            _context = context;
            _mapper = mapper;
            _refGen = refGen;
        }

        // GET: api/Applications
        /// <summary>
        /// All Applications
        /// </summary>
        /// <remarks>Retrieves all applications for the program.</remarks>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Application> GetApplications()
        {
            var programId = this.GetProgramId();

            var apps = _context.Applications.Where(a => a.User.ProgramId == programId && a.UserId != this.GetUserId())
                .Include(a => a.Address)
                .Include(a => a.ShippingAddress)
                .Include(a => a.Documents)
                ;
            var applications = _mapper.Map<IEnumerable<Application>>(apps);

            return applications;
        }

        // GET: api/Applications/5
        /// <summary>
        /// Application Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var app = await _context.Applications
                .Include(a => a.Address)
                .Include(a => a.ShippingAddress)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.ApplicationId == id);

            if (app == null)
            {
                return NotFound();
            }
            var application = _mapper.Map<Application>(app);
            return Ok(application);
        }

        // PUT: api/Applications/5
        /// <summary>
        /// Update Application Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication([FromRoute] string id, [FromBody] Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != application.ApplicationId)
            {
                return BadRequest();
            }

            throw new NotImplementedException();

            // TODO: need to think what do I here -- 
            // what does it mean to update the application -- 

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Applications
        /// <summary>
        /// Create Application 
        /// </summary>
        /// <remarks>
        /// Submit a new application to initiate approval process
        /// </remarks>
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostApplication([FromBody] Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var apiUuserEmail = User.Identity;

            var user = await _context.Users.FindAsync(application.UserId);
            if (user == null || user?.ProgramId != this.GetProgramId())
            {
                return NotFound(new { Message = "User not found" });
            }

            var previousApps = _context.Applications.Where(a => a.UserId == application.UserId);

            var applicationId = Guid.NewGuid().ToString();

            var app = _mapper.Map<Embily.Models.Application>(application);
            app.ApplicationId = applicationId;

            // card type --
            app.Status = Embily.Models.ApplicationStatus.Submitted;
            app.AccountType = Embily.Models.AccountTypes.UnionPayDebit;
            app.CurrencyCode = Embily.Models.CurrencyCodes.USD;

            app.Address.UserId = user.Id;
            app.ShippingAddress.UserId = user.Id;

            app.ApplicationNumber = app.GenApplicationNumber(user);

            long num = Convert.ToInt64(app.ApplicationNumber.Replace("-", ""));
            app.Reference = _refGen.GenAppRef(num);

            _context.Applications.Add(app);
            await _context.SaveChangesAsync();

            return Ok(new { applicationId, Status = "Submitted" });
        }

        //private Embily.Models.Document MapDocument(Embily.Models.Application app, string docBase64, Embily.Models.DocumentTypes type)
        //{
        //    var fileType = "image/jpg";

        //    docBase64.IndexOf("base64,");
        //    docBase64.Split("base64");

        //    return new Embily.Models.Document
        //    {
        //        Application = app,
        //        DocumentId = Guid.NewGuid().ToString(),
        //        DocumentType = Embily.Models.DocumentTypes.ProofOfAddress,
        //        Image = Convert.FromBase64String(docBase64),
        //        FileType = fileType,
        //    };
        //}

        // DELETE: api/Applications/5
        /// <summary>
        /// Delete Application
        /// </summary>
        /// <remarks>
        /// <remarks>Processed or in-process application cannot be deleted.</remarks>
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            if (application.Status == Embily.Models.ApplicationStatus.Submitted || application.Status == Embily.Models.ApplicationStatus.Started)
            {
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();

                return Ok(new { application.ApplicationId });
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool ApplicationExists(string id)
        {
            return _context.Applications.Any(e => e.ApplicationId == id);
        }
    }
}