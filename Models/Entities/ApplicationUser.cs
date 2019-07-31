using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Embily.Models
{
    public class ApplicationUser : IdentityUser
    {

        /*
         # Tips 

         ### Set seeding via SQL 

         -- create sequence 

         CREATE SEQUENCE UserNumberSeq AS BIGINT START WITH 1000000009 INCREMENT BY 1
         
        -- alter to table 

         ALTER TABLE user
            ADD CONSTRAINT DF_UserNumber UserNumber
            DEFAULT (NEXT VALUE FOR UserNumberSeq) FOR [UserNumber]
        
        */

        public Int64 UserNumber { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime DateLastAccessed { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public string AffiliatedWithUserId { get; set; }

        public ApplicationUser AffiliatedWithUser { get; set; }

        public string AffiliateTokenUsed { get; set; }

        public string ProgramId { get; set; }

        public Program Program { get; set; }

    }
}
