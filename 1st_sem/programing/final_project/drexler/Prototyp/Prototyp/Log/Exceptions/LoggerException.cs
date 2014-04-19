//-----------------------------------------------------------------------
// <copyright file="LoggerException.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.Log.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Custom logger exception class
    /// </summary>
    [Serializable]
    public class LoggerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerException"/> class
        /// </summary>
        public LoggerException() : base() 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public LoggerException(string message) : base(message)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerException"/> class
        /// </summary>
        /// <param name="format">Format of the exception</param>
        /// <param name="args">Arguments or so</param>
        public LoggerException(string format, params object[] args) : base(string.Format(format, args)) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerException"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public LoggerException(string message, Exception innerException) : base(message, innerException)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerException"/> class
        /// </summary>
        /// <param name="format">Format or so</param>
        /// <param name="innerException">Inner exception</param>
        /// <param name="args">Argument or so</param>
        public LoggerException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerException"/> class
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected LoggerException(SerializationInfo info, StreamingContext context) : base(info, context) 
        { 
        }
    }
}
