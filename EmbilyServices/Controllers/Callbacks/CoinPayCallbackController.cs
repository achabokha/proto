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
    public class CoinPayCallbackController : Controller
    {
        readonly IHostingEnvironment _env;
        readonly IConfiguration _configuration;
        readonly IEmailQueueSender _emailSender;
        readonly ILogger<CoinPayCallbackController> _logger;
        readonly EmbilyDbContext _ctx;

        public CoinPayCallbackController(
            IHostingEnvironment env,
            IConfiguration configuration,
            EmbilyDbContext ctx,
            IEmailQueueSender emailSender,
            ILogger<CoinPayCallbackController> logger)
        {
            _env = env;
            _configuration = configuration;
            _ctx = ctx;
            _emailSender = emailSender;
            _logger = logger;
        }


        // response comes in JSON from CoinPay, here is an example: 
        // { "order_id":"8","message":"paid in full","confirmed_in_full":true,"signature":"786a9b1dde3deed19c94c1989c2c240659c48723"}

        [HttpPost()]
        public async Task<IActionResult> CoinPayCallback([FromBody] ResponseCallbacks model)
        {
            _logger.LogInformation($"CoinPayCallback: Model [{JsonConvert.SerializeObject(model)}]");

            //if (!ModelState.IsValid)
            //{
            //    throw new ApplicationException($"CoinPayCallback model is invalid");
            //}

            var orderId = model.OrderId;

            // 0. validate signer - 
            //Signature signature = new Signature();
            //var result = signature.Check(model);

            //if (result)
            //{
            // 1. look up application by orderId in CardOrders 
            var cardOrder = _ctx.CardOrders.Where(co => co.CardOrderId == orderId).FirstOrDefault();
            if (cardOrder == null)
            {
                throw new ApplicationException($"CoinPayCallback: card order not found. Model: [{JsonConvert.SerializeObject(model)}]");
            }

            // 2. set application to status Paid
            var application = _ctx.Applications.Where(app => app.ApplicationId == cardOrder.ApplicationId).FirstOrDefault();
            application.Status = ApplicationStatus.Paid;

            // 3. set order to status Complete
            cardOrder.Status = CardOrderStatuses.Paid;

            await _ctx.SaveChangesAsync();

            // 4. notify about payment received for an application

            // send notification email  
            await SendToNotifcation(application, cardOrder);

            return Ok(new { Status = "Ok" });
            //}
            //else
            //{
            //    return BadRequest(new { Status = "" });
            //}
        }

        private async Task SendToNotifcation(Application application, CardOrder cardOrder)
        {
            var user = await _ctx.Users.FindAsync(application.UserId);
            await _emailSender.ApplicationPaidAsync(user, application, cardOrder);
            await _emailSender.NewApplicationAlertAsync(user, application);
        }

    }
}
