using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Gateways
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string name, string subject, string message, string cc);
    }
}
