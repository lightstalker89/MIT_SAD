using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp1b
{
    [Serializable]
    public class QueueEmptyException : Exception
    {
        public QueueEmptyException() { }
        public QueueEmptyException(string message) : base(message) { }
        public QueueEmptyException(string message, Exception inner) : base(message, inner) { }
        protected QueueEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
