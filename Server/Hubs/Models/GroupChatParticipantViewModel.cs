using System.Collections.Generic;

namespace Server.Hubs.Models
{
	public class GroupChatParticipantViewModel : ChatParticipantViewModel
	{
		public string GroupId { get; set; }
		public IList<ChatParticipantViewModel> ChattingTo { get; set; }
	}
}