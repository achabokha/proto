using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Embily.Models
{
    public class Transaction : BaseEntity
    {
        public string TransactionId { get; set; }

        [Required]
        public string TransactionNumber { get; set; }

        public string Reference { get; set; }

        public string TxnGroupRef { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }

        public string Memo { get; set; }

        [Required]
        [Column("TxnType")]
        public string TxnTypeString
        {
            get { return TxnType.ToString(); }
            set { TxnType = value.ParseEnum<TxnTypes>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public TxnTypes TxnType { get; set; }

        [Required]
        [Column("TxnCode")]
        public string TxnCodeString
        {
            get { return TxnCode.ToString(); }
            set { TxnCode = value.ParseEnum<TxnCodes>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public TxnCodes TxnCode { get; set; }

        [Required]
        [Column("Status")]
        public string StatusString
        {
            get { return Status.ToString(); }
            set { Status = value.ParseEnum<TxnStatus>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public TxnStatus Status { get; set; }

        public bool IsAmountKnown { get; set; }

        [Required]
        public double OriginalAmount { get; set; }

        [Required]
        [Column("OriginalCurrencyCode")]
        public string OriginalCurrencyCodeString
        {
            get { return OriginalCurrencyCode.ToString(); }
            set { OriginalCurrencyCode = value.ParseEnum<CurrencyCodes>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCodes OriginalCurrencyCode { get; set; }

        public double DestinationAmount { get; set; }

        [Required]
        [Column("DestinationCurrencyCode")]
        public string DestinationCurrencyCodeString
        {
            get { return DestinationCurrencyCode.ToString(); }
            set { DestinationCurrencyCode = value.ParseEnum<CurrencyCodes>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCodes DestinationCurrencyCode { get; set; }

        [Required]
        [Column("CryptoProvider")]
        public string CryptoProviderString
        {
            get { return CryptoProvider.ToString(); }
            set { CryptoProvider = value.ParseEnum<CryptoProviders>(); }
        }

        [NotMapped]
        [JsonConverter(typeof(StringEnumConverter))]
        public CryptoProviders CryptoProvider { get; set; }

        [Required]
        public string CryptoAddress { get; set; }

        public string CryptoTxnId { get; set; }
        
        public double ExchangeFee { get; set; }

        public double PlatformFee { get; set; }

        [Required]
        public string AccountId { get; set; }

        public Account Account { get; set; }

        public string SourceAccountId { get; set; }

        public string DestinationAccountId { get; set; }
    }
}
