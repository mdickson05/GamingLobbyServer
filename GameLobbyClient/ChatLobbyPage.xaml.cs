﻿using DataServer;
using System;
﻿using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
using System.Diagnostics;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

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
            RefreshChatLobby();
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
                PrivateMessagePage privateMessagePage = new PrivateMessagePage(_username, privateLobbyName, _client);
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
            var messages = _client.GetRoomMessages(_lobbyName, false);
            var users = _client.GetRoomUsers(_lobbyName, false);

            // Create a list to hold the parsed ChatMessage objects
            var parsedMessages = new List<ChatMessage>();

            // Iterate through each message and create ChatMessage objects
            foreach (var message in messages)
            {
                ChatMessage chatMessage = new ChatMessage();

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

            // Update the UI by setting the ItemsSource for the chat history and user list
            ChatHistoryBox.ItemsSource = parsedMessages;  
            UserListBox.ItemsSource = users;                     
        }

        public class ChatMessage
        {
            public string MessageText { get; set; }
            public string Hyperlink { get; set; }
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