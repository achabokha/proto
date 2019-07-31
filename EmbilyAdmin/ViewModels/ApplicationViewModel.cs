using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyAdmin.ViewModels
{
    public class ApplicationViewModel
    {
        [Required]
        public string ApplicationId { get; set; }

        public string AddressId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public string ApplicationNumber { get; set; }

        public string ShippingAddressId { get; set; }

        public string ShippingCarrier { get; set; }

        public string ShippingTrackingNum { get; set; }

        public string DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public string Title { get; set; }

        public string MaritalStatus { get; set; }

        public string Gender { get; set; }

        public string Comments { get; set; }

        public string CurrencyCodeString { get; set; }

        public string ProviderAccountNumber { get; set; }

        public string ProviderUserId { get; set; }

        public string ProviderName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Reference { get; set; }

        public string ShippingOptions { get; set; }

        public double ShippingCost { get; set; }

        public int SelectedShippingOption { get; set; }

        public double CardCost { get; set; }

        public string CardNumber { get; set; }

        public ICollection<DocumentViewModel> Documents { get; set; }
    }

    public class DocumentViewModel
    {
        public string DocumentId { get; set; }

        public int Order { get; set; }

        public string ApplicationId { get; set; }

        public string DocumentType { get; set; }

        public string FileType { get; set; }
    }

    //public class RegisterCardViemModel
    //{
    //    public string CardNumber { get; set; }

    //    public string ApplicationId { get; set; }
    //}

    //public class IntegrationViewModel
    //{
    //    [Required]
    //    public string CardNumber { get; set; }

    //    public string ClientId { get; set; }

    //    [Required]
    //    public string ApplicationId { get; set; }
    //}

    public class ApplicationStatusViewModel
    {
        public string ApplicationId { get; set; }

        public string Status { get; set; }

        public StatusDescViewModel StatusDesc { get; set; }
    }

    public class StatusDescViewModel
    {
        public string Reason { get; set; }

        public string Description { get; set; }

        public string MoreInfo { get; set; }
    }

    public class ApplicationIdViewModel
    {
        [Required]
        public string ApplicationId { get; set; }
    }
}