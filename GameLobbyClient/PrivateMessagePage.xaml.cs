using DataServer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
using static GameLobbyClient.ChatLobbyPage;

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

            // Create a list to hold the parsed ChatMessage objects
            var parsedMessages = new List<ChatMessage>();

            // Iterate through each message and create ChatMessage objects
            foreach (var message in messages)
            {
                ChatMessage chatMessage = new ChatMessage();

                // Check if the message contains a file link or is a regular text message
                // Check if the message contains a file link or is a regular text message
                if (message.Contains(".txt") || message.Contains(".png") || message.Contains(".jpg"))
                {
                    string link = "";
                    string pattern = @"([a-zA-Z]:\\|\\\\|\/)([^\s\\/]+[\\/])*[^\s\\/]+\.\w+";
                    Regex fileRegex = new Regex(pattern);


                    // Find the first match in the input string
                    Match match = fileRegex.Match(message);
                    string username = message.Substring(0, match.Index).Trim();

                    // Check if a file path was found
                    if (match.Success)
                    {
                        link = match.Value;
                    }

                    chatMessage.Hyperlink = link;
                    chatMessage.MessageText = username;
                }
                else
                {
                    // Treat the message as a normal message
                    chatMessage.MessageText = message;
                }

                // Add the parsed message to the list
                parsedMessages.Add(chatMessage);
            }

            ChatHistoryBox.ItemsSource = messages;
            UserListBox.ItemsSource = users;
        }

        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|Image files (*.PNG; *.JPG)|*.PNG; *.JPG";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                _client.SendMessage(_lobbyName, _username, fileName, false);
                RefreshPrivateMessage();
            }
        }


        // Event handler for hyperlinks
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Open the link in the default browser
            string pathname = e.Uri.ToString();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = System.IO.Path.GetFileName(pathname); // Default filename is the same as the source file
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|Image files (*.png; *.jpg)|*.png; *.jpg";  // Can adjust filters for specific file types

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

