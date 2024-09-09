using DataServer;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

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
         */
        
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = LoginBox.Text;
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a valid username.");
            }
            else
            {
                try
                {
                    if (await Task.Run(() => _client.AlreadyLoggedIn(username)))
                    {
                        MessageBox.Show($"User '{username}' logged in elsewhere.");
                    }
                    else if (await Task.Run(() => _client.AlreadyExists(username)))
                    {
                        if (await Task.Run(() => _client.Login(username)))
                        {
                            NavigationService.Navigate(new MainLobbyPage(_client, username));
                        }
                        else
                        {
                            MessageBox.Show("Login failed. Please try again.");
                        }
                    }
                    else
                    {
                        await Task.Run(() => _client.CreateUser(username));
                        NavigationService.Navigate(new MainLobbyPage(_client, username));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        /*
         * Simple keydown handler so on Enter key press it does the login button click
         */
        private void LoginBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}
