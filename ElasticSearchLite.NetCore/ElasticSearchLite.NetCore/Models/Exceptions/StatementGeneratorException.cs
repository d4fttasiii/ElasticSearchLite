using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Models.Exceptions
{
    public class StatementGeneratorException : Exception
    {
        public StatementGeneratorException(string message) : base(message)
        {
        }
    }
}
