using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public class ChatGroup
    {
        public Guid Id {get; private set;}
        public DateTime DateCreated {get; set;}
        public string Title {get; set;}
        public ICollection<Participant> Participants {get; set; }

    }
}