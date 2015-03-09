#region File Header
// <copyright file="Bootstrapper.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure
{
    #region Usings

    using PassSecure.Data;
    using PassSecure.Service;

    #endregion

    /// <summary>
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// </summary>
        public Bootstrapper()
        {
            SimpleContainer.Register(new DataManager());
            SimpleContainer.Register(new DataStore());
            SimpleContainer.Register(new KeyLogger());
        }
    }
}
