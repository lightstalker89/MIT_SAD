// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserTraining.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// <author>Mario Murrent</author>
// --------------------------------------------------------------------------------------------------------------------

namespace PassSecure.Models
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// </summary>
    public class UserTraining
    {
        /// <summary>
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// </summary>
        public List<TrainingEntry> Trainings { get; set; }
    }
}
