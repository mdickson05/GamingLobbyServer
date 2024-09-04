using Microsoft.Win32;
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
        //Observable collections allows visual updating for both lists
        public ObservableCollection<string> ChatMessages { get; set; }
        public ObservableCollection<string> testUsers { get; set; }

        /*
         * Creates instances of both lists and adds some test strings
         * 
         * DataContext is how "binding" works inside the xml
         * We bind the list to a specific object/thing
         * This will be replaced later when we incorporate classes and instances
         * 
         * Might implement backout button for ChatLobbyPage to return to MainLobbyPage
         *  for lobby reselection
         * 
         */
        public ChatLobbyPage(string lobbyName)
        {
            InitializeComponent();
            DataContext = this; // Review above comment block
            LobbyNameBlock.Text = lobbyName; //Sets lobby name
            ChatMessages = new ObservableCollection<string>(); //Testing chat messages
            testUsers = new ObservableCollection<string>(); //Testing users

            testUsers.Add("Joe");
            testUsers.Add("Bob");
            testUsers.Add("Garry");
            testUsers.Add("Beans");

            ChatMessages.Add("Joe: Hello!");
            ChatMessages.Add("Bob: Hi there!");
            ChatMessages.Add("Garry: Good morning!");
        }

        /*
         * User send message button, takes input from box and saves into collection
         */
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string message = $"{testUsers[0]}: {UserInputBox.Text}";
            ChatMessages.Add(message);
            UserInputBox.Clear();
        }

        /*
         * Copied above function just to test SendMessageButton_Click
         */
        private void PrivateMessageButton_Click(object sender, RoutedEventArgs e)
        {
            Button tempButton = sender as Button;
            string userName = tempButton.Content.ToString();
            PrivateMessagePage privateMessagePage = new PrivateMessagePage(userName);
            NavigationService.Navigate(privateMessagePage);
        }

        /*
         * Simple logout button returns to LoginPage
         */
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            NavigationService.Navigate(loginPage);
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
    }
}