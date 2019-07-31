using Embily.Gateways.CoinPayInTh.Models;
using Embily.Models;
using Embily.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.Controllers
{
    [Route("callbacks/[controller]")]
    public class B2BinPayErrorCallbackController : Controller
    {
        readonly IHostingEnvironment _env;
        readonly IConfiguration _configuration;
        readonly IEmailQueueSender _emailSender;
        readonly ILogger<B2BinPayErrorCallbackController> _logger;
        readonly EmbilyDbContext _ctx;

        public B2BinPayErrorCallbackController(
            IHostingEnvironment env,
            IConfiguration configuration,
            EmbilyDbContext ctx,
            IEmailQueueSender emailSender,
            ILogger<B2BinPayErrorCallbackController> logger)
        {
            _env = env;
            _configuration = configuration;
            _ctx = ctx;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> B2BinPayErrorCallback([FromForm] string model)
        {
            _logger.LogInformation($"B2BinPayCallback: Model [{JsonConvert.SerializeObject(model)}]");

            //if (!ModelState.IsValid)
            //{
            //    throw new ApplicationException($"CoinPayCallback model is invalid");
            //}

            await _emailSender.GenericAsync("andrei.chabokha@embily.com", $"An error callback received from B2BinPay:<br /> <br />{JsonConvert.SerializeObject(model, Formatting.Indented)}");


            return Ok();
        }
    }
}
