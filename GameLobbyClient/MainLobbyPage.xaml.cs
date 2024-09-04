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
using DataServer;

namespace GameLobbyClient
{
    /// <summary>
    /// Interaction logic for MainLobbyPage.xaml
    /// </summary>
    public partial class MainLobbyPage : Page
    {
        //Counts total lobbies to max of 5
        private int lobbyCount = 0;
        //String username temp which may be replaced by class use
        private string userName;

        private IGLSInterface _client;

        public MainLobbyPage(IGLSInterface client, string username)
        {
            InitializeComponent();
            userName = username;
            _client = client;

            //Redundant lines for testing
            UsernameBlock.Text = $"Welcome, {userName}";
            UsernameBlock.Visibility = Visibility.Visible;
        }

        /* 
         * Create lobby button
         * Checks lobbyCount to ensure max hasn't been hit
         * Checks for non null or empty input
         * Switchcase handles which button to use/name
         * One final check to ensure lobbyButton isn't null before setting
         */
        private void CreateLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            string lobbyName = LobbyNameBox.Text;
            if (lobbyCount >= 5)
            {
                MessageBox.Show("Maximum number of lobbies reached.");
            }
            else if (string.IsNullOrEmpty(lobbyName))
            {
                MessageBox.Show("Please enter a lobby name.");
            }

            //Handles making lobbies visible based on count
            else
            {
                lobbyCount++;
                Button lobbyButton = null;

                switch (lobbyCount)
                {
                    case 1:
                        lobbyButton = LobbyButtonOne;
                        break;

                    case 2:
                        lobbyButton = LobbyButtonTwo;
                        break;

                    case 3:
                        lobbyButton = LobbyButtonThree;
                        break;

                    case 4:
                        lobbyButton = LobbyButtonFour;
                        break;

                    case 5:
                        lobbyButton = LobbyButtonFive;
                        break;
                }

                //Sets name and makes visible
                if (lobbyButton != null)
                {
                    lobbyButton.Content = lobbyName;
                    lobbyButton.Visibility = Visibility.Visible;
                    _client.CreateRoom(lobbyName);
                }

                //Clears LobbyNameBox of previous text
                LobbyNameBox.Clear();
            }
        }

        /*
         * Logs user out and takes them back to LoginPage
         * Removes their username from UserManager
         */
        private void LogoutMainLobby_Click(object sender, RoutedEventArgs e)
        {
            _client.Logout(userName);
            LoginPage loginPage = new LoginPage(_client);
            NavigationService.Navigate(loginPage);
        }

        /*
         * Each button click is represented by what lobby it is.
         * It takes the user-named lobby and passes it to a ChatLobbyPage
         * Which then we navigate to through NavigationService
         */
        private void LobbyButtonOne_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonOne.Content.ToString();
            ChatLobbyPage lobbyOne = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyOne);
        }

        private void LobbyButtonTwo_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonTwo.Content.ToString();
            ChatLobbyPage lobbyTwo = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyTwo);
        }

        private void LobbyButtonThree_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonThree.Content.ToString();
            ChatLobbyPage lobbyThree = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyThree);
        }

        private void LobbyButtonFour_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonFour.Content.ToString();
            ChatLobbyPage lobbyFour = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyFour);
        }

        private void LobbyButtonFive_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonFive.Content.ToString();
            ChatLobbyPage lobbyFive = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyFive);
        }
    }
}
