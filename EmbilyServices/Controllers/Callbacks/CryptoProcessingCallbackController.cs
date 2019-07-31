using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Embily.Gateways;
using Embily.Messages;
using Embily.Models;
using Embily.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using EmbilyServices.Hubs;
using Microsoft.Extensions.Configuration;

namespace EmbilyServices.Controllers
{
    /*
     
    Callback example:
     
       {
          "account": {
            "id": "9f12413b-c08e-4eef-b3b7-f6b07c136d91"
          },
          "transaction": {
            "hash": "0x84874a49831e9f698e8023f772b980e329fda34418b065e8060031fc5ec12056",
            "position": 0,
            "from_address": "0x8612910B3419608b0bD9db130538f12B1Ec39003",
            "to_address": "0x50b7373549f3e2100d33aba7066ffca4977f611e",
            "value": 17962050000000000,
            "gas_price": 3000000000,
            "nonce": 0,
            "input_data": "0x",
            "type": "receive"
          },
          "event": {
            "event_type": "CREATE",
            "event_group": "TX"
          },
          "block_number": null,
          "confirmation": 0
        }     
    */

    public class CryptoProcessingCallbackModel
    {
        public class CPCAccount
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public class CPCTransaction
        {
            [Required]
            [JsonProperty("hash")]
            public string Hash { get; set; }

            [Required]
            [JsonProperty("position")]
            public int Position { get; set; }

            [Required]
            [JsonProperty("from_address")]
            public string FromAddress { get; set; }

            [Required]
            [JsonProperty("to_address")]
            public string ToAddress { get; set; }

            [Required]
            [JsonProperty("value")]
            public double Value { get; set; }

            [Required]
            [JsonProperty("gas_price")]
            public double GasPrice { get; set; }

            [Required]
            [JsonProperty("nonce")]
            public long Nonce { get; set; }

            [Required]
            [JsonProperty("input_data")]
            public string InputData { get; set; }

            [Required]
            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class CPCEvent
        {
            [Required]
            [JsonProperty("event_type")]
            public string EventType { get; set; }

            [Required]
            [JsonProperty("event_group")]
            public string EventGroup { get; set; }
        }

        [JsonProperty("account")]
        public CPCAccount Account { get; set; }

        [Required]
        [JsonProperty("transaction")]
        public CPCTransaction Transaction { get; set; }

        [Required]
        [JsonProperty("event")]
        public CPCEvent Description { get; set; }

        //[Required]
        [JsonProperty("block_number")]
        public long? BlockNumber { get; set; }

        [Required]
        [JsonProperty("confirmation")]
        public int Confirmation { get; set; }
    }

    [Route("callbacks/[controller]")]
    public class CryptoProcessingCallbackController : Controller
    {
        readonly ILogger<CryptoProcessingCallbackController> _logger;
        readonly CallbackProcessor _processor;


        public CryptoProcessingCallbackController(
            IHostingEnvironment env,
            IConfiguration configuration,
            EmbilyDbContext ctx,
            IEmailQueueSender emailSender,
            ILogger<CryptoProcessingCallbackController> logger,
            IRefGen refGen,
            IHubContext<BlockchainHub> hubContext
        )
        {
            _processor = new CallbackProcessor(env, configuration, ctx, emailSender, logger, refGen, hubContext);
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> CryptoProcessingCallback([FromBody] CryptoProcessingCallbackModel model)
        {
            _logger.LogInformation($"CryptoProcessingCallbackModel: started. Model: [{JsonConvert.SerializeObject(model)}]");

            if (!ModelState.IsValid)
            {
                return BadRequest($"invalid parameters!");
            }

            await _processor.Process(model.Transaction.ToAddress, model.Transaction.Hash, model.Transaction.Value);

            _logger.LogInformation($"CryptoProcessingCallbackModel: complete. Model: [{JsonConvert.SerializeObject(model)}]");

            return Ok();
        }
    }
}
