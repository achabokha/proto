using System.Collections.Generic;
using System.Threading.Tasks;
using Server.ViewModels;
using Messages;
using Models;
using Services;

namespace Server
{
    public static class EmailSenderExtensions
    {
        public static Task EmailConfirmationAsync(this IEmailQueueSender emailSender, ApplicationUser user, string link)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "email verification",
                Message = null,
                Template = "email-verification",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "confirmLink", link },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ResetPasswordAsync(this IEmailQueueSender emailSender, ApplicationUser user, string callbackUrl)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "password reset",
                Message = null,
                Template = "password-reset",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "resetLink", callbackUrl },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }
    }
}
