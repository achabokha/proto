using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public abstract class BaseEntity
    {
        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
