using DataServer;
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
            if(key.Key == Key.Enter)
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
            var users = new List<string> { userNameOne, userNameTwo};
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
         * Button that uploads file to chat
         */

        private void UploadFile_Click(object sender, RoutedEventArgs e) 
        { 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //string path = appFolder + @"\MyAppLibrary\";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);

            //    // Add existing files to that folder
            //    var rm = Properties.Resources.ResourceManager;
            //    var resSet = rm.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            //    foreach (var res in resSet)
            //    {
            //        var entry = ((DictionaryEntry)res);
            //        var name = (string)entry.Key;
            //        var file = (byte[])rm.GetObject(name);

            //        var filePath = path + name + ".dxf";
            //        File.WriteAllBytes(filePath, file);
            //    }
            //}

            //// Load all files from the library folder
            //string[] filePaths = Directory.GetFiles(path, "*.dxf");
            //Nullable bool to check if file was selected
            int? i = null;

            bool? response = dlg.ShowDialog();

            if (response == true) {
                string filepath = dlg.FileName;
                ChatMessages.Add($"File uploaded: {filepath}");
                //FileInfo fileInfo = new FileInfo(filepath);
                ChatMessages.Add(filepath);
                //{ 
                //    FileName = filename,
                //    FileSize = string.Format("{0} {1}", fileInfo.Length/1.049e+6)
                //})
            }
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
            var messages = _client.GetRoomMessages(_lobbyName, false);
            var users = _client.GetRoomUsers(_lobbyName, false);

            ChatHistoryBox.ItemsSource = messages;
            UserListBox.ItemsSource = users;
        }
    }
}