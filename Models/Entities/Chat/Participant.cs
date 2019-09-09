using System;

namespace Models.Entities
{
    public class Participant
    {
        public Guid Id { get; private set; } 
        public ChatGroup Group {get; set;}
        public ApplicationUser User {get; set;}
    }
}