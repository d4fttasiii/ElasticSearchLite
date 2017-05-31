using System;

namespace ElasticSearchLite.NetCore.Exceptions
{
    public class ConnectionNotAvailableException : Exception
    {
        public ConnectionNotAvailableException() { }

        public ConnectionNotAvailableException(string message) : base(message) { }
    }
}
