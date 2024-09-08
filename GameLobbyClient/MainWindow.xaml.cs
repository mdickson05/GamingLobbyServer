using DataServer;
using System.ServiceModel;
using System.Windows;

namespace GameLobbyClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

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
