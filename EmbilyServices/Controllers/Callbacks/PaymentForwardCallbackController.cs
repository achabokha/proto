using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

using Embily.Gateways;
using Embily.Models;
using Embily.Services;
using Microsoft.AspNetCore.SignalR;
using EmbilyServices.Hubs;
using Microsoft.Extensions.Configuration;

namespace EmbilyServices.Controllers
{
    /*
     Sample:  
        {
            "value": 100000000,
            "input_address": "16uKw7GsQSzfMaVTcT7tpFQkd7Rh9qcXWX",
            "destination": "15qx9ug952GWGTNn7Uiv6vode4RcGrRemh",
            "input_transaction_hash": "39bed5d...",
            "transaction_hash": "1aa6103..."
        } 
    */

    public class PaymentForwardCallbackModel
    {
        [Required]
        [JsonProperty("value")]
        public double Value { get; set; }

        [Required]
        [JsonProperty("input_address")]
        public string InputAddress { get; set; }

        [Required]
        [JsonProperty("destination")]
        public string Destination { get; set; }

        [Required]
        [JsonProperty("input_transaction_hash")]
        public string InputTransactionHash { get; set; }

        [Required]
        [JsonProperty("transaction_hash")]
        public string TransactionHash { get; set; }
    }

    [Route("api/[controller]")]
    [Route("callbacks/[controller]")]
    public class PaymentForwardCallbackController : Controller
    {
        readonly ILogger<PaymentForwardCallbackController> _logger;
        readonly CallbackProcessor _processor;

        public PaymentForwardCallbackController(
            IHostingEnvironment env,
            IConfiguration configuration,
            EmbilyDbContext ctx,
            IEmailQueueSender emailSender,
            ILogger<PaymentForwardCallbackController> logger,
            IRefGen refGen,
            IHubContext<BlockchainHub> hubContext
            )
        {
            _processor = new CallbackProcessor(env, configuration, ctx, emailSender, logger, refGen, hubContext);
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> PaymentForwardCallback([FromBody] PaymentForwardCallbackModel model)
        {
            _logger.LogInformation($"PaymentForwardCallback: started [{JsonConvert.SerializeObject(model)}]");

            if (!ModelState.IsValid)
            {
                return BadRequest($"invalid parameters!");
            }

            await _processor.Process(model.InputAddress, model.TransactionHash, model.Value);

            _logger.LogInformation($"PaymentForwardCallback: complete. Model: [{JsonConvert.SerializeObject(model)}]");

            return Ok();
        }
    }
}
