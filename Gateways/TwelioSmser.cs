using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Gateways
{
    public class TwilioSmser : ISmsSender
    {
        // Find your Account Sid and Auth Token at twilio.com/console
        const string accountSid = "ACbd8dc448e1533882640f3fa5bba5b60e";
        const string authToken = "69b84954e18b87b9941ed304032e54c3";
        const string fromNumber = "+447403935468";

        public async Task SendSmsAsync(string number, string message)
        {
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber(number);
            var from = new PhoneNumber(fromNumber);

            var msg = await MessageResource.CreateAsync(
              to,
              from: from,
              body: message);

            if(msg.ErrorCode != null)
            {
                throw new ApplicationException("Error at Twilio Smser: " + msg.ErrorMessage);
            }


            // msg.Sid - response i guess --
        }
    }
}
