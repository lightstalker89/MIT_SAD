// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// <author>Mario Murrent</author>
// --------------------------------------------------------------------------------------------------------------------

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
        }
    }
}
