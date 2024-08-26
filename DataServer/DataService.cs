using System;
using System.ServiceModel;

namespace DataServer
{
    internal class DataService
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to our gaming lobby server for our first assignment");
            Console.WriteLine("Attempting to create a gaming lobby service...");
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();
            // The server is bound to implementation of LobbyServer
            host = new ServiceHost(typeof(GLSImplementation));
            // STATIC IP USED as per assignment spec
            // 0.0.0.0 accepts all interfaces, and server will run on port 8100. 
            host.AddServiceEndpoint(typeof(GLSInterface), tcp, "net.tcp://0.0.0.0:8100/GamingLobbyService");
            // host is opened on the specified service endpoint
            host.Open();
            Console.WriteLine("Success; system online.");
            Console.ReadLine();
            // host is closed.
            host.Close();
        }
    }
}
