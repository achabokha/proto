using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Embily.Models
{
    public class CryptoAddress : BaseEntity
    {
        public string CryptoAddressId { get; set; }

        [Required]
        public string Address { get; set; }

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
        public string AccountId { get; set; }

        public Account Account { get; set; }
    }
}

