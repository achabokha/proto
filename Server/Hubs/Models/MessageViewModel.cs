using System;
using System.Collections.Generic;
using Models.Entities;
using Models.Entities.Chat;

namespace Server.Hubs.Models
{
	public class MessageViewModel
	{
		public int Type { get; set; }
		public string Id { get; set; }
		public ChatParticipantViewModel FromUser { get; set; }
		public string ToId { get; set; }

		public string GroupId { get; set; }

		public string Message { get; set; }
		public DateTime? DateSent { get; set; }
		public ICollection<ChatMessageSeenViewModel> DateSeen { get; set; }
		public string DownloadUrl { get; set; }
		public int? FileSizeInBytes { get; set; }
	}
}