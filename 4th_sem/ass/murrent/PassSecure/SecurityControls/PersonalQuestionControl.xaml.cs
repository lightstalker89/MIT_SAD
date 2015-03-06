using System.Windows.Controls;

namespace SecurityControls
{
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for PersonalQuestionControl.xaml
    /// </summary>
    public partial class PersonalQuestionControl
    {
        public PersonalQuestionControl()
        {
            InitializeComponent();
        }

        public Dictionary<string, string> PersonalQuestions { get; set; }

        private void CheckAnswer()
        {
            
        }
    }
}
