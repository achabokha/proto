using System.Collections.Generic;
using System.Threading.Tasks;
using Embily.Messages;
using Embily.Models;
using Embily.Services;

namespace EmbilyServices
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

        public static Task TopupTransferAsync(this IEmailQueueSender emailSender, Transaction txn, ApplicationUser user, Account account)
        {
            var cardNumber = account.CardNumber;
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "funds transfer",
                Message = null,
                Template = "topup-transfer",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "cardNumber", $"{cardNumber.Substring(0, 2)}**********{cardNumber.Substring(12)}" },
                    { "currencyCode", txn.OriginalCurrencyCode.ToString() },
                    { "amount", txn.OriginalAmount.ToString("N8") },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationSubmittedAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "card application submitted",
                Message = null,
                Template = "card-application-submitted-kyc",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationAwaitingPaymentAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "card application awaiting payment",
                Message = null,
                Template = "card-application-awaiting-payment",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationPaidAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app, CardOrder order)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "card application payment received",
                Message = null,
                Template = "card-application-paid",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task NewApplicationAlertAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"Cornelis van der Hoeven",
                To = "cvdhoeven@embily.com",
                //Cc = "cvdhoeven@embily.com",
                Subject = "new card application alert",
                Message = null,
                Template = "card-application-alert",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "applicationNumber", app.ApplicationNumber },
                    { "reference", app.Reference },
                    { "applicationId", app.ApplicationId},
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }
        public static Task LostCardNewApplicationAlertAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app, Account account)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"Cornelis van der Hoeven",
                To = "cvdhoeven@embily.com",
                //Cc = "cvdhoeven@embily.com",
                Subject = "USER LOST CARD & create new card application",
                Message = null,
                Template = "lost-card-new-application-alert",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "accoutnNumber", account.AccountNumber },
                    { "accountId", account.AccountId },
                    { "applicationNumber", app.ApplicationNumber },
                    { "reference", app.Reference },
                    { "applicationId", app.ApplicationId},
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task InitialisationLostCardAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app, Account account, Application oldApp)
        {
            var cardNumber = account.CardNumber;
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Initialisation Lost Card",
                Message = null,
                Template = "initialisation-lost-card",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "cardNumber", $"{cardNumber.Substring(0, 2)}**********{cardNumber.Substring(12)}"},
                    { "reference", app.Reference },
                    { "oldReference", oldApp.Reference}
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task AffiliateReedemAsync(this IEmailQueueSender emailSender, ApplicationUser user, Account account, double amount)
        {
            var cardNumber = account.CardNumber;
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Affiliate balance redemption",
                Message = null,
                Template = "affiliate-balance-redemption",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "cardNumber", $"{cardNumber.Substring(0, 2)}**********{cardNumber.Substring(12)}" },
                    { "currencyCode", account.CurrencyCode.ToString() },
                    { "amount", amount.ToString("N2") },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task AffiliateInInviteAsync(this IEmailQueueSender emailSender, ApplicationUser user, string email)
        {
            var msgIn = new NotifyEmail
            {
                Name = email,
                To = email,
                Subject = "Invitation to Embily",
                Message = null,
                Template = "affiliate-invite",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName}
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task GenericAsync(this IEmailQueueSender emailSender, string email, string messsage)
        {
            var msgIn = new NotifyEmail
            {
                Name = email,
                To = email,
                Subject = "Notification",
                Message = null,
                Template = "generic",
                FieldDict = new Dictionary<string, string>
                {
                    { "message", messsage},
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }
    }
}
