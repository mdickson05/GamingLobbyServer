using DataServer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
                var messages = await Task.Run(() => _client.GetParsedRoomMessages(_lobbyName, false));
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
            return string.Join("-", users);
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
            RefreshChatLobby();
            MessageBox.Show("Lobby refreshed!"); //Delete this later, just emphasizing the refresh
        }

        /*
         * Simple refresh method that obtains the lists and sets the item sources for
         * both boxes.
         */
        private void RefreshChatLobby()
        {
            // Retrieve messages and users from the client
            var parsedMessages = _client.GetParsedRoomMessages(_lobbyName, false);
            var users = _client.GetRoomUsers(_lobbyName, false);

            ChatHistoryBox.ItemsSource = parsedMessages;
            UserListBox.ItemsSource = users;
        }

        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|Image files (*.PNG; *.JPG)|*.png; *.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                _client.SendMessage(_lobbyName, _username, fileName, false);
                RefreshChatLobby();
            }
        }

        // Event handler for hyperlinks
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Open the link in the default browser
            string pathname = e.Uri.AbsolutePath;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = System.IO.Path.GetFileName(pathname); // Default filename is the same as the source file
            saveFileDialog.Filter = "Text files(*.txt) | *.txt | Image files(*.PNG; *.JPG)| *.png; *.jpg";  // Can adjust filters for specific file types

            // If the user selects a location and clicks Save
            if (saveFileDialog.ShowDialog() == true)
            {
                string destinationPath = saveFileDialog.FileName;

                try
                {
                    if (pathname.StartsWith("http") || pathname.StartsWith("ftp"))
                    {
                        // If it's a web URL, download the file
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile(pathname, destinationPath);
                        }
                    }
                    else if (File.Exists(pathname))
                    {
                        // If it's a local file path, copy it to the destination
                        File.Copy(pathname, destinationPath, true);
                    }

                    MessageBox.Show("File downloaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    System.Diagnostics.Process.Start(destinationPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while downloading the file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Prevent further navigation
            e.Handled = true;
        }
    }
}