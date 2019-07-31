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
    public class B2BinPayCallbackController : Controller
    {
        readonly IHostingEnvironment _env;
        readonly IConfiguration _configuration;
        readonly IEmailQueueSender _emailSender;
        readonly ILogger<B2BinPayCallbackController> _logger;
        readonly EmbilyDbContext _ctx;

        public class B2BinPayCallbackModel
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("url")]
            public string Position { get; set; }

            [JsonProperty("created")]
            public string Created { get; set; }

            [JsonProperty("expired")]
            public string Expired { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("tracking_id")]
            public string Tracking_id { get; set; }

            [JsonProperty("amount")]
            public string Amount { get; set; }

            [JsonProperty("actual_amount")]
            public string ActualAmount { get; set; }

            [JsonProperty("pow")]
            public string Pow { get; set; }

            [JsonProperty("Transactions")]
            public string Transactions { get; set; }
        }

        public B2BinPayCallbackController(
        IHostingEnvironment env,
        IConfiguration configuration,
        EmbilyDbContext ctx,
        IEmailQueueSender emailSender,
        ILogger<B2BinPayCallbackController> logger)
        {
            _env = env;
            _configuration = configuration;
            _ctx = ctx;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> B2BinPayCallback([FromForm] B2BinPayCallbackModel model)
        {
            _logger.LogInformation($"B2BinPayCallback: Model [{JsonConvert.SerializeObject(model)}]");

            //if (!ModelState.IsValid)
            //{
            //    throw new ApplicationException($"CoinPayCallback model is invalid");
            //}

            await _emailSender.GenericAsync("andrei.chabokha@embily.com", $"A callback received from B2BinPay:<br /> <br />{JsonConvert.SerializeObject(model, Formatting.Indented)}");

            return Ok("OK");

        }
    }
}
