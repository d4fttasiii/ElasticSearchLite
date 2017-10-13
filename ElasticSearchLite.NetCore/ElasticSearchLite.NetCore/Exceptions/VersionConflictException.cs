using System;

namespace ElasticSearchLite.NetCore.Exceptions
{
    public class VersionConflictException : Exception
    {
        public VersionConflictException() { }

        public VersionConflictException(string message) : base(message) { }
    }
}
