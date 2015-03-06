namespace PassSecure.Models
{
    using System.Collections.Generic;
    using System.Windows.Documents;

    public class UserTraining
    {
        public string UserName { get; set; }

        public List<TrainingEntry> Trainings { get; set; };
    }
}
