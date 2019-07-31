using System.ComponentModel.DataAnnotations;

namespace EmbilyServices.ViewModels
{
    public class SetPINViewModel
    {
        [Required]     
        public string CardReferenceID { get; set; }

        [Required]       
        public string NewPIN { get; set; }

        [Required]
        public string AccountId { get; set; }
    }
    
    public class ConfirmPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}