using System.Windows;

namespace PassSecure.Views
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
        }

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
