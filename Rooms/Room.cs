using System.Collections.Generic;
using Messages;

namespace Rooms
{
    public class Room
    {
        public string Roomname { get; set; } // includes the name of the room
        public List<string> Users { get; set; } // as well as all users in the room
        public List<string> Messages { get; set; } // Stores all room messages
        public List<Message> ParsedMessages { get; set; }

        public Room(string roomName)
        {
            this.Roomname = roomName;
            this.Users = new List<string>();
            this.Messages = new List<string>();
            this.ParsedMessages = new List<Message>();
        }
    }
}
