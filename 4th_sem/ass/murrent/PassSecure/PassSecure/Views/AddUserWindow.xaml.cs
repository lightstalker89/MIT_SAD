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

    #endregion

    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        /// <summary>
        /// </summary>
        public AddUserWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (UserName.Text.Trim().Length > 0 && Password.Text.Trim().Length > 0)
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
    }
}
