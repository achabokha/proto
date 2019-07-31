using Embily.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Services
{
    public class EmailQueueSender : IEmailQueueSender
    {
        readonly private IConfiguration _configuration;

        public EmailQueueSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendToEmailQueueAsync(NotifyEmail message)
        {
            // to access hosting environment i have to bring in all that IHostingEnvironment crap form ASP.NET,
            // just sticking to config library! --
            bool isDev = _configuration["ASPNETCORE_ENVIRONMENT"] == "Development";
            bool isStg = _configuration["ASPNETCORE_ENVIRONMENT"] == "Staging";
            bool isSandbox = _configuration["ASPNETCORE_ENVIRONMENT"] == "Sandbox";

            if (isDev) // testing
            {
                message.To = "achabokha@gmail.com";
                message.Subject = message.Subject + " [test]";
                if (!string.IsNullOrWhiteSpace(message.Cc)) message.Cc = "achabokha@gmail.com";
                if (!string.IsNullOrWhiteSpace(message.Bcc)) message.Bcc = "achabokha@gmail.com";
            }

            if (isStg) // staging 
            {
                message.To = "achabokha@gmail.com";
                message.Subject = message.Subject + " [test-stg]";
                if (!string.IsNullOrWhiteSpace(message.Cc)) message.Cc = "achabokha@gmail.com";
                if (!string.IsNullOrWhiteSpace(message.Bcc)) message.Bcc = "achabokha@gmail.com";
            }

            if (isSandbox) // sandbox
            {
                message.Subject = message.Subject + " [sandbox]";
            }

            CloudQueue queue = await GetQueue("notify-email");
            var msg = new CloudQueueMessage(JsonConvert.SerializeObject(message));
            await queue.AddMessageAsync(msg);
        }

        async Task<CloudQueue> GetQueue(string name)
        {
            var queueConnectionString = _configuration.GetConnectionString("AzureWebJobsStorage");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(queueConnectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(name);
            await queue.CreateIfNotExistsAsync();
            return queue;
        }
    }
}
