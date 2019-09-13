using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
	public class ChatGroup
	{
		public Guid Id { get; private set; }
		public DateTime DateCreated { get; set; }
		public string Title { get; set; }
		public EnumChatGroupParticipantType ParticipantType {get; set;}
		public ICollection<Participant> Participants { get; set; }



	}

	public enum EnumChatGroupParticipantType {
		user, 
		group
	}
}