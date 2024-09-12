using Messages;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace DataServer
{
    [ServiceContract]
    public interface IGLSInterface
    {
        // OperationContract tag to define as function in the contract
        [OperationContract]
        void CreateUser(string username);
        [OperationContract]
        void Logout(string username);
        [OperationContract]
        void CreateRoom(string roomname);
        [OperationContract]
        void CreatePrivateRoom(string roomname);
        [OperationContract]
        int GetRoomCount();
        [OperationContract]
        void DeleteRoom(string roomname);
        [OperationContract]
        void JoinRoom(string roomname, string username, Boolean isPrivate);
        [OperationContract]
        void LeaveRoom(string roomname, string username, Boolean isPrivate);
        [OperationContract]
        List<string> GetRoomUsers(string roomname, Boolean isPrivate);
        [OperationContract]
        List<string> GetRoomMessages(string roomname, Boolean isPrivate);

        [OperationContract]
        List<string> GetAvailableLobbies();
        [OperationContract]
        List<string> GetAvailablePrivateLobbies();
        [OperationContract]
        void SendMessage(string roomname, string username, string message, Boolean isPrivate);
        [OperationContract]
        List<Message> GetParsedRoomMessages(string roomName, bool isPrivate);
        [OperationContract]
        bool AlreadyLoggedIn(string username);
        [OperationContract]
        bool Login(String username);
        [OperationContract]
        bool AlreadyExists(String username);

    }
}
