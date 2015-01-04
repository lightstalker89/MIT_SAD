namespace VirtualMachineClient.Models
{
    public class VmRating
    {
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
                UpdateRatingImage();
            }
        }

        public string Comment { get; set; }

        public void UpdateRatingImage()
        {
            switch (rating.Trim())
            {
                case "0":
                    break;

                case "1":
                    break;

                case "2":
                    break;

                case "3":
                    break;

                case "4":
                    break;

                case "5":
                    break;
            }
        }
    }
}
