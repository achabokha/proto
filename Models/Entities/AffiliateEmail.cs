using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Embily.Models
{
    public class AffiliateEmail : BaseEntity
    {
        public string AffiliateEmailId { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string Email { get; set; }
                
        // configured as Unique Index in DbContext setup --
        [Required]
        public string NormalizedEmail { get; set; }

        public bool HasRegistred { get; set; }

        public string RegistredUserId { get; set; }
    }
}

