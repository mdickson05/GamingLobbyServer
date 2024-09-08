using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

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
