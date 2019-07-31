using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Embily.Models
{
    public class Account : BaseEntity 
    {
        public string AccountId { get; set; }

        // Visa | MasterCard | Serve, etc...
        // it is also a product name --
        [Required]
        [Column("AccountType")]
        public string AccountTypeString
        {
            get { return AccountType.ToString(); }
            set { AccountType = value.ParseEnum<AccountTypes>(); }
        }

        [NotMapped]
        public AccountTypes AccountType { get; set; }

        // internal Account Number readable by a person or card number 
        [Required]
        public string AccountNumber { get; set; }

        // friendly Account Name set by a user --
        public string AccountName { get; set; }

        public double Balance { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

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
        [Column("AccountStatus")]
        public string AccountStatusString
        {
            get { return AccountStatus.ToString(); }
            set { AccountStatus = value.ParseEnum<AccountStatuses>(); }
        }

        [NotMapped]
        public AccountStatuses AccountStatus { get; set; }

        [Required]
        [Column("ProviderName")]
        public string ProviderNameString
        {
            get { return ProviderName.ToString(); }
            set { ProviderName = value.ParseEnum<AccountProviders>(); }
        }

        [NotMapped]
        public AccountProviders ProviderName { get; set; }

        [Required]
        public string ProviderAccountNumber { get; set; }

        public string ProviderUserId { get; set; }

        public string CardNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<CryptoAddress> CryptoAddreses { get; set; }
    }
}
