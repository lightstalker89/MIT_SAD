#region File Header
// <copyright file="DataWindow.xaml.cs" company="">
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

    using PassSecure.Data;
    using PassSecure.Service;

    #endregion

    /// <summary>
    /// Interaction logic for DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        /// <summary>
        /// </summary>
        public DataWindow()
        {
            InitializeComponent();
            DataStore dataStore = SimpleContainer.Resolve<DataStore>();
            //DataListView.ItemsSource = dataStore.GetUserTrainings();
            DataTreeView.ItemsSource = dataStore.GetUserTrainings();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
