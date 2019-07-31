using Embily.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmbilyAdmin.ViewModels
{
    public class ProgramViewModel
    {
        [Required]
        public string ProgramId { get; set; }

        public string Domain { get; set; }

        public string Title { get; set; }

        // a place to store a JSON with program settings --
        public string Settings { get; set; }
    }
}