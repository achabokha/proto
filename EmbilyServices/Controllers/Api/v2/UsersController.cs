using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using EmbilyServices.Controllers.Api.v2.Models;

using Embily.Services;

namespace EmbilyServices.Controllers.Api.v2
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/v2/[controller]"), Produces("application/json"), ApiController, SwaggerTag]
    public class UsersController : ApiBaseController
    {
        private readonly Embily.Models.EmbilyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<Embily.Models.ApplicationUser> _userManager;

        public UsersController(Embily.Models.EmbilyDbContext context, IMapper mapper, ILogger<UsersController> logger, 
            UserManager<Embily.Models.ApplicationUser> userManager, IProgramService programService)
            : base(programService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: api/Users
        /// <summary>
        /// All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            var programId = this.GetProgramId();
            var users = _mapper.Map<IList<User>>(_context.Users.Where(u => u.ProgramId == programId && u.Id != this.GetUserId()));

            return users;
        }

        // GET: api/Users/5
        /// <summary>
        /// User Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var u = await _context.Users.FindAsync(id);

            if (u == null)
            {
                return NotFound();
            }

            var user = _mapper.Map<User>(u);

            return Ok(user);
        }

        // GET: api/Users/5/Applications
        /// <summary>
        /// User Applications
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/applications"), ProducesResponseType(typeof(Application[]), 200)]
        public async Task<IActionResult> GetUserApplications([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var apps = _context.Applications
                .Include(a => a.Address)
                .Include(a => a.ShippingAddress)
                .Include(a => a.Documents)
                .Where(a => a.UserId == id);

            //var usr = await _context.Users.Include(u => u.Applications).FirstOrDefaultAsync(u => u.Id == id);

            if (apps == null)
            {
                return NotFound();
            }

            var applictaions = _mapper.Map<List<Application>>(apps);

            return Ok(applictaions);
        }

        // GET: api/Users/5/Accounts
        /// <summary>
        /// User Accounts
        /// </summary>
        /// <remarks>Retrieves all users for the program.</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/accounts"), ProducesResponseType(typeof(Account[]), 200)]
        public async Task<IActionResult> GetUserAccounts([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usr = await _context.Users.Include(u => u.Accounts).FirstOrDefaultAsync(u => u.Id == id);

            if (usr == null)
            {
                return NotFound();
            }

            var accounts = _mapper.Map<List<Account>>(usr.Accounts);

            return Ok(accounts);
        }

        // PUT: api/Users/5
        /// <summary>
        /// Update User Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] string id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var u = await _context.Users.FindAsync(id);

            if (u == null)
            {
                return NotFound();
            }


            //var u1 = _mapper.Map<User, ApplicationUser>(user);
            u = _mapper.Map(user, u);

            _context.Entry(u).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { UserId = id });
        }

        // POST: api/Users
        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var u = _mapper.Map<Embily.Models.ApplicationUser>(user);

            var userId = Guid.NewGuid().ToString();

            u.UserName = user.Email;
            u.Id = userId;
            u.ProgramId = this.GetProgramId();

            var password = "Pa$$w0rd!";
            var r = await _userManager.CreateAsync(u, password);
            if (!r.Succeeded)
            {
                return BadRequest(new { errors = r.Errors });
            }

            user.UserId = userId;
            user.Status = "Active";

            return Ok(user);
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Delete User
        /// </summary>
        /// <remarks>
        /// Only User without accounts and/or applications can be deleted.
        /// </remarks>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var u = await _context.Users.FindAsync(id);
            if (u == null)
            {
                return NotFound();
            }

            _context.Users.Remove(u);
            await _context.SaveChangesAsync();

            return Ok(new { UserId = id });
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}