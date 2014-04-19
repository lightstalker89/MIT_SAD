using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp1a
{
    [Serializable]
    public class StackEmptyException : Exception
    {
        public StackEmptyException() { }
        public StackEmptyException(string message) : base(message) { }
        public StackEmptyException(string message, Exception inner) : base(message, inner) { }
        protected StackEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
