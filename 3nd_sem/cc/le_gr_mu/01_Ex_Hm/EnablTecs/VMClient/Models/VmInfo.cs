namespace VirtualMachineClient.Models
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

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

        public string OperatingSystemType
        {
            get;

            set;
        }

        public string OperatingSystemVersion { get; set; }

        public string Size { get; set; }

        public string RecommendedCPU { get; set; }

        public string RecommendedRAM { get; set; }

        public string SupportedVirtualizationPlatform { get; set; }

        public List<string> Software { get; set; }

        public List<string> SupportedProgramingLanguages { get; set; }

        public string Status { get; set; }

        private string rating;
        public string Rating
        {
            get
            {
                return rating;

            }
            set
            {
                rating = value;
                RaisePropertyChanged("Rating");
            }
        }

        public string RatingDescription { get; set; }

        private int ratingIndex;
        public int RatingIndex
        {
            get
            {
                if (Rating == String.Empty)
                {
                    Rating = "0";
                }
                return int.Parse(Rating);
            }
            set
            {
                ratingIndex = value;
                UpdateRating();
            }
        }

        private void UpdateRating()
        {
            Rating = (ratingIndex + 1).ToString();
        }
    }
}
