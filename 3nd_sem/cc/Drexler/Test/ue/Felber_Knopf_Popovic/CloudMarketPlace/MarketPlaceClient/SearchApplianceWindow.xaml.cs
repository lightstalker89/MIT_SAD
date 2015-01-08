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
    /// Interaktionslogik für SearchApplianceWindow.xaml
    /// </summary>
    public partial class SearchApplianceWindow : Window
    {
        public CommonModel.VirtualAppliance Appliance { get; set; }

        public SearchApplianceWindow()
        {
            InitializeComponent();

            Appliance = new CommonModel.VirtualAppliance();

        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Appliance.OperatingSystemType = txtOSType.Text;
            Appliance.OperatingSystemName = txtOSName.Text;
            Appliance.OperatingSystemVersion = txtOSVersion.Text;
            Appliance.SupportedVirtualizationPlatforms = txtSupportVirt.Text;

            int appType;
            int.TryParse(txtAppType.Text, out appType);
            Appliance.ApplicationType = appType;

            int instSW;
            int.TryParse(txtInstalledSW.Text, out instSW);
            Appliance.InstalledSoftware = instSW;

            int suppPL;
            int.TryParse(txtProgLang.Text, out suppPL);
            Appliance.SupportedProgrammingLanguages = suppPL;

            int size;
            int.TryParse(txtSize.Text, out size);
            Appliance.Size = size;

            int cpu;
            int.TryParse(txtRecommendedCpu.Text, out cpu);
            Appliance.RecommendedCpu = cpu;

            int memory;
            int.TryParse(txtRecommendedMemory.Text, out memory);
            Appliance.RecommendedMemory = memory;


            DialogResult = true;

        }
    }
}
