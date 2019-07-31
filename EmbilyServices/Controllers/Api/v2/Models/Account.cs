using AutoMapper;
using Embily.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.Controllers.Api.v2.Models
{
    public class AccounMappingProfile : Profile
    {
        public AccounMappingProfile()
        {
            CreateMap<Embily.Models.Account, Account>();
            CreateMap<Account, Embily.Models.Account>();
        }
    }

    public class Account
    {
        public string AccountId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyCodes CurrencyCode { get; set; }
    }
}
