using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
	public abstract class BaseEntity
	{
		[Required]
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;

		public DateTime? DateModified { get; set; }

		public string Id { get; set; }
	}
}
