using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Embily.Gateways.CCSPrepay;
using Embily.Gateways.CCSPrepay.Models;
using AutoMapper;
using System.Globalization;
using System.Linq;

namespace Embily.Services
{
    public class CCSPrepayCard : ICardLoad, ICard
    {
        CCSPrepayAPI _api;
        private readonly IMapper _mapper;

        /// <summary>
        /// based on; 
        /// https://github.com/AutoMapper/AutoMapper/blob/master/docs/Custom-type-converters.md
        /// </summary>
        private class DateTimeTypeConverter : ITypeConverter<string, DateTime>
        {
            public DateTime Convert(string source, DateTime destination, ResolutionContext context)
            {
                DateTimeFormatInfo ukDtfi = new CultureInfo("en-GB", false).DateTimeFormat;
                return System.Convert.ToDateTime(source, ukDtfi);
            }
        }

        private class TransactionMapProfile : Profile
        {
            public TransactionMapProfile()
            {
                CreateMap<TransactionHistoryData, TransactionInfo>()
                    .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.CardAccNameAddress));
                CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
            }
        }

        public CCSPrepayCard()
        {
            _api = new CCSPrepayAPI(
                Environment.GetEnvironmentVariable("CCSPrepay_username"),
                Environment.GetEnvironmentVariable("CCSPrepay_password")
            );
        }

        public CCSPrepayCard(IMapper mapper)
        {
            _mapper = mapper;
            _api = new CCSPrepayAPI(
                Environment.GetEnvironmentVariable("CCSPrepay_username"),
                Environment.GetEnvironmentVariable("CCSPrepay_password")
            );
        }

        public async Task<double> GetCardBalanceAsync(string providerUserId, string providerAccountNumber)
        {
            var request = new CardBalanceRequest
            {
                ClientId = Convert.ToInt64(providerUserId)
            };

            var response = await _api.CardBalanceAsync(request);

            return Convert.ToDouble(response.Response.Data.AvailableBalance);
        }

        public async Task<IList<TransactionInfo>> GetTransactionsAsync(string providerUserId, string providerAccountNumber)
        {
            var request = new TransactionHistoryRequest
            {
                ClientId = Convert.ToInt64(providerUserId),
                StartDate = DateTime.UtcNow.AddDays(-90).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                EndDate = DateTime.UtcNow.AddDays(1).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"),
                PageNumber = 1,
            };
            var response = await _api.TransactionHistoryAsync(request);

            var txns = _mapper.Map<IList<TransactionInfo>>(response.Response.Data);
            return txns.OrderByDescending(t => t.TransactionDate).ToList();
        }

        public async Task LoadCardAsync(string providerUserId, string providerAccountNumber, double amount, string transactionId)
        {
            var request = new LoadCardRequest
            {
                ClientId = Convert.ToInt64(providerUserId),
                Amount = amount,
                TransactionId = transactionId,
            };

            var response = await _api.LoadCardAsync(request);
        }
    }
}