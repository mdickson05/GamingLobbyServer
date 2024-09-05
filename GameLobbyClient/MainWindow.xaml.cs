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
        private IGLSInterface foob;
        /* 
         * Simply uses MainFrame in .xaml and loads pages
         * Loads LoginPage as startup page
         * Other page logic handles switching between
         */
        public MainWindow()
        {
            InitializeComponent();
            //Creating client service
            ChannelFactory<DataServer.IGLSInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/GamingLobbyService";
            foobFactory = new ChannelFactory<IGLSInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            MainFrame.Navigate(new LoginPage(foob));
        }
    }
}
