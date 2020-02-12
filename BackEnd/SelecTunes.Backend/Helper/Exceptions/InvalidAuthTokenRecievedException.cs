using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Helper.Exceptions
{
    public class InvalidAuthTokenRecievedException : Exception
    {
        public InvalidAuthTokenRecievedException(string message) : base(message)
        {
        }

        public InvalidAuthTokenRecievedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidAuthTokenRecievedException()
        {
        }
    }
}
