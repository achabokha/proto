using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EmbilyAdmin.ViewModels;
using Embily.Models;

namespace EmbilyAdmin.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed {get; set;}
        public bool EmailConfirmed {get; set;}
        public string DateLastAccessed { get; internal set; }
        public long UserNumber { get; internal set; }

        public List<string> Roles {get; set;}
        public List<RoleViewModel> Transactions {get; set;}

        public IEnumerable<Account> Accounts {get; set;}

        public virtual List<Transaction> AccountsTransactions { get; set; }
        public string ProgramId { get; set; }
    }
}
