using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Embily.Models
{
    public class AffiliateToken : BaseEntity
    {
        public string AffiliateTokenId { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        
        // configured as Unique Index in DbContext setup --
        [Required]
        public string Token { get; set; }

        [Required]
        public string NormalizedToken { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsSuspended { get; set; }

        public int Counter { get; set; }
    }
}
