#region File Header
// <copyright file="MainWindow.xaml.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Views
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    using PassSecure.Data;
    using PassSecure.Events;
    using PassSecure.Models;
    using PassSecure.Service;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// </summary>
        private static MainWindow Instance { get; set; }

        /// <summary>
        /// </summary>
        private static readonly List<KeyStroke> KeyStrokes = new List<KeyStroke>();

        /// <summary>
        /// </summary>
        private readonly DataStore dataStore;

        /// <summary>
        /// </summary>
        private readonly KeyLogger keyLogger;

        /// <summary>
        /// </summary>
        private UserTraining currentUserTraining;

        /// <summary>
        /// </summary>
        private readonly PasswordAnalyzer passwordAnalyzer;

        /// <summary>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Bootstrapper bootstrapper = new Bootstrapper();
            passwordAnalyzer = SimpleContainer.Resolve<PasswordAnalyzer>();
            dataStore = SimpleContainer.Resolve<DataStore>();
            keyLogger = SimpleContainer.Resolve<KeyLogger>();
            keyLogger.SetMainWindow(this);
            keyLogger.KeyLogPerformed += this.KeyLoggerKeyLogPerformed;
            keyLogger.EnterPressed += this.KeyLoggerEnterPressed;
            UpdateData();
            this.Closing += this.MainWindowClosing;
            this.Password.Focus();
            Instance = this;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void KeyLoggerEnterPressed(object sender, EventArgs e)
        {
            Instance.AddOrCheck();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void KeyLoggerKeyLogPerformed(object sender, KeyLogEventArgs e)
        {
            KeyStrokes.Add(e.KeyStroke);
            Status.Text = string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void MainWindowClosing(object sender, CancelEventArgs e)
        {
            keyLogger.UnHook();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            Instance.AddOrCheck();
            Password.Focus();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void MenuItemModeTrainChecked(object sender, RoutedEventArgs e)
        {
            MenuItemModeNormal.IsChecked = !MenuItemModeTrain.IsChecked;
            CheckMode();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void MenuItemModeNormalChecked(object sender, RoutedEventArgs e)
        {
            MenuItemModeTrain.IsChecked = !MenuItemModeNormal.IsChecked;
            CheckMode();
        }

        /// <summary>
        /// </summary>
        private void CheckMode()
        {
            if (ModeText != null)
            {
                ModeText.Text = MenuItemModeNormal.IsChecked
                                    ? MenuItemModeNormal.Header.ToString()
                                    : MenuItemModeTrain.Header.ToString();
            }

            if (Status != null)
            {
                Status.Text = string.Empty;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void AddUserButtonClick(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindowWindow = new AddUserWindow();
            addUserWindowWindow.ShowDialog();
            if (addUserWindowWindow.DialogResult.HasValue && addUserWindowWindow.DialogResult.Value)
            {
                dataStore.AddUserTraining(
                    new UserTraining()
                        {
                            UserName = addUserWindowWindow.UserName.Text, 
                            Password = addUserWindowWindow.Password.Text
                        });
                UpdateData();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void OnStatisticsClick(object sender, RoutedEventArgs e)
        {
            StatisticsWindow statisticsWindow = new StatisticsWindow();
            statisticsWindow.ShowDialog();
        }

        /// <summary>
        /// </summary>
        private void UpdateData()
        {
            UserNames.Items.Clear();
            IEnumerable<UserTraining> trainings = dataStore.GetUserTrainings();
            foreach (UserTraining userTraining in trainings)
            {
                UserNames.Items.Add(userTraining.UserName);
            }

            UserNames.SelectedIndex = 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void DataClick(object sender, RoutedEventArgs e)
        {
            DataWindow dataWindow = new DataWindow();
            dataWindow.ShowDialog();
        }

        /// <summary>
        /// </summary>
        public void AddOrCheck()
        {
            bool allowedToAdd = currentUserTraining != null
                                  && Password.Password.Equals(currentUserTraining.Password);
            if (MenuItemModeNormal.IsChecked && currentUserTraining != null)
            {
                PasswordEntry passwordEntry = new PasswordEntry()
                                                  {
                                                      KeyStrokes =  KeyStrokes
                                                  };
                passwordEntry.Analyze();
                bool accepted = passwordAnalyzer.IsAccepted(UserNames.SelectedItem.ToString(), passwordEntry);
                if (accepted)
                {
                    currentUserTraining.Trainings.Add(new TrainingEntry() { KeyStrokes = KeyStrokes });
                    ShowSuccessWindow();
                }
            }
            else
            {
                if (allowedToAdd)
                {
                    currentUserTraining.Trainings.Add(new TrainingEntry() { KeyStrokes = KeyStrokes });
                    currentUserTraining.Analyze();
                    dataStore.UpdateUserTraining();
                    Status.Text = "Entry added successfully";
                }
                else
                {
                    Status.Text = "Wrong password provided. Entry is not added";
                }
            }

            ClearInput();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void UserNamesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserNames.SelectedItem != null)
            {
                string username = UserNames.SelectedItem.ToString();
                currentUserTraining = dataStore.GetUserTraining(username);
            }
        }

        private void ShowSuccessWindow()
        {
            SuccessWindow successWindow = new SuccessWindow();
            successWindow.ShowDialog();
        }

        /// <summary>
        /// </summary>
        private void ClearInput()
        {
            Password.Clear();
            KeyStrokes.Clear();
        }
    }
}
