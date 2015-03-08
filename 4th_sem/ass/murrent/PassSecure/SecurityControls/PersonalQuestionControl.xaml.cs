#region File Header
// <copyright file="PersonalQuestionControl.xaml.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace SecurityControls
{
    #region Usings

    using System.Collections.Generic;
    using System.Windows.Controls;

    #endregion

    /// <summary>
    /// Interaction logic for PersonalQuestionControl.xaml
    /// </summary>
    public partial class PersonalQuestionControl : UserControl
    {
        /// <summary>
        /// </summary>
        public PersonalQuestionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        public Dictionary<string, string> PersonalQuestionsAndAnswers { get; set; }

        /// <summary>
        /// </summary>
        public void Show()
        {
            
        }

        /// <summary>
        /// </summary>
        private void CheckAnswer()
        {
            
        }
    }
}
