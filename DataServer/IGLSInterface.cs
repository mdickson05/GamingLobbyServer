using Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Users;

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
        void DeleteRoom(string roomname);
        [OperationContract]
        void JoinRoom(string roomname, string username);
        [OperationContract]
        void LeaveRoom(string roomname, string username);
    }
}
