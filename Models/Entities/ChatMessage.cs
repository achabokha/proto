using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; private set; } 
        public int Type { get; set; }
        public string FromId { get; set; }

        public ApplicationUser FromUser {get; set; }

        public string ToId { get; set; }
        public ApplicationUser ToUser {get; set; }
        public string Message { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateSeen { get; set; }
        public string DownloadUrl { get; set; }
        public int? FileSizeInBytes { get; set; }
    }
}