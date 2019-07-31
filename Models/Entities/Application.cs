using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Embily.Models
{
    public class Application : BaseEntity
    {
        public string ApplicationId { get; set; }

        [Required]
        public string ApplicationNumber { get; set; }

        public string Reference { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [Required]
        [Column("Status")]
        public string StatusString
        {
            get { return Status.ToString(); }
            set { Status = value.ParseEnum<ApplicationStatus>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public ApplicationStatus Status { get; set; }

        [Required]
        [Column("AccountType")]
        public string AccountTypeString
        {
            get { return AccountType.ToString(); }
            set { AccountType = value.ParseEnum<AccountTypes>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccountTypes AccountType { get; set; }

        [Required]
        [Column("CurrencyCode")]
        public string CurrencyCodeString
        {
            get { return CurrencyCode.ToString(); }
            set { CurrencyCode = value.ParseEnum<CurrencyCodes>(); }
        }

        [NotMapped]
        public CurrencyCodes CurrencyCode { get; set; }

        [Required]
        [Column("ProviderName")]
        public string ProviderNameString
        {
            get { return ProviderName.ToString(); }
            set { ProviderName = value.ParseEnum<AccountProviders>(); }
        }

        [NotMapped]
        public AccountProviders ProviderName { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public string AddressId { get; set; }

        public Address Address { get; set; }

        public string ShippingAddressId { get; set; }

        public Address ShippingAddress { get; set; }

        public string ShippingCarrier { get; set; }

        public string ShippingTrackingNum { get; set; }

        public int SelectedShippingOption { get; set; } = -1;

        public string ShippingOptions { get; set; }

        public double ShippingCost { get; set; }

        public double CardCost { get; set; }

        public string PromoCode { get; set; }

        [Column("Title")]
        public string TitleString
        {
            get { return Title.ToString(); }
            set { Title = value.ParseEnum<Titles>(); }
        }

        [NotMapped]
        public Titles Title { get; set; }

        [Column("MaritalStatus")]
        public string MaritalStatusString
        {
            get { return MaritalStatus.ToString(); }
            set { MaritalStatus = value.ParseEnum<MaritalStatuses>(); }
        }

        [NotMapped]
        public MaritalStatuses MaritalStatus { get; set; }

        [Column("Gender")]
        public string GenderString
        {
            get { return Gender.ToString(); }
            set { Gender = value.ParseEnum<Genders>(); }
        }

        [NotMapped]
        public Genders Gender { get; set; }

        /// <summary>
        /// for CCS Prepay it is 
        /// dd/mm/yyyy -- yyyy/MM/dd
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// for CCS Prepay it is 
        /// 3 letters --
        /// </summary>
        public string Nationality { get; set; }

        public string ProviderAccountNumber { get; set; }

        public string ProviderUserId { get; set; }

        public string CardNumber { get; set; }

        /// <summary>
        /// for example: why we rejected the card --
        /// </summary>
        public string Comments { get; set; }
    }
}