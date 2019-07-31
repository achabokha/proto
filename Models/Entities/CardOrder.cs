using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Embily.Models
{
    public class CardOrder : BaseEntity
    {
        public string CardOrderId { get; set; }

        public double PurchaseAmount { get; set; }

        [Required]
        [Column("PurchaseCurrencyCode")]
        public string PurchaseCurrencyCodeString
        {
            get { return PurchaseCurrencyCode.ToString(); }
            set { PurchaseCurrencyCode = value.ParseEnum<CurrencyCodes>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCodes PurchaseCurrencyCode { get; set; }

        public double CryptoAmount { get; set; }

        [Required]
        [Column("CryptoCurrencyCode")]
        public string CryptoCurrencyCodeString
        {
            get { return CryptoCurrencyCode.ToString(); }
            set { CryptoCurrencyCode = value.ParseEnum<CurrencyCodes>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCodes CryptoCurrencyCode { get; set; }

        public string CryptoAddress { get; set; }

        public string CryptoAddressQRCode { get; set; }

        [Required]
        [Column("Status")]
        public string StatusString
        {
            get { return Status.ToString(); }
            set { Status = value.ParseEnum<CardOrderStatuses>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public CardOrderStatuses Status { get; set; }

        public string Comments { get; set; }

        [Required]
        public string ApplicationId { get; set; }

        public Application Application { get; set; }

        public string CryptoAddressLink { get; set; }
    }
}
