using Embily.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Services
{
    public interface IEmailQueueSender
    {
        Task SendToEmailQueueAsync(NotifyEmail email);
    }
}
