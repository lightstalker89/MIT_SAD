#region File Header
// <copyright file="DataStore.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Data
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
            //ReadLocalData();
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
        public void UpdateUserTraining()
        {
            Save();
        }

        /// <summary>
        /// </summary>
        /// <param name="username">
        /// </param>
        /// <returns>
        /// </returns>
        public bool ContainsUserName(string username)
        {
            return userTrainings.Any(p => p.UserName.ToLower().Trim() == username.ToLower().Trim());
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
        /// <param name="username">
        /// </param>
        /// <returns>
        /// </returns>
        public UserTraining GetUserTraining(string username)
        {
            return this.userTrainings.SingleOrDefault(p => p.UserName.Equals(username));
        }

        /// <summary>
        /// </summary>
        public void ReadLocalData()
        {
            List<UserTraining> localTrainings = this.dataManager.Read();
            if (localTrainings != null)
            {
                userTrainings = localTrainings;
                Parallel.ForEach(localTrainings, p => p.Analyze());
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
