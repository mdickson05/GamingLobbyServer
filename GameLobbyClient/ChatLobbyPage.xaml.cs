using DataServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace GameLobbyClient
{
    /// <summary>
    /// Interaction logic for ChatLobbyPage.xaml
    /// </summary>
    public partial class ChatLobbyPage : Page
    {
        private string _username;
        private string _lobbyName;
        private IGLSInterface _client;

        /*
         * Creates instances of both lists and adds some test strings
         * 
         * DataContext is how "binding" works inside the xml
         * We bind the list to a specific object/thing
         * This will be replaced later when we incorporate classes and instances
         * 
         */
        public ChatLobbyPage(string lobbyName, IGLSInterface client, string username)
        {
            InitializeComponent();
            DataContext = this; // Review above comment block
            _client = client;
            _username = username;
            _lobbyName = lobbyName;
            LobbyNameBlock.Text = _lobbyName; //Sets lobby name

            InitializeBackgroundTasks();
        }

        private void InitializeBackgroundTasks()
        {
            Task.Run(async () => 
            {
                while (true)
                {
                    await UpdateMessages();
                    await Task.Delay(1000); // Update every second
                }
            });

            Task.Run(async () =>
            {
                while (true)
                {
                    await UpdateUserList();
                    await Task.Delay(5000); // Update every second
                }
            });


        }

        private async Task UpdateMessages()
        {
            try
            {
                var messages = await Task.Run(() => _client.GetRoomMessages(_lobbyName, false));
                await Dispatcher.InvokeAsync(() =>
                {
                    ChatHistoryBox.ItemsSource = messages;
                });
            }
            catch (Exception ex)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show($"Error updating messages: {ex.Message}");
                });
            }
        }

        private async Task UpdateUserList()
        {
            try
            {
                var users = await Task.Run(() => _client.GetRoomUsers(_lobbyName, false));
                await Dispatcher.InvokeAsync(() =>
                {
                    UserListBox.ItemsSource = users;
                });
            }
            catch (Exception ex)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show($"Error updating user list: {ex.Message}");
                });
            }
        }

        /*
         * User send message button, takes input from box and saves into collection
         */
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string message = UserInputBox.Text;
            _client.SendMessage(_lobbyName, _username, message, false);
            UserInputBox.Clear();
            RefreshChatLobby(); //Might remove this
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
         * Handles clicking someones name to message
         * UserMessageName is the person you want to message name
         */
        private void PrivateMessageButton_Click(object sender, RoutedEventArgs e)
        {
            Button tempButton = sender as Button;
            string userMessageName = tempButton.Content.ToString();
            if (userMessageName == _username)
            {
                MessageBox.Show("You can't private message yourself.");
            }
            else
            {
                string privateLobbyName = makePrivateChatName(_username, userMessageName); //Created a private lobby name of current user and who they pm concatted
                _client.CreatePrivateRoom(privateLobbyName);
                _client.JoinRoom(privateLobbyName, _username, true);
                PrivateMessagePage privateMessagePage = new PrivateMessagePage(_username, privateLobbyName, userMessageName, _client);
                NavigationService.Navigate(privateMessagePage);
            }
        }

        /*
         * Simple method to make a consistent private lobby name
         * Makes server handling easier
         */
        private string makePrivateChatName(string userNameOne, string userNameTwo)
        {
            var users = new List<string> { userNameOne, userNameTwo };
            users.Sort();
            return string.Join("", users);
        }

        /*
         * Simple logout button returns to LoginPage
         */
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _client.LeaveRoom(_lobbyName, _username, false);
            _client.Logout(_username);
            LoginPage loginPage = new LoginPage(_client);
            NavigationService.Navigate(loginPage);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _client.LeaveRoom(_lobbyName, _username, false);
            NavigationService.GoBack();
        }

        /*
         * Simple refresh button pulls information from server
         */
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                await UpdateMessages();
                await UpdateUserList();
                await Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show("Lobby refreshed!"); // Delete later?
                });
            });
        }

        /*
         * Simple refresh method that obtains the lists and sets the item sources for
         * both boxes.
         */
        private void RefreshChatLobby()
        {
            var messages = _client.GetRoomMessages(_lobbyName, false);
            var users = _client.GetRoomUsers(_lobbyName, false);

            ChatHistoryBox.ItemsSource = messages;
            UserListBox.ItemsSource = users;
        }
    }
}