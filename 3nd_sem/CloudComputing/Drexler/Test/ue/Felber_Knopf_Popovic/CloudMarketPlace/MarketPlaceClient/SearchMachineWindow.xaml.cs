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
    /// Interaktionslogik für SearchMachineWindow.xaml
    /// </summary>
    public partial class SearchMachineWindow : Window
    {
        public VirtualMachine Machine { get; set; }

        public SearchMachineWindow()
        {
            InitializeComponent();

            Machine = new VirtualMachine();

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

            DialogResult = true;

        }
    }
}
