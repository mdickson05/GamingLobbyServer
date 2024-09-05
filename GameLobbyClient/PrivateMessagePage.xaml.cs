using DataServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PrivateMessagePage.xaml
    /// </summary>
    public partial class PrivateMessagePage : Page
    {
        private IGLSInterface _client;
        private string _username;
        private string _lobbyName;
        /*
         * Might take in person object
         * Will make similar to ChatLobbyPage for now
         */
        public PrivateMessagePage(string username, string privateLobbyName, IGLSInterface client)
        {
            InitializeComponent();
            _client = client;
            _username = username;
            _lobbyName = privateLobbyName;
            PrivateNameBlock.Text = $"PM: {privateLobbyName}";
            DataContext = this; // Review in ChatLobbyPage for details
            RefreshPrivateMessage();
        }

        /*
         * User send message button, takes input from box and saves into collection
         */
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string message = UserInputBox.Text;
            _client.SendMessage(_lobbyName, _username, message, true);
            RefreshPrivateMessage(); //May remove as it helps auto refresh chat
            UserInputBox.Clear();
        }

        /*
         * KeyDown method to allow Enter key to be used to send message
         * instead of button click directly
         */
        private void UserInputBox_KeyDown(object sender, KeyEventArgs key)
        {
            if (key.Key == Key.Enter)
            {
                SendMessageButton_Click(sender, key);
            }
        }

        /*
         * Copied above function just to test SendMessageButton_Click
         * Make popout warning to say already in private message
         */
        private void PrivateMessageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Error: Already in private messages.");
        }

        /*
         * Simple logout button returns to LoginPage
         */
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _client.LeaveRoom(_lobbyName, _username, true);
            _client.Logout(_username);
            LoginPage loginPage = new LoginPage(_client);
            NavigationService.Navigate(loginPage);
        }

        /*
         * Simple back button to return to ChatLobbyPage
         */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _client.LeaveRoom(_lobbyName, _username, true);
            NavigationService.GoBack();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshPrivateMessage();
            MessageBox.Show("Lobby refreshed!"); //Delete this later, just emphasizing the refresh
        }

        /*
         * Simple refresh method that obtains the lists and sets the item sources for
         * both boxes.
         */
        private void RefreshPrivateMessage()
        {
            var messages = _client.GetRoomMessages(_lobbyName, true);
            var users = _client.GetRoomUsers(_lobbyName, true);

            ChatHistoryBox.ItemsSource = messages;
            UserListBox.ItemsSource = users;
        }
    }
}
