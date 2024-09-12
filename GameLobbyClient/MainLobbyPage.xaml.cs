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
    /// Interaction logic for MainLobbyPage.xaml
    /// </summary>
    public partial class MainLobbyPage : Page
    {
        //Stores client username
        private string userName;

        //Passes instance of client
        private IGLSInterface _client;

        public MainLobbyPage(IGLSInterface client, string username)
        {
            InitializeComponent();
            userName = username;
            _client = client;

            UsernameBlock.Text = $"Welcome: {userName}";
            UsernameBlock.Visibility = Visibility.Visible;

            InitializeBackgroundTasks();
        }

        private void InitializeBackgroundTasks()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await UpdateLobbyList();
                    await Task.Delay(5000);
                }
            });
        }

        private async Task UpdateLobbyList()
        {
            try
            {
                var lobbies = await Task.Run(() => _client.GetAvailableLobbies());
                await Dispatcher.InvokeAsync(() =>
                {
                    UpdateLobbyButtons(lobbies);
                });
            }
            catch (Exception ex)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show($"Error updating lobby list: {ex.Message}");
                });
            }
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
            int lobbyCount = _client.GetRoomCount();

            if (lobbyCount >= 5)
            {
                MessageBox.Show("Maximum number of lobbies reached.");
            }
            else if (string.IsNullOrEmpty(lobbyName))
            {
                MessageBox.Show("Please enter a lobby name.");
            }
            else if (_client.GetAvailableLobbies().Contains(lobbyName))
            {
                MessageBox.Show($"Lobby '{lobbyName}' already exists!");
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

                // Clears LobbyNameBox of previous text
                LobbyNameBox.Clear();
                RefreshChatLobby(); // Could remove?
            }
        }

        /*
         * Simple method that allows Enter key press to create lobby
         */
        private void LobbyNameBox_KeyDown(object sender, KeyEventArgs key)
        {
            if (key.Key == Key.Enter)
            {
                CreateLobbyButton_Click(sender, key);
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
            _client.JoinRoom(lobbyName, userName, false);
            ChatLobbyPage lobbyOne = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyOne);
        }

        private void LobbyButtonTwo_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonTwo.Content.ToString();
            _client.JoinRoom(lobbyName, userName, false);
            ChatLobbyPage lobbyTwo = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyTwo);
        }

        private void LobbyButtonThree_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonThree.Content.ToString();
            _client.JoinRoom(lobbyName, userName, false);
            ChatLobbyPage lobbyThree = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyThree);
        }

        private void LobbyButtonFour_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonFour.Content.ToString();
            _client.JoinRoom(lobbyName, userName, false);
            ChatLobbyPage lobbyFour = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyFour);
        }

        private void LobbyButtonFive_Click(object sender, RoutedEventArgs e)
        {
            var lobbyName = LobbyButtonFive.Content.ToString();
            _client.JoinRoom(lobbyName, userName, false);
            ChatLobbyPage lobbyFive = new ChatLobbyPage(lobbyName, _client, userName);
            NavigationService.Navigate(lobbyFive);
        }

        /*
         * Simple refresh button method 
         */
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                await UpdateLobbyList();
            });
        }

        /*
         * Simple refresh method that refreshes LobbyButtons updating them based
         * on server information.
         */
        private void RefreshChatLobby()
        {
            var lobbies = _client.GetAvailableLobbies();
            UpdateLobbyButtons(lobbies);
        }

        private void UpdateLobbyButtons(List<string> lobbies)
        {
            List<Button> lobbyButtons = new List<Button> { LobbyButtonOne, LobbyButtonTwo, LobbyButtonThree, LobbyButtonFour, LobbyButtonFive };

            foreach (var button in lobbyButtons)
            {
                button.Visibility = Visibility.Collapsed;
                button.Content = string.Empty;
            }

            for (int i = 0; i < lobbies.Count && i < lobbyButtons.Count; i++)
            {
                lobbyButtons[i].Content = lobbies[i];
                lobbyButtons[i].Visibility = Visibility.Visible;
            }
        }
    }
}