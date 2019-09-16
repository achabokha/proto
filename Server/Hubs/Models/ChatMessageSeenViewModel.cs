using System;

namespace Server.Hubs.Models
{
    public class ChatMessageSeenViewModel
    {
        public string UserId;
        public DateTime? DateSeen;

        public string msgId;
    }
}