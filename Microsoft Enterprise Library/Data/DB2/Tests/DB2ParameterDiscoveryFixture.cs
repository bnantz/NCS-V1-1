//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  LONG_RUNNING_TESTS
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests
{
    [TestFixture]
    public class DB2ParameterDiscoveryFixture : ParameterDiscoveryFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new Db2TestConfigurationContext());
            this.db = factory.CreateDatabase("DB2Test");
            storedProcedure = db.GetStoredProcCommandWrapper("CustOrdersOrders", "ALFKI");
            connection = db.GetConnection();
            connection.Open();
            storedProcedure.Command.Connection = connection;
            cache = new ParameterCache();
        }

        [Test]
        public void CanGetParametersForStoredProcedure()
        {
            cache.FillParameters(storedProcedure, '@');
            Assert.AreEqual(2, storedProcedure.Command.Parameters.Count);
            Assert.AreEqual("RETURN", ((IDataParameter)storedProcedure.Command.Parameters["RETURN"]).ParameterName);
            Assert.AreEqual("VCUSTOMERID", ((IDataParameter)storedProcedure.Command.Parameters["vCustomerId"]).ParameterName);
        }

        [Test]
        public void IsCacheUsed()
        {
            TestCache testCache = new TestCache();
            testCache.FillParameters(storedProcedure, '@');

            DBCommandWrapper storedProcDuplicate = db.GetStoredProcCommandWrapper("CustOrdersOrders", "ALFKI");
            storedProcDuplicate.Command.Connection = connection;
            testCache.FillParameters(storedProcDuplicate, '@');

            Assert.IsTrue(testCache.CacheUsed, "Cache is not used");
        }
    }
}

#endif