using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Chat
{
	public class ChatMessageSeen
	{
		[Key]
		public string Id { get; set; }
		public ApplicationUser User { get; set; }
		public DateTime? DateSeen { get; set; }
	}
}