using System.ComponentModel.DataAnnotations;

namespace EmbilyAdmin.ViewModels
{
    public class PageTableViewModel
    {
        [Required]
        public int Size { get; set; }
        [Required]
        public int TotalElements { get; set; }
        [Required]
        public int TotalPages { get; set; }
        [Required]
        public int PageNumber { get; set; }
    }
}