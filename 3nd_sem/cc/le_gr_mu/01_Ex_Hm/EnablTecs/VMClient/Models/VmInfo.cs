namespace VirtualMachineClient.Models
{
    using System;
    using System.Collections.Generic;

    using GalaSoft.MvvmLight;

    public class VmInfo : ViewModelBase
    {
        private string originalDescription = String.Empty;
        private string origialRating = String.Empty;
        private string originalRatingDescription = String.Empty;

        private bool descriptionChanged = false;
        public bool DescriptionChanged
        {
            get
            {
                return descriptionChanged;
            }
            set
            {
                descriptionChanged = value;
                RaisePropertyChanged("DescriptionChanged");
            }
        }

        private bool ratingChanged = false;
        public bool RatingChanged
        {
            get
            {
                return ratingChanged;
            }
            set
            {
                ratingChanged = value;
                RaisePropertyChanged("RatingChanged");
            }
        }

        private bool ratingDescriptionChanged = false;
        public bool RatingDescriptionChanged
        {
            get
            {
                return ratingDescriptionChanged; 
                
            }
            set
            {
                ratingDescriptionChanged = value;
                RaisePropertyChanged("RatingDescriptionChanged");
            }
        }

        private bool ratingAndDescriptionChanged = false;
        public bool RatingAndDescriptionChanged
        {
            get
            {
                return ratingAndDescriptionChanged;
                
            }
            set
            {
                ratingAndDescriptionChanged = value;
                RaisePropertyChanged("RatingAndDescriptionChanged");
            }
        }

        public string Id { get; set; }

        public string ReferencedVirtualMachineId { get; set; }

        public string Name { get; set; }

        private string description;

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                if (originalDescription == String.Empty)
                {
                    originalDescription = value;
                }
                DescriptionChanged = value != originalDescription;
                RaisePropertyChanged("Description");
            }
        }

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
                if (origialRating == String.Empty)
                {
                    origialRating = Rating;
                }
                RatingChanged = value != origialRating;
                RatingAndDescriptionChanged = RatingDescriptionChanged || RatingChanged;
                RaisePropertyChanged("Rating");
            }
        }

        private string ratingDescription;

        public string RatingDescription
        {
            get
            {
                return ratingDescription;
            }
            set
            {
                if (originalRatingDescription == String.Empty)
                {
                    originalRatingDescription = value;
                }
                RatingDescriptionChanged = value != originalRatingDescription;
                RatingAndDescriptionChanged = RatingDescriptionChanged || RatingChanged;
                ratingDescription = value;
            }
        }

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
            Rating = (ratingIndex).ToString();
        }
    }
}
