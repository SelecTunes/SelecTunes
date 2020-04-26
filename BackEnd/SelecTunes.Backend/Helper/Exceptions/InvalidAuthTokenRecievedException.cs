using System;

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
