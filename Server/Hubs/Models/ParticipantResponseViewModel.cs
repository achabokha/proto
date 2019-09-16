using System.Collections.Generic;

namespace Server.Hubs.Models
{
	public class ParticipantResponseViewModel
	{
		public IList<ChatParticipantViewModel> Participants { get; set; }
		public ParticipantMetadataViewModel Metadata { get; set; }
	}
}