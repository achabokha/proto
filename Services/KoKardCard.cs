using AutoMapper;
using Embily.Gateways.KoKard;
using Embily.Gateways.KoKard.Models;
using Embily.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EmbilyDocument = Embily.Models.Document;
using KoKardDocument = Embily.Gateways.KoKard.Models.Document;


namespace Embily.Services
{
    public class KoKardCard : ICardLoad, ICard, ICardHolder
    {
        private class TransactionMapProfile : Profile
        {
            public TransactionMapProfile()
            {
                CreateMap<TransactionTableItem, TransactionInfo>()
                    .ForMember(dest => dest.Description, opts => opts.MapFrom(src => MapDescription(src.Description, src.Merchant)))
                    .ForMember(dest => dest.BillingAmount, opts => opts.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.BillingCurrencyCode, opts => opts.MapFrom(src => "USD"))
                    .ForMember(dest => dest.TransactionCode, opts => opts.MapFrom(src => src.Description))
                    ;
            }

            string MapDescription(string description, string merchant)
            {
                return string.IsNullOrWhiteSpace(merchant) ? description : merchant;
            }
        }

        private class CardHolderMapProfile : Profile
        {
            public CardHolderMapProfile()
            {
                CreateMap<Application, RegisterCardholderRequest>()
                    .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Address.AddressLine1))
                    .ForMember(dest => dest.Address2, opts => opts.MapFrom(src => src.Address.AddressLine2))
                    .ForMember(dest => dest.City, opts => opts.MapFrom(src => src.Address.City))
                    .ForMember(dest => dest.PostalCode, opts => opts.MapFrom(src => src.Address.PostalCode))
                    .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Address.Country))
                    .ForMember(dest => dest.DateOfBirth, opts => opts.MapFrom(src => Convert.ToDateTime(src.DateOfBirth).ToString("MM/dd/yyyy")))
                    .ForMember(dest => dest.Gender, opts => opts.Ignore())
                    ;

                CreateMap<EmbilyDocument, KoKardDocument>()
                    .ForMember(dest => dest.TypeID, opts => opts.MapFrom(src => MapTypeID(src.DocumentType)))
                    .ForMember(dest => dest.FileType, opts => opts.MapFrom(src => src.FileType.Replace("application/", ".").Replace("image/", ".")))
                    .ForMember(dest => dest.IDNumber, opts => opts.MapFrom(src => ""))
                    ;
            }

            private string MapTypeID(DocumentTypes documentType)
            {
                /* 

                Proof of Identification Type IDs

                1 Driver's License Driver's License 
                2 Passport Passport 
                3 ID Card ID Card 
                4 Matricula Consular Matricula Consular 
                5 IFE Voter Card IFE Voter Card 
                6 Military ID Military ID 
                7 State ID/Government Work Card State ID/Government Work Card 
                8 SSN Social Security Number 
                9 SP Selfie Photo 
                10 SS Specimen Signature 
                15 SV Selfie Video 

                Proof of Address TypeIDs    

                11 Recent Utility Bill Recent Utility Bill 
                12 Mortgage Statement Mortgage Statement 
                13 Tennancy Agreement Tennancy Agreement 
                14 Bank or Credit Card Statement Bank or Credit Card Statement

                */

                switch (documentType)
                {
                    case DocumentTypes.ProofOfID:
                        return "2";
                    case DocumentTypes.ProofOfAddress:
                        return "11";
                    case DocumentTypes.Selfie:
                        return "9";
                    default:
                        throw new NotSupportedException($"{documentType} unsupported");
                }
            }
        }

        private readonly EmbilyDbContext _ctx;
        private readonly KoKardAPI _api;
        private readonly IMapper _mapper;
        private readonly ILogger<KoKardCard> _logger;

        public KoKardCard(EmbilyDbContext ctx, IConfiguration configuration, IMapper mapper, ILogger<KoKardCard> logger)
        {
            var url = configuration["KoKard_url"];
            var apiId = configuration["KoKard_apiId"];
            var apiKey = configuration["KoKard_apiKey"];

            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
            _api = new KoKardAPI(url, apiId, apiKey);
        }

        public KoKardCard(string url, string apiId, string apiKey)
        {
            _api = new KoKardAPI(url, apiId, apiKey);
        }

        public async Task<double> GetCardBalanceAsync(string providerUserId, string providerAccountNumber)
        {
            var response = await _api.GetCardBalanceAsync(providerAccountNumber);
            return response.TableResult.CardBalanceTable[0].Amount;
        }

        public async Task<IList<TransactionInfo>> GetTransactionsAsync(string providerUserId, string providerAccountNumber)
        {
            var response = await _api.GetTransactionHistoryAsync(providerAccountNumber);
            var txns = _mapper.Map<IList<TransactionInfo>>(response.TableResult.TransactionTable);
            return txns;
        }

        public async Task LoadCardAsync(string providerUserId, string providerAccountNumber, double amount, string transactionNumber)
        {
            await _api.CreditFundsAsync(providerAccountNumber, amount);
        }

        public async Task<string> RegisterWithDocs(Application application)
        {
            var request = _mapper.Map<Application, RegisterCardholderRequest>(application);

            _logger.LogDebug($"Request: {JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings { MaxDepth = 1 })}");

            var response = await _api.RegisterCardholderAsync(request);
            var cardHolderID = response.AdditionalInfo.CardHolderID;
            return cardHolderID;
        }

        public async Task AssignCard(string cardHolderRef, string cardRef)
        {
            await _api.AddCardholderCardAsync(cardHolderRef, cardRef);
        }
    }
}
