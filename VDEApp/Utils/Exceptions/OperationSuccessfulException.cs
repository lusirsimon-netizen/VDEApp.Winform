using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Utils.Exceptions
{
    public class OperationSuccessfulException : Exception
    {
        public OperationSuccessfulException() { }
        public OperationSuccessfulException(string message) : base(message) { }
        public OperationSuccessfulException(string message, Exception inner) : base(message, inner) { }
        protected OperationSuccessfulException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}