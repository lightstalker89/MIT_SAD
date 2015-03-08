// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatisticsWindow.xaml.cs" company="">
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

    using PassSecure.Data;
    using PassSecure.Service;

    #endregion

    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        /// <summary>
        /// </summary>
        private readonly DataStore dataStore = SimpleContainer.Resolve<DataStore>();

        /// <summary>
        /// </summary>
        public StatisticsWindow()
        {
            InitializeComponent();
        }
    }
}
