namespace PassSecure.Data
{
    using System.Collections.Generic;

    using PassSecure.Models;
    using PassSecure.Service;

    public class DataStore
    {
        private readonly DataReader dataReader = null;
        public DataStore()
        {
            dataReader = SimpleContainer.Resolve<DataReader>();
            ReadLocalData();
        }

        /// <summary>
        /// </summary>
        private List<UserTraining> userTrainings = new List<UserTraining>();

        public void AddUserTraining(UserTraining userTraining)
        {
            this.userTrainings.Add(userTraining);
        }

        public void UpdateUserTraining(UserTraining userTraining)
        {
            
        }

        public IEnumerable<UserTraining> GetUserTrainings()
        {
            return this.userTrainings;
        }

        public void ReadLocalData()
        {
            List<UserTraining> localTrainings = dataReader.Read();
            if (localTrainings != null)
            {
                userTrainings = localTrainings;
            }
        }
    }
}
