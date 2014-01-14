using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPServer
{
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        public MessageEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
    }
}
