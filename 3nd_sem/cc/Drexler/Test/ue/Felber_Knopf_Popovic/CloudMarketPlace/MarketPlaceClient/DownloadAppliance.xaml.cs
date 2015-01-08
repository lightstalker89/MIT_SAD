using Microsoft.Win32;
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
    /// Interaktionslogik für DownloadAppliance.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        public string Path { get; set; }

        public DownloadWindow()
        {
            InitializeComponent();
        }

        private void btnFileDialog_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save Image File";
            //saveFileDialog1.CheckFileExists = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                txtFileDialog.Text = saveFileDialog1.FileName;
                Path = saveFileDialog1.FileName;
            }

        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (Path != string.Empty && Path != null)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Invalid Input!");
            }
        }
    }
}
