using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.ViewModels
{
    public class DetectedInBlockchainViewModel
    {
        [Required]
        public string TxnId { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}
