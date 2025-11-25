using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Utils.Exceptions
{
    public class OperationFailureException : Exception
    {
        public OperationFailureException() : base() { }
        public OperationFailureException(string message) : base(message) { }
        public OperationFailureException(string message, Exception innerException) : base(message, innerException) { }
    }
}
