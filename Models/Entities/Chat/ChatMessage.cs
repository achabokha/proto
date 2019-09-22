using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Entities.Chat;

namespace Models.Entities
{
	public class ChatMessage
	{
		public Guid Id { get; private set; }
		public int Type { get; set; }
		public ApplicationUser FromUser { get; set; }

		[Required]
		public ChatGroup ChatGroup { get; set; }
		public string Message { get; set; }
		public DateTime? DateSent { get; set; }
		public string DownloadUrl { get; set; }
		public int? FileSizeInBytes { get; set; }

	}
	
}

