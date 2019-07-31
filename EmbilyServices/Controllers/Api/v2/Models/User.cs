using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Embily.Models;

namespace EmbilyServices.Controllers.Api.v2.Models
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<ApplicationUser, User>()
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => "Active"));

            CreateMap<User, ApplicationUser>();
        }
    }

    /// <summary>
    /// Basic User Information 
    /// </summary>
    /// <remarks>
    /// First name, last name and email, this is all required to register a user. That's it. 
    /// </remarks>
    public class User
    {
        /// <summary>
        /// User Id
        /// </summary>
        /// <remarks>
        /// When creating a User, User Id will be returned upon successful user creation successfully. 
        /// When updating user information via PUT User Id should be present. 
        /// </remarks>
        public string UserId { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; }

        public string Status { get; set; }
    }
}
