namespace PassSecure.Models
{
    using System.Collections.Generic;

    public class UserTraining
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public List<TrainingEntry> Trainings { get; set; }
    }
}
