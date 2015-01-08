
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
    /// Interaction logic for AddMachine.xaml
    /// </summary>
    public partial class AddMachine : Window
    {
        public CommonModel.VirtualMachine Machine { get; set; }
        public string Path { get; set; }

        public AddMachine()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Machine = new CommonModel.VirtualMachine();
            Machine.OperatingSystemType = txtOSType.Text;
            Machine.OperatingSystemName = txtOSName.Text;
            Machine.OperatingSystemVersion = txtOSVersion.Text;
            Machine.SupportedVirtualizationPlatforms = txtSupportVirt.Text;

            //int appType;
            //int.TryParse(txtAppType.Text, out appType);
            //Machine.ApplicationType = appType;

            //int instSW;
            //int.TryParse(txtInstalledSW.Text, out instSW);
            //Machine.InstalledSoftware = instSW;

            //int suppPL;
            //int.TryParse(txtProgLang.Text, out suppPL);
            //Machine.SupportedProgrammingLanguages = suppPL;

            int size;
            int.TryParse(txtSize.Text, out size);
            Machine.Size = size;

            int cpu;
            int.TryParse(txtRecommendedCpu.Text, out cpu);
            Machine.RecommendedCpu = cpu;

            int memory;
            int.TryParse(txtRecommendedMemory.Text, out memory);
            Machine.RecommendedMemory = memory;

            if (Machine.Size > 0 && Machine.RecommendedCpu > 0 && Machine.RecommendedMemory > 0  
                 && Machine.OperatingSystemType != string.Empty && Machine.OperatingSystemName != string.Empty
                && Machine.OperatingSystemVersion != string.Empty && Machine.SupportedVirtualizationPlatforms != string.Empty && Path != string.Empty && Path != null)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void btnFileDialog_Click(object sender, RoutedEventArgs e)
        {
            Path = string.Empty;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == true)
            {
                Path = openFileDialog1.FileName;
                txtFileDialog.Text = Path;
            }
        }
    }
}
