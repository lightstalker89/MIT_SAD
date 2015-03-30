#region File Header
// <copyright file="AddUserWindow.xaml.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Views
{
    #region Usings

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using PassSecure.Data;
    using PassSecure.Service;

    #endregion

    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        /// <summary>
        /// </summary>
        private DataStore dataStore = null;

        /// <summary>
        /// </summary>
        private bool userNameAlreadyExists = true;

        /// <summary>
        /// </summary>
        public AddUserWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            UserName.Focus();
            dataStore = SimpleContainer.Resolve<DataStore>();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (UserName.Text.Trim().Length > 0 && Password.Text.Trim().Length > 0 && !userNameAlreadyExists)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show(
                    "Please specify a username and a password", 
                    "Username and Password", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void UserNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (dataStore.ContainsUserName(UserName.Text))
            {
                userNameAlreadyExists = true;
                UserName.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                userNameAlreadyExists = false;
                if (this.UserName.Foreground.ToString() == new SolidColorBrush(Colors.Red).ToString())
                {
                    UserName.ClearValue(ForegroundProperty);
                }
            }
        }
    }
}
