using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Models
{
    /// ----- Users ---
    public enum Roles
    {
        Client_Registered,
        Client_KYC,
        Client_LB_Club,
        Admin,
        CustomerSupport,
        Affiliate
    }

    /// ----- Accounts -----
    
    public enum AccountStatuses
    {
        Active,
        Awaiting,
        Closed, //  person bounced (we can re-use the card and crypto addresses)
        Suspended // card stolen or lost, we should transfer CryptoAddresses to maintain crypto transfer continuation.
    }

    public enum AccountTypes
    {
        VisaPrepaid,
        MasterCardPrepaid,
        UnionPayDebit,
        Affiliate,
    }

    public enum CurrencyCodes
    {
        BTC,
        ETH,
        LTC,
        USD,
        EUR,
        NONE
    }

    public enum CryptoProviders
    {
        Bitfinex,
        None,
    }

    public enum AccountProviders
    {
        Embily,
        CCSPrepay, // discontinued -- keeping here for DB integraty --
        Virtual,
        KoKard,
        Test
    }

    /// ----- Transactions -----
    public enum TxnTypes
    {
        CREDIT,
        DEBIT
    }

    public enum TxnCodes
    {
        LOAD,
        LOAD_DIRECT,
        HANDLING,
        CARD_PURCHASE,
        LOAD_FEE,
        TRANSFER,
        AFFILIATE_REDEEM,
        AFFILIATE_FEE,
        VIRTUALCARD_LOAD
    }

    public enum TxnStatus
    {
        Created,
        Unconfirmed,
        Received,
        ExchangeOrder,
        ExchangeComplete,
        Deposited,
        Complete,
        Duplicate,
        InsufficientFunds,
        Error,
        OverTheLimit
    }

    // ----- Applications -----
    public enum ApplicationStatus
    {
        Started,
        Submitted,
        AwaitingPayment,
        Paid,
        Processing,
        Approved,
        Shipped,
        Rejected,
    }

    public enum Titles 
    {
        Mr, 
        Dr, 
        Ms,
        Mrs,
        Unknown,
    }  

    public enum MaritalStatuses 
    {
        Single, 
        Married,
        Divorced, 
        Widowed,
        Unknown,
    }

    public enum Genders 
    {
        Male,
        Female,
        Unknown,
    }

    public enum DocumentTypes
    {
        ProofOfID,
        ProofOfAddress,
        Selfie
    }

    // -------- orders ----- 
    public enum CardOrderStatuses
    {
        Erorr,
        Created,
        PaidPartially,
        Paid
    }
}
