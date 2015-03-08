// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrainingEntry.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region File Header
// <copyright file="TrainingEntry.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Models
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// </summary>
    public class TrainingEntry
    {
        /// <summary>
        /// </summary>
        public int TrainingId { get; set; }

        /// <summary>
        /// </summary>
        public int Errors { get; set; }

        /// <summary>
        /// </summary>
        public List<KeyStroke> KeyStrokes { get; set; }

        /// <summary>
        /// </summary>
        public void Analyze()
        {
            
        }
    }
}
