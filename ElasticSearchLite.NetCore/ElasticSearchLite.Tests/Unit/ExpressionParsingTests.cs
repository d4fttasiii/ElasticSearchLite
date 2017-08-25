using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class ExpressionParsingTests
    {
        [TestMethod]
        public void ParseNestedObjectArrays()
        {
            Search.In("asd")
                .Return<ComplexPoco>()
                .Match(c => c.Pocos.Select(p => p.TestDateTime), "")
                .ByUsingOperator(NetCore.Models.Enums.ElasticOperators.And);
        }
    }
}
