using System.Collections.Generic;

namespace Users
{
    internal class User
    {
        public string Username { get; set; }
        public List<string> Rooms { get; set; }

        public User(string username)
        {
            this.Username = username;
            this.Rooms = new List<string>(); // no room yet
        }
    }
}
