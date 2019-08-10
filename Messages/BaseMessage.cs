using System;

namespace Messages
{
    public class BaseMessage
    {
        /// <summary>
        /// Just random Id to identify the message 
        /// </summary>
        public string MessageId { get { return Guid.NewGuid().ToString(); } }

        public DateTimeOffset DateCreated { get { return DateTimeOffset.UtcNow; } }

        /// <summary>
        /// Transaction Id in Embily platform --
        /// </summary>
        public string TxnId { get; set; }

        /// <summary>
        /// human readable Transaction Number for troubleshooting convenience 
        /// </summary>
        public string TransactionNumber { get; set; }
    }
}
