using System.Collections.Generic;
using System.Threading.Tasks;
using EmbilyAdmin.ViewModels;
using Embily.Messages;
using Embily.Models;
using Embily.Services;

namespace EmbilyAdmin
{
    public static class EmailSenderExtensions
    {
        public static Task ApplicationProcessingAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Processing your card application ",
                Message = null,
                Template = "card-application-processing",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationRejectedAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app, StatusDescViewModel statusDesc)
        {
             var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Card application rejected",
                Message = null,
                Template = "card-application-rejected",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                    { "reason", statusDesc.Reason },
                    { "reasonDesc", statusDesc.Description },
                    { "moreInfo", statusDesc.MoreInfo },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationAppovedKYCAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Card application approved",
                Message = null,
                Template = "card-application-approved-kyc",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationAppovedNonKYCAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Card application approved",
                Message = null,
                Template = "card-application-approved-nonkyc",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationShippedAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app, string trackingUrl)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Card shipped",
                Message = null,
                Template = "card-shipping",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                    { "carrier", app.ShippingCarrier },
                    { "trackingNumber", app.ShippingTrackingNum },
                    { "trackingUrl", trackingUrl },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationPickedUpAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app)
        {
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Card picked up",
                Message = null,
                Template = "card-pickup",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "reference", app.Reference },
                },
            };

            return emailSender.SendToEmailQueueAsync(msgIn);
        }

        public static Task ApplicationPaidAsync(this IEmailQueueSender emailSender, ApplicationUser user, Application app)
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

        public static Task TopupLoadAsync(this IEmailQueueSender emailSender, ApplicationUser user, Transaction txn, Account account)
        {
            var cardNumber = account.CardNumber;
            var msgIn = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Funds deposited",
                Message = null,
                Template = "topup-load",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "cardNumber", $"{cardNumber.Substring(0, 2)}**********{cardNumber.Substring(12)}"},
                    { "currencyCode", txn.DestinationCurrencyCode.ToString() },
                    { "amount", txn.DestinationAmount.ToString("N2") },
                },
                TransactionNumber = txn.TransactionNumber,
                TxnId = txn.TransactionId,
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
