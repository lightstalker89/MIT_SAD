#region File Header
// <copyright file="TrainingEntry.cs" company="">
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
    using System.Windows.Input;

    #endregion

    /// <summary>
    /// Interaction logic for SuccessWindow.xaml
    /// </summary>
    public partial class SuccessWindow : Window
    {
        public SuccessWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CloseButton.Focus();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CloseKeyDown(object sender, KeyEventArgs e)
        {
           
        }
    }
}
