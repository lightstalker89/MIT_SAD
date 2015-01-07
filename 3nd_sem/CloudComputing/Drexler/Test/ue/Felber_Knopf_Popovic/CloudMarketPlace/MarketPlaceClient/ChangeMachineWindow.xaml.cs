using CommonModel;
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
    /// Interaktionslogik für ChangeMachineWindow.xaml
    /// </summary>
    public partial class ChangeMachineWindow : Window
    {
        public VirtualMachine Machine { get; set; }

        public ChangeMachineWindow(VirtualMachine machine)
        {
            InitializeComponent();

            Machine = new VirtualMachine();

            //txtAppType.Text = machine.ApplicationType.ToString();
            //txtInstalledSW.Text = machine.InstalledSoftware.ToString();
            txtOSName.Text = machine.OperatingSystemName;
            txtOSType.Text = machine.OperatingSystemType;
            txtOSVersion.Text = machine.OperatingSystemVersion;
            //txtProgLang.Text = machine.SupportedProgrammingLanguages.ToString();
            txtRecommendedCpu.Text = machine.RecommendedCpu.ToString();
            txtRecommendedMemory.Text = machine.RecommendedMemory.ToString();
            txtSize.Text = machine.Size.ToString();
            txtSupportVirt.Text = machine.SupportedVirtualizationPlatforms;
            Machine.ImageId = machine.ImageId;
            Machine.Comment = machine.Comment;
            Machine.Rate = machine.Rate;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
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
                && Machine.OperatingSystemVersion != string.Empty && Machine.SupportedVirtualizationPlatforms != string.Empty)
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
