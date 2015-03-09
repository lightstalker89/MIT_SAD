#region File Header
// <copyright file="DataManager.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Service
{
    #region Usings

    using System.Collections.Generic;
    using System.IO;
    using System.Web.Script.Serialization;

    using PassSecure.Converter;
    using PassSecure.Models;

    #endregion

    /// <summary>
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// </summary>
        private const string FileName = "data.pss";

        /// <summary>
        /// </summary>
        private readonly JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public DataManager()
        {
            javaScriptSerializer.RegisterConverters(new[] { new TimeSpantoJSONConverter() });
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public List<UserTraining> Read()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                return javaScriptSerializer.Deserialize<List<UserTraining>>(json);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="userTrainings">
        /// </param>
        public void Save(List<UserTraining> userTrainings)
        {
            string json = javaScriptSerializer.Serialize(userTrainings);

            File.WriteAllText(FileName, json);
        }
    }
}
