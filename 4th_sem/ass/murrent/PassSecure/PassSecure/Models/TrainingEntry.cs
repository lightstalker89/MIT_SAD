namespace PassSecure.Models
{
    using System.Collections.Generic;

    public class TrainingEntry
    {
        public int TrainingId { get; set; }

        public int Errors { get; set; }

        public List<KeyStroke> KeyStrokes { get; set; }

        public void Analyze()
        {
            
        }
    }
}
