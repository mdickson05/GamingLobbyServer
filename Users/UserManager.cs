using System;
using System.Collections.Generic;

namespace Users
{
    public class UserManager
    // Class used to manage users without directly accessing components
    {
        private Dictionary<string, User> Users { get; set; }

        public UserManager()
        {
            // statically call UserGenerator to generate the five users needed to populate a server
            this.Users = UserGenerator.GenerateUsers();
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
            LogDictionaryState();
            return true;
        }
        // The function calling Logout should check whether the user calling it is actually logged in
        public void Logout(string username)
        {
            if (Users.ContainsKey(username))
            {
                Users.Remove(username);
            }
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



        public bool InRoom(string roomname, string username)
        {
            if(!Users.ContainsKey(username))
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
