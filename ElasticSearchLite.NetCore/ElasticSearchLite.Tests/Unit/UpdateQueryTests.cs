﻿using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Poco;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class UpdateQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void UpdateQuery_ExceptionTest_Poco()
        {
            TestExceptions(typeof(ArgumentNullException), () => Update<MyPoco>.Document(null), "Poco is null");
        }

        [TestMethod]
        public void UpdateQuery_ExceptionTest_PocoIndex()
        {
            InitPoco();
            poco.Index = null;
            TestExceptions(typeof(ArgumentNullException), () => Update<MyPoco>.Document(poco), "Poco index is null");
        }

        [TestMethod]
        public void UpdateQuery_ExceptionTest_PocoType()
        {
            InitPoco();
            poco.Type = null;
            TestExceptions(typeof(ArgumentNullException), () => Update<MyPoco>.Document(poco), "Poco type is null");
        }

        [TestMethod]
        public void UpdateQuery_ExceptionTest_PocoId()
        {
            InitPoco();
            poco.Id = null;
            TestExceptions(typeof(ArgumentNullException), () => Update<MyPoco>.Document(poco), "Poco Id is null");
        }

        [TestMethod]
        public void UpdateQuery_Generation_UpdatePoco()
        {
            // Arrange
            InitPoco();
            var query = Update<MyPoco>.Document(poco);
            var queryObject = new
            {
                doc = new
                {
                    TestInteger = 12345,
                    TestText = "ABCDEFG",
                    TestBool = true,
                    TestDateTime = new DateTime(2017, 9, 10),
                    TestDouble = 1.337
                }
            };

            // Act and Assert
            TestQuery(queryObject, query);
        }
    }
}