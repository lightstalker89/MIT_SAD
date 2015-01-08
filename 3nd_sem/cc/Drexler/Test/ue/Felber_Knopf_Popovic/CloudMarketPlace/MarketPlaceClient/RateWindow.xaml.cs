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

namespace MarketPlaceClient
{
    /// <summary>
    /// Interaktionslogik für RateApplianceWindow.xaml
    /// </summary>
    public partial class RateWindow : Window
    {
        public int Rate { get; set; }

        public RateWindow()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Rate = 1;
            DialogResult = true;
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            Rate = 2;
            DialogResult = true;
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            Rate = 3;
            DialogResult = true;
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            Rate = 4;
            DialogResult = true;
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            Rate = 5;
            DialogResult = true;
        }
    }
}
