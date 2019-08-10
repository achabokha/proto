using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class Notify : BaseMessage
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string Template { get; set; }

        public IDictionary<string, string> FieldDict { get; set; }
    }
}
