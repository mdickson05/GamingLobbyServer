using System.Collections.Generic;

namespace Rooms
{
    public class RoomManager
    {
        public Dictionary<string, Room> RoomsList { get; set; } // includes a list of all rooms in the Server
        public Dictionary<string, Room> PrivateRoomsList { get; set; } //List for private rooms, needed for private messages
        public RoomManager()
        {
            this.RoomsList = new Dictionary<string, Room>();
            this.PrivateRoomsList = new Dictionary<string, Room>();
        }


        // logic for creating new room
        // conditions: room cannot already exist
        // return boolean for extra error handling abilities
        public bool CreateNewRoom(string roomName)
        {
            if (!RoomsList.ContainsKey(roomName))
            {
                Room newRoom = new Room(roomName);
                RoomsList.Add(roomName, newRoom);
                return true;
            }
            else
            {
                return false;
            }
        }

        // logic for creating new room
        // conditions: room cannot already exist
        // return boolean for extra error handling abilities
        public bool CreateNewPrivateRoom(string roomName)
        {
            if (!PrivateRoomsList.ContainsKey(roomName))
            {
                Room newRoom = new Room(roomName);
                PrivateRoomsList.Add(roomName, newRoom);
                return true;
            }
            else
            {
                return false;
            }
        }

        // logic for deleting room
        // conditions: no users can be in a room for it to be deleted.
        // return boolean for extra error handling abilities
        public bool DeleteRoom(string roomName)
        {
            Room toDelete = RoomsList[roomName];

            if (toDelete != null && toDelete.Users.Count == 0)
            {
                RoomsList.Remove(roomName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddToRoom(string roomname, string username)
        {
            Room room = GetRoomByName(roomname);
            room.Users.Add(username);
        }

        public void AddToPrivateRoom(string roomname, string username)
        {
            Room room = GetPrivateRoomByName(roomname);
            room.Users.Add(username);
        }

        public void DeleteFromRoom(string roomname, string username)
        {
            Room room = GetRoomByName(roomname);
            room.Users.Remove(username);
        }

        public void DeleteFromPrivateRoom(string roomname, string username)
        {
            Room room = GetPrivateRoomByName(roomname);
            room.Users.Remove(username);
        }

        public Room GetRoomByName(string roomname)
        {
            return RoomsList[roomname];
        }
        public Room GetPrivateRoomByName(string roomname)
        {
            return PrivateRoomsList[roomname];
        }
    }
}
