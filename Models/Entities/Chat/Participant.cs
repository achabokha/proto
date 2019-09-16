using System;

namespace Models.Entities
{
	public class Participant
	{
		public Guid Id { get; private set; }
		public ChatGroup Group { get; set; }
		public ApplicationUser User { get; set; }

		public int UnreadMessages { get; set; }

		public EnumParticipantStatus ParticipantStatus { get; set; }
		public EnumParticipantPermissionLevel PermissionLevel { get; set; }
	}

	public enum EnumParticipantStatus
	{
		approved,
		declined,
		pending
	}

	public enum EnumParticipantPermissionLevel
	{
		Read,
		Write,
		Admin
	}
}