using System;
using System.Collections.Generic;

namespace Users
{
    public class UserManager
    // Class used to manage users without directly accessing components
    {
        private Dictionary<string, User> Users { get; set; }
        private List<string> LoggedInUsers { get; set; }

        public UserManager()
        {
            this.Users = new Dictionary<string, User>();
            this.LoggedInUsers = new List<string>();
        }

        public bool LoggedInElsewhere(String username)
        {
            return LoggedInUsers.Contains(username);
        }

        // returns a boolean just in case extra error checking is required down the track
        public bool CreateNewUser(string username)
        {
            if (Users.ContainsKey(username))
            {
                return false;
            }

            User User = new User(username);
            Users.Add(username, User);
            Login(username);

            LogDictionaryState();
            return true;
        }

        public bool Login(string username)
        {
            if (Users.ContainsKey(username) && !LoggedInUsers.Contains(username))
            {
                LoggedInUsers.Add(username);
                return true;
            }
            return false;
        }

        public void Logout(string username)
        {
            LoggedInUsers.Remove(username);
        }

        public void AddToRoomsList(string roomname, string username)
        {
            User user = GetUserByName(username);
            user.Rooms.Add(roomname);
        }

        public void DeleteFromRoomsList(string roomname, string username)
        {
            User user = GetUserByName(username);
            user.Rooms.Remove(roomname);
        }

        public bool UserAlreadyExists(String username)
        {
            return Users.ContainsKey(username);
        }

        public bool InRoom(string roomname, string username)
        {
            if (!Users.ContainsKey(username))
            {
                Console.WriteLine($"Error: User '{username}' not found in Users dictionary.");
                return false;
            }

            User user = Users[username];

            List<string> userRoomsList = user.Rooms;

            if (roomname != null && userRoomsList.Contains(roomname))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private User GetUserByName(string username)
        {
            return Users[username];
        }

        private void LogDictionaryState()
        {
            Console.WriteLine("Initial state of Users dictionary:");
            foreach (var kvp in Users)
            {
                Console.WriteLine($"Username: {kvp.Key}, User Object: {kvp.Value}");
            }
        }
    }
}
