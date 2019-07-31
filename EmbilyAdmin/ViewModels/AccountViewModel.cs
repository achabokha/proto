using Embily.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmbilyAdmin.ViewModels
{
    public class AccountViewModel
    {
        [Required]
        public string AccountId { get; set; }
       
        public string CurrencyCodeString { get; set; }

        public CurrencyCodes CurrencyCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}