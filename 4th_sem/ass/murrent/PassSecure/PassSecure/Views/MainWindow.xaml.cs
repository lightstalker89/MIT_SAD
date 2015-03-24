#region File Header
// <copyright file="MainWindow.xaml.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion

using System.Diagnostics;

namespace PassSecure.Views
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;

    using PassSecure.Data;
    using PassSecure.Events;
    using PassSecure.Models;
    using PassSecure.Service;

    using Application = System.Windows.Application;
    using MessageBox = System.Windows.Forms.MessageBox;
    using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

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
            dataStore.ReadLocalData();
            UpdateData();
            this.Closing += this.MainWindowClosing;
            this.Password.Focus();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
            NotAccepted.Visibility = Visibility.Collapsed;
            Accepted.Visibility = Visibility.Collapsed;
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
            MenuItemModeNormal.IsChecked = false;
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
            MenuItemModeTrain.IsChecked = false;
            CheckMode();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemModeNormalUnchecked(object sender, RoutedEventArgs e)
        {
            MenuItemModeTrain.IsChecked = true;
            CheckMode();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemModeTrainUnchecked(object sender, RoutedEventArgs e)
        {
            MenuItemModeNormal.IsChecked = true;
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
            bool isPasswordEqual = Password.Password.Equals(currentUserTraining.Password);
            bool allowedToAdd = currentUserTraining != null && isPasswordEqual;
            if (MenuItemModeNormal.IsChecked && currentUserTraining != null && isPasswordEqual)
            {
                UserTraining passwordEntry = new UserTraining()
                                                  {
                                                      Password = currentUserTraining.Password,
                                                      UserName = currentUserTraining.UserName,
                                                      Trainings = new List<TrainingEntry>()
                                                                      {
                                                                          new TrainingEntry(){ KeyStrokes = KeyStrokes}
                                                                      }
                                                  };
                passwordEntry.Analyze();
                Enums.PasswordStatus status = passwordAnalyzer.IsAccepted(UserNames.SelectedItem.ToString(), passwordEntry);
                if (status == Enums.PasswordStatus.Accepted)
                {
                    //TrainingEntry trainingEntry = new TrainingEntry() { KeyStrokes = KeyStrokes };
                    //trainingEntry.Analyze();
                    //trainingEntry.Distance = passwordAnalyzer.CalculateDistance(currentUserTraining, trainingEntry);
                    //currentUserTraining.Trainings.Add(trainingEntry);
                    //currentUserTraining.AcceptedUserAttempt = true;
                    //dataStore.UpdateUserTraining();
                    Accepted.Visibility = Visibility.Visible;
                }
                else if (status == Enums.PasswordStatus.PartialAccepted)
                {

                }
                else if (status == Enums.PasswordStatus.NotAccepted)
                {
                    currentUserTraining.AcceptedUserAttempt = false;
                    Accepted.Visibility = Visibility.Collapsed;
                    NotAccepted.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (allowedToAdd)
                {
                    TrainingEntry trainingEntry = new TrainingEntry() { KeyStrokes = KeyStrokes };
                    trainingEntry.Analyze();
                    trainingEntry.Distance = passwordAnalyzer.CalculateDistance(currentUserTraining, trainingEntry);
                    Debug.WriteLine("TRAINING DISTANCE: " + trainingEntry.Distance);
                    currentUserTraining.Trainings.Add(trainingEntry);
                    currentUserTraining.AcceptedUserAttempt = false;
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

        /// <summary>
        /// </summary>
        private void ClearInput()
        {
            Password.Clear();
            KeyStrokes.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ImportDataClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "";
            openFileDialog.DefaultExt = ".pss";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                await Task.Factory.StartNew(
                    () => File.Copy(openFileDialog.FileName, "data.pss", true));
                dataStore.ReadLocalData();
                UpdateData();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportDataClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.DefaultExt = ".pss";
            saveFileDialog.Filter = "PassSecure File (.pss)|*.pss";
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists("data.pss"))
                {
                    File.Copy("data.pss", saveFileDialog.FileName);
                }
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
