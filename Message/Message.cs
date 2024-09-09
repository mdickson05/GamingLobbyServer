using System;
using System.Runtime.Serialization;

namespace Messages
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string MessageText { get; set; }
        [DataMember]
        public string Hyperlink { get; set; }

        public Message(String message)
        {
            this.MessageText = message;
            this.Hyperlink = "";
        }
    }
}
