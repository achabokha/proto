using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.Controllers.Api.v2.Models
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<Embily.Models.Application, Application>()
                .ForMember(dest => dest.NationalityISO3, opts => opts.MapFrom(src => src.Nationality))
                .ForMember(dest => dest.Documents, opts => opts.MapFrom(src => src.Documents))
                ;
            CreateMap<Embily.Models.Address, Address>();

            CreateMap<Application, Embily.Models.Application>()
                .ForMember(dest => dest.Nationality, opts => opts.MapFrom(src => src.NationalityISO3))
                .ForMember(dest => dest.Documents, opts => opts.MapFrom(src => src.Documents))
                ;
            CreateMap<Address, Embily.Models.Address>();
        }
    }

    public class Application
    {
        public string ApplicationId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string NationalityISO3 { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        public Address ShippingAddress { get; set; }

        public Document[] Documents { get; set; }
    }

    public class Address
    {
        public string AddressId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public string Province { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
