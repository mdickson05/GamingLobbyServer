using Rooms;
using System.Collections.Generic;

namespace Users
{
    public class User
    {
        public string Username { get; set; }
        public List<Room> Rooms { get; set; }

        public User(string username)
        {
            this.Username = username;
            this.Rooms = new List<Room>(); // no room yet
        }
    }
}
