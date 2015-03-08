// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddUserWindow.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// <author>Mario Murrent</author>
// --------------------------------------------------------------------------------------------------------------------

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
        private DataStore dataStore = null;

        private bool userNameAlreadyExists = true;
        /// <summary>
        /// </summary>
        public AddUserWindow()
        {
            InitializeComponent();
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

        private void UserNameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (dataStore.ContainsUserName(UserName.Text))
            {
                userNameAlreadyExists = true;
                UserName.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                userNameAlreadyExists = false;
                if (Equals(this.UserName.Foreground, new SolidColorBrush(Colors.DarkRed)))
                {
                    UserName.ClearValue(ForegroundProperty);
                }
            }
        }
    }
}
