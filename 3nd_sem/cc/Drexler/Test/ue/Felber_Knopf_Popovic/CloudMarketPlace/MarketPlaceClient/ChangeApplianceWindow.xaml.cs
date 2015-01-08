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
    /// Interaktionslogik für ChangeWindow.xaml
    /// </summary>
    public partial class ChangeApplianceWindow : Window
    {
        public CommonModel.VirtualAppliance Appliance { get; set; }

        public ChangeApplianceWindow(VirtualAppliance appliance)
        {
            InitializeComponent();

            Appliance = new CommonModel.VirtualAppliance();

            txtAppType.Text = appliance.ApplicationType.ToString();
            txtInstalledSW.Text = appliance.InstalledSoftware.ToString();
            txtOSName.Text = appliance.OperatingSystemName;
            txtOSType.Text = appliance.OperatingSystemType;
            txtOSVersion.Text = appliance.OperatingSystemVersion;
            txtProgLang.Text = appliance.SupportedProgrammingLanguages.ToString();
            txtRecommendedCpu.Text = appliance.RecommendedCpu.ToString();
            txtRecommendedMemory.Text = appliance.RecommendedMemory.ToString();
            txtSize.Text = appliance.Size.ToString();
            txtSupportVirt.Text = appliance.SupportedVirtualizationPlatforms;
            Appliance.ImageId = appliance.ImageId;
            Appliance.Comment = appliance.Comment;
            Appliance.Rate = appliance.Rate;
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

            if (Appliance.Size > 0 && Appliance.RecommendedCpu > 0 && Appliance.RecommendedMemory > 0 && Appliance.ApplicationType > 0 && Appliance.InstalledSoftware > 0
                && Appliance.SupportedProgrammingLanguages > 0 && Appliance.OperatingSystemType != string.Empty && Appliance.OperatingSystemName != string.Empty
                && Appliance.OperatingSystemVersion != string.Empty && Appliance.SupportedVirtualizationPlatforms != string.Empty)
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
