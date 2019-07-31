using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Messages
{
    public class NotifyEmail : BaseMessage
    {
        public string Name { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string Template { get; set; }

        public IDictionary<string, string> FieldDict { get; set; }
    }
}
