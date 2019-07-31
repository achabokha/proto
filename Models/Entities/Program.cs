using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Embily.Models
{
    public class Program : BaseEntity
    {
        // for example, "sandbox"
        public string ProgramId { get; set; } 

        public string Domain { get; set; }

        public string Title { get; set; }

        // a place to store a JSON with program settings --
        public string Settings { get; set; }
    }
}
