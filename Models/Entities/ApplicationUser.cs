using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime DateLastAccessed { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
