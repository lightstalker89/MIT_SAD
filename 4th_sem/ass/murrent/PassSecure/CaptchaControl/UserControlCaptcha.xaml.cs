using System.Windows.Controls;

namespace CaptchaControl
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for UserControlCaptcha.xaml
    /// </summary>
    public partial class UserControlCaptcha : UserControl
    {
        public UserControlCaptcha()
        {
            InitializeComponent();
        }

        private event EventHandler<EventArgs> Success; 

        private const string CaptchaKey = "CaptchaControl";

        private static readonly char[] CharArray =
        "ABCEFGHJKLMNPRSTUVWXYZ2346789".ToCharArray();

        public void Generate()
        {
            char[] captcha = new char[8];

            Random random = new Random();

            for (int x = 0; x < captcha.Length; x++)
            {
                captcha[x] = CharArray[random.Next(CharArray.Length)];
            }

            CaptchaTextBlock.Text = new string(captcha);
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        private bool IsHuman()
        {
            return EnteredCaptcha.Text.Trim().Equals(CaptchaTextBlock.Text);
        }

        private void SubmitButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsHuman())
            {
                OnSuccess();
            }
            else
            {
                Generate();
            }
        }

        private void OnSuccess()
        {
            if (Success != null)
            {
                Success(this, new EventArgs());
            }
        }
    }
}
