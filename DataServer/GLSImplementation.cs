using Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Users;

namespace DataServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class GLSImplementation : IGLSInterface
    {
        private static readonly RoomManager roomManager = new RoomManager();
        private static readonly UserManager userManager = new UserManager();
        public RoomManager RoomManager => roomManager;
        public UserManager UserManager => userManager;

        public void CreateUser(string username)
        {
            UserManager.CreateNewUser(username);
        }

        public void Logout(string username)
        {
            UserManager.Logout(username);
        }

        public void CreateRoom(string roomname)
        {
            RoomManager.CreateNewRoom(roomname);
        }

        public void CreatePrivateRoom(string roomname)
        {
            RoomManager.CreateNewPrivateRoom(roomname);
        }

        public void DeleteRoom(string roomname)
        {
            RoomManager.DeleteRoom(roomname);
        }

        /*
         * Handles joining rooms
         * isPrivate dictates whether it's for a private or public lobby
         */
        public void JoinRoom(string roomname, string username, Boolean isPrivate)
        {
            if (isPrivate)
            {
                // If NOT already in room, join room
                if (!UserManager.InRoom(roomname, username))
                {
                    RoomManager.AddToPrivateRoom(roomname, username);
                    UserManager.AddToRoomsList(roomname, username);
                }
            }
            else
            {
                // If NOT already in room, join room
                if (!UserManager.InRoom(roomname, username))
                {
                    RoomManager.AddToRoom(roomname, username);
                    UserManager.AddToRoomsList(roomname, username);
                }
            }
        }
        /*
         * Handles leaving rooms
         * isPrivate dictates whether it's for a private or public lobby
         */
        public void LeaveRoom(string roomname, string username, Boolean isPrivate)
        {
            if (isPrivate)
            {
                if (UserManager.InRoom(roomname, username))
                {
                    RoomManager.DeleteFromPrivateRoom(roomname, username);
                    UserManager.DeleteFromRoomsList(roomname, username);
                }
            }
            else
            {
                if (UserManager.InRoom(roomname, username))
                {
                    RoomManager.DeleteFromRoom(roomname, username);
                    UserManager.DeleteFromRoomsList(roomname, username);
                }
            }
        }

        /*
         * Returns list of users
         * Checks RoomManager for name making sure its not null otherwise returning a new list
         * isPrivate dictates whether it's for a private or public lobby
         */
        public List<string> GetRoomUsers(string roomname, Boolean isPrivate)
        {
            if(isPrivate)
            {
                return RoomManager.GetPrivateRoomByName(roomname)?.Users ?? new List<string>();
            }
            else
            {
                return RoomManager.GetRoomByName(roomname)?.Users ?? new List<string>();
            } 
        }

        /*
         * Returns list of messages
         * Checks RoomManager for name making sure its not null otherwise returning a new list
         * isPrivate dictates whether it's for a private or public lobby
         */
        public List<string> GetRoomMessages(string roomname, Boolean isPrivate)
        {
            if(isPrivate)
            {
                return RoomManager.GetPrivateRoomByName(roomname)?.Messages ?? new List<string>();
            }
            else
            {
                return RoomManager.GetRoomByName(roomname)?.Messages ?? new List<string>();
            }
        }

        /*
         * Returns list of keys as list of strings
         * Used in MainLobbyPage
         */
        public List<string> GetAvailableLobbies()
        {
            return RoomManager.RoomsList.Keys.ToList();
        }

        /*
         * Returns list of keys as list of strings
         */
        public List<string> GetAvailablePrivateLobbies()
        {
            return RoomManager.PrivateRoomsList.Keys.ToList();
        }

        /*
         * Adds message to list for room
         * isPrivate dictates whether it's for a private or public lobby
         */
        public void SendMessage(string roomname, string username, string message, Boolean isPrivate)
        {
            if (isPrivate)
            {
                Room room = RoomManager.GetPrivateRoomByName(roomname);
                if (room != null)
                {
                    string formatMessage = $"{username}: {message}";
                    room.Messages.Add(formatMessage);
                }
            }
            else
            { 
                Room room = RoomManager.GetRoomByName(roomname);
                if (room != null)
                {
                    string formatMessage = $"{username}: {message}";
                    room.Messages.Add(formatMessage);
                }
            }
        }
    }
}
