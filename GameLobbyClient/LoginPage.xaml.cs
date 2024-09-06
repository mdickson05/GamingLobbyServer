﻿using DataServer;
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
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = LoginBox.Text;
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a valid username.");
            }
            else if (_client.AlreadyLoggedIn(username))
            {
                MessageBox.Show($"User '{username}' logged in elsewhere.");
            }
            else if (_client.AlreadyExists(username))
            {
                if (_client.Login(username))
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
                _client.CreateUser(username);
                NavigationService.Navigate(new MainLobbyPage(_client, username));
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
