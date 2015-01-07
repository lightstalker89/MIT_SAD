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
    /// Interaktionslogik für CommentWindow.xaml
    /// </summary>
    public partial class CommentWindow : Window
    {
        public string Comment { get; set; }

        public CommentWindow()
        {
            InitializeComponent();
        }

        private void btnComment_Click(object sender, RoutedEventArgs e)
        {
            if (txtComment.Text != string.Empty)
            {
                Comment = txtComment.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Invalid Input!");
            }
        }
    }
}
