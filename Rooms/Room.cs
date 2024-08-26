using System.Collections.Generic;

namespace Rooms
{
    public class Room
    {
        public string Roomname { get; set; } // includes the name of the room
        public List<string> Users { get; set; } // as well as all users in the room

        public Room(string roomName)
        {
            this.Roomname = roomName;
            this.Users = new List<string>();
        }
    }
}
