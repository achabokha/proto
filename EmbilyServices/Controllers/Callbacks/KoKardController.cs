using Embily.Gateways.CoinPayInTh.Models;
using Embily.Models;
using Embily.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.Controllers
{

    /*

    // Cardholder Status Change
    {
        "CardHolderID": "",
        "Status": 0,
        "Remarks": ""
    }

    // Card Load Request Status Change
    {
        "TransactionID": 0,
        "Status": 0,
        "Remarks": ""
    }

    transaction id sample: 
    {"StatusCode":200,"ErrorDetails":null,"AdditionalInfo":{"ReturnStatus":"Success","TransactionID":2.0},"TableResult":null}

    note: it is actually decimal or double 

    // Card Assignment
    {
        "CardHolderID": "",
        "CardReferenceID": "",
    }

    */

    public class CardHolderStatusChangeModel
    {
        [Required]
        public string CardHolderID { get; set; }

        [Required]
        public int Status { get; set; }

        public string Remarks { get; set; }
    }

    public class CardLoadRequestStatusChangeModel
    {
        [Required]
        public double TransactionID { get; set; }

        [Required]
        public int Status { get; set; }

        public string Remarks { get; set; }
    }

    public class CardAssignmentModel
    {
        [Required]
        public string CardHolderID { get; set; }

        [Required]
        public string CardReferenceID { get; set; }
    }

    [Route("callbacks/[controller]")]
    public class KoKardController : Controller
    {
        readonly IHostingEnvironment _env;
        readonly IConfiguration _configuration;
        readonly IEmailQueueSender _emailSender;
        readonly ILogger<KoKardController> _logger;
        readonly EmbilyDbContext _ctx;

        public KoKardController(
            IHostingEnvironment env,
            IConfiguration configuration,
            EmbilyDbContext ctx,
            IEmailQueueSender emailSender,
            ILogger<KoKardController> logger)
        {
            _env = env;
            _configuration = configuration;
            _ctx = ctx;
            _emailSender = emailSender;
            _logger = logger;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> CardholderStatusChange([FromBody] CardHolderStatusChangeModel model)
        {
            _logger.LogInformation($"CardholderStatusChange: Model [{JsonConvert.SerializeObject(model)}]");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model is invalid");
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CardLoadRequestStatusChange([FromBody] CardLoadRequestStatusChangeModel model)
        {
            _logger.LogInformation($"CardLoadRequestStatusChange: Model [{JsonConvert.SerializeObject(model)}]");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model is invalid");
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CardAssignment([FromBody] CardAssignmentModel model)
        {
            _logger.LogInformation($"CardAssignment: Model [{JsonConvert.SerializeObject(model)}]");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model is invalid");
                return BadRequest();
            }

            return Ok();
        }
    }
}
