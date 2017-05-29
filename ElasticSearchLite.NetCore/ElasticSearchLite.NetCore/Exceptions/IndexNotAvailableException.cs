using System;

namespace ElasticSearchLite.NetCore.Exceptions
{
    public class IndexNotAvailableException : Exception
    {
        public IndexNotAvailableException() { }

        public IndexNotAvailableException(string message) : base(message) { }
    }
}
