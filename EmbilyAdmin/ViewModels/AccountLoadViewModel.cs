using System.ComponentModel.DataAnnotations;

namespace EmbilyAdmin.ViewModels
{
    public class AccountLoadViewModel
    {
        [Required]
        public string AccountId { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}