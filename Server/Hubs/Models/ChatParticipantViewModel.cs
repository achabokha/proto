namespace Server.Hubs.Models
{
	public class ChatParticipantViewModel
	{
		public ChatParticipantTypeEnum ParticipantType { get; set; }
		public string HubContextId { get; set; }
		public EnumChatParticipantStatus Status { get; set; }
		public string Avatar { get; set; }
		public string Email { get; set; }
		public string UserId { get; set; }
		public string DisplayName { get; set; }
	}

	public enum EnumChatParticipantStatus
	{
		Online,
		Busy,
		Away,
		Offline
	}
}