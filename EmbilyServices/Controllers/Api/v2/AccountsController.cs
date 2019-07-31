using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;
using AutoMapper;
using Microsoft.Extensions.Logging;

using EmbilyServices.Controllers.Api.v2.Models;
using Embily.Services;

namespace EmbilyServices.Controllers.Api.v2
{
    /// <summary>
    /// Account API 
    /// </summary>
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/v2/[controller]"), Produces("application/json"), ApiController, SwaggerTag]
    public class AccountsController : ApiBaseController
    {
        private readonly Embily.Models.EmbilyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(Embily.Models.EmbilyDbContext context, IMapper mapper, ILogger<AccountsController> logger, IProgramService programService)
            : base(programService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Accounts
        /// <summary>
        /// All Accounts 
        /// </summary>
        /// <remarks>
        /// Retrieves all accounts for the program. 
        /// </remarks>
        /// <example> 
        /// example: var a = getAccounts();
        /// </example>
        /// <see cref="GetAccount(string)"/>
        /// <seealso cref="PostAccount(Account)"/>
        /// <returns>a list of accounts</returns>
        [HttpGet, Produces("application/json")]
        public IEnumerable<Account> GetAccounts()
        {
            var programId = this.GetProgramId();
            var accounts = _context.Accounts.Where(a => a.User.ProgramId == programId && a.UserId != this.GetUserId());
            return _mapper.Map<IList<Account>>(accounts);
        }

        // GET: api/Accounts/5
        /// <summary>
        /// Account Details
        /// </summary>
        /// <remarks>Retrieve an individual account</remarks>
        /// <param name="id">Account Id</param>
        /// <returns>an account</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // GET: api/Accounts/5/Balance
        /// <summary>
        /// Account Balance 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Balance")]
        public async Task<IActionResult> GetAccountBalance([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            // TODO: call KoKard

            var balance = 0;

            return Ok(balance);
        }

        // GET: api/Accounts/5/Transactions
        /// <summary>
        /// Account Transactions
        /// </summary>
        /// <remarks>Returns a list of transactions for last 90 days</remarks>
        /// <param name="id"></param>
        /// <returns>A list of transactions for last 90 days</returns>
        [HttpGet("{id}/Transactions"), ProducesResponseType(typeof(Transaction[]), 200)]
        public async Task<IActionResult> GetAccountTransactions([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Accounts.Include(a => a.Transactions).FirstOrDefaultAsync(a => a.AccountId == id);

            if (account == null)
            {
                return NotFound();
            }

            var transactions = _mapper.Map<Transaction>(account.Transactions);

            return Ok(account);
        }


        /// <summary>
        /// Account Crypto Address
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currencyCode">BTC | LTC | ETH</param>
        /// <returns></returns>
        [HttpGet("{id}/Address/{currencyCode}")]
        public async Task<IActionResult> GetAccountAddress([FromRoute] string id, [FromRoute] string currencyCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            // TODO: get address from a provider 
            //var address = account.CryptoAddreses.Where(adr => adr.CurrencyCodeString == currencyCode);

            var address = $"sendbox {Guid.NewGuid()} sendbox"; 

            return Ok(address);
        }

        // PUT: api/Accounts/5
        /// <summary>
        /// Update Account Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount([FromRoute] string id, [FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/Accounts
        //[HttpPost]
        //public async Task<IActionResult> PostAccount([FromBody] Account account)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //_context.Accounts.Add(account);
        //    //await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        //}

        // DELETE: api/Accounts/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAccount([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var account = await _context.Accounts.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Accounts.Remove(account);
        //    await _context.SaveChangesAsync();

        //    return Ok(account);
        //}

        private bool AccountExists(string id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}