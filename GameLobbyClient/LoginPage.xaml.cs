using DataServer;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GameLobbyClient
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private IGLSInterface _client;
        public LoginPage(IGLSInterface client)
        {
            InitializeComponent();
            _client = client;
        }

        /* 
         * Takes username from loginbox and passes to MainLobbyPage
         * CHANGE when classes are implemented
         */
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = LoginBox.Text;
            _client.CreateUser(username);
            NavigationService.Navigate(new MainLobbyPage(_client, username));
            
            /* Commented out testing code to implement server use.
          
            var username = LoginBox.Text;
            MainLobbyPage lobbyPage = new MainLobbyPage(username);
            NavigationService.Navigate(lobbyPage);
            */
        }
    }
}
