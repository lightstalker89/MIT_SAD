// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataStore.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// <author>Mario Murrent</author>
// --------------------------------------------------------------------------------------------------------------------

namespace PassSecure.Data
{
    #region Usings

    using System.Collections.Generic;

    using PassSecure.Models;
    using PassSecure.Service;

    #endregion

    /// <summary>
    /// </summary>
    public class DataStore
    {
        /// <summary>
        /// </summary>
        private readonly DataManager dataManager = null;

        /// <summary>
        /// </summary>
        public DataStore()
        {
            this.dataManager = SimpleContainer.Resolve<DataManager>();
            ReadLocalData();
        }

        /// <summary>
        /// </summary>
        private List<UserTraining> userTrainings = new List<UserTraining>();

        /// <summary>
        /// </summary>
        /// <param name="userTraining">
        /// </param>
        public void AddUserTraining(UserTraining userTraining)
        {
            this.userTrainings.Add(userTraining);
            Save();
        }

        /// <summary>
        /// </summary>
        /// <param name="userTraining">
        /// </param>
        public void UpdateUserTraining(UserTraining userTraining)
        {
            
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<UserTraining> GetUserTrainings()
        {
            return this.userTrainings;
        }

        /// <summary>
        /// </summary>
        public void ReadLocalData()
        {
            List<UserTraining> localTrainings = this.dataManager.Read();
            if (localTrainings != null)
            {
                userTrainings = localTrainings;
            }
        }

        /// <summary>
        /// </summary>
        public void Save()
        {
            this.dataManager.Save(userTrainings);
        }
    }
}
