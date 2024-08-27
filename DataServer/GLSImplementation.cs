using Rooms;
using System.ServiceModel;
using Users;

namespace DataServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class GLSImplementation
    {
        public RoomManager RoomManager { get; set; }

        public UserManager UserManager { get; set; }

        public GLSImplementation()
        {
            this.RoomManager = new RoomManager();
            this.UserManager = new UserManager();
        }

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

        public void DeleteRoom(string roomname)
        {
            RoomManager.DeleteRoom(roomname);
        }

        public void JoinRoom(string roomname, string username)
        {
            // If NOT already in room, join room
            if (!UserManager.InRoom(username, roomname))
            {
                RoomManager.AddToRoom(roomname, username);
                UserManager.AddToRoomsList(roomname, username);
            }
        }

        public void LeaveRoom(string roomname, string username)
        {
            if (UserManager.InRoom(username, roomname))
            {
                RoomManager.DeleteFromRoom(roomname, username);
                UserManager.DeleteFromRoomsList(roomname, username);
            }
        }

    }
}
