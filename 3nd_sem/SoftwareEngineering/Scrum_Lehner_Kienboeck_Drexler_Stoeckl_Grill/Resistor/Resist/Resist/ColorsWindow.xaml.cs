using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Resist
{
    /// <summary>
    /// Interaktionslogik für ColorsWindow.xaml
    /// </summary>
    public partial class ColorsWindow : Window
    {
        RelayCommand selector = null;
        public int ReturnValue { get; set; }

        public RelayCommand Selector
        {
            get
            {
                return selector ?? (selector = new RelayCommand((x) =>
                {
                    this.ReturnValue = int.Parse((string)x); 
                    this.Close();
                }));
            }
        }
        public ColorsWindow()
        {
            InitializeComponent();
        }
    }
}
