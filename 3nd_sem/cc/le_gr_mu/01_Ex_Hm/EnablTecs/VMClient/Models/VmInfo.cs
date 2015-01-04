using System;

namespace VirtualMachineClient.Models
{
    using System.Collections.Generic;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using GalaSoft.MvvmLight;

    public class VmInfo : ViewModelBase
    {
        public string Id { get; set; }

        public string ReferencedVirtualMachineId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string ApplicationType { get; set; }

        public string OperatingSystem { get; set; }

        private string operatingSystemType;
        public string OperatingSystemType
        {
            get
            {
                return operatingSystemType;
            }
            set
            {
                operatingSystemType = value;
                UpdateVmImage();
            }
        }

        public string OperatingSystemVersion { get; set; }

        public string Size { get; set; }

        public string RecommendedCPU { get; set; }

        public string RecommendedRAM { get; set; }

        public string SupportedVirtualizationPlatform { get; set; }

        public List<string> Software { get; set; }

        public List<string> SupportedProgramingLanguages { get; set; }

        public string Status { get; set; }

        public VmRating[] Ratings { get; set; }

        private string vmImagePath;
        public string VmImagePath
        {
            get
            {
                return vmImagePath;
            }
            set
            {
                vmImagePath = value;
                RaisePropertyChanged("VmImagePath");
            }
        }

        public void UpdateVmImage()
        {
            if (operatingSystemType.Contains("Linux"))
            {
                VmImagePath = "Assets/linux.png";
            }
            else if (operatingSystemType.Contains("Windows"))
            {
                VmImagePath = "Assets/windows.png";
            }
            else if (operatingSystemType.Contains("Mac"))
            {
                VmImagePath = "Assets/apple.png";
            }
        }
    }
}
