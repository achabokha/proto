using Embily.Models;
using Embily.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.ViewModels
{
    public class AffiliateInviteViewModels
    {
        public string AffiliateEmailId { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string NormalizedEmail { get; set; }

        public bool HasRegistred { get; set; }

        public string RegistredUserId { get; set; }
        public string SatusColor { get; set; }
    }

    public class AffiliateTokenViewModels
    {
        public string AffiliateTokenId { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NormalizedToken { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsSuspended { get; set; }

        public int Counter { get; set; }

        public int CountRegistered { get; set; }

        public int CountApplied { get; set; }

        public int CountTransacting { get; set; }      
    }
}
