//-----------------------------------------------------------------------
// <copyright file="INetwork.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.Network
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Network interface
    /// </summary>
    public interface INetwork
    {
        /// <summary>
        /// Send data
        /// </summary>
        void Send();

        /// <summary>
        /// Receive data
        /// </summary>
        void Receive();
    }
}
