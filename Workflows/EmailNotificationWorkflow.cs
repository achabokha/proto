using Embily.Gateways;
using Embily.Messages;
using Embily.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Embily.Workflows
{
    public class EmailNotificationWorkflow : BaseWorkflow
    {
        IEmailSender _mailer;

        public EmailNotificationWorkflow(IEmailSender mailer, NameValueCollection appSettings, EmbilyDbContext ctx, TextWriter log) 
            : base(appSettings, ctx, log)
        {
            _mailer = mailer;
        }

        public async Task Process(NotifyEmail msg)
        {
            string body = msg.Message;

            if (!string.IsNullOrWhiteSpace(msg.Template))
            {
                var path = Path.Combine("EmailTemplates", $"{msg.Template}.html");

                string template = File.ReadAllText(path);

                // extract all fields from templates 
                var fields = ExtractFieilds(template);

                // fill out from a dictionary --
                body = InserValues(template, fields, msg.FieldDict);

                //await _log.WriteAsync(body);
            }

            await _mailer.SendEmailAsync(msg.To, msg.Name, msg.Subject, body, msg.Cc);
        }

        private string InserValues(string template, MatchCollection fields, IDictionary<string, string> fieldDict)
        {
            foreach (Match field in fields)
            {
                var key = field.Value;
                var value = fieldDict[key];
                template = template.Replace("{{" + key + "}}", value);
            }

            return template;
        }

        private MatchCollection ExtractFieilds(string template)
        {
            Regex rgx = new Regex(@"(?<={{)(.*?)(?=}})", RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(template);

            // for debugging 
            //foreach (Match match in matches)
            //{
            //    Console.WriteLine(match.Value);
            //}

            return matches;
        }
    }
}
