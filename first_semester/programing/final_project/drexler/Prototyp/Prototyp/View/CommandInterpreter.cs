//-----------------------------------------------------------------------
// <copyright file="CommandInterpreter.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Command interpreter
    /// </summary>
    public abstract class CommandInterpreter
    {
        /// <summary>
        /// Handle incoming commands
        /// </summary>
        /// <param name="input">Incoming commands</param>
        public abstract void HandleCommand(string input);
    }
}
