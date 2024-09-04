using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataServer;

namespace GameLobbyClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public IGLSInterface foob;
    public partial class MainWindow : Window
    {
        /* 
         * Simply uses MainFrame in .xaml and loads pages
         * Loads LoginPage as startup page
         * Other page logic handles switching between
         */
        public MainWindow()
        {
            InitializeComponent();
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/DataService";
            var chanFactory = new ChannelFactory<DataServer>(tcp, URL);
            foob = chanFactory.CreateChannel();
            MainFrame.Navigate(new LoginPage());

        }
    }
}
