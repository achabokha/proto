using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gateways
{
    public class SendGridMailer : IEmailSender
    {
        // embily keys -- 
        const string apiKey = "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";

        public async Task SendEmailAsync(string email, string name, string subject, string messageText, string cc)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("service@embily.com", "Team Embily"),
                
                Subject = subject,
                HtmlContent = messageText,
            };

            msg.AddTo(new EmailAddress(email, name));
            if(!string.IsNullOrWhiteSpace(cc)) msg.AddCc(cc);
            msg.AddBcc("funding@embily.com", "Funding Embily");

            var response = await client.SendEmailAsync(msg);
        }
    }
}
