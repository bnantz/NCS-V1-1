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

#if  UNIT_TESTS
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestFixture]
    public class OracleParameterDiscoveryFixture : ParameterDiscoveryFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDatabase("OracleTest");
            storedProcedure = db.GetStoredProcCommandWrapper("CustOrdersOrders", "ALFKI", null);
            connection = db.GetConnection();
            connection.Open();
            storedProcedure.Command.Connection = connection;
            cache = new ParameterCache();
        }

        [Test]
        public void CanGetParametersForStoredProcedure()
        {
            cache.FillParameters(storedProcedure, ':');
            Assert.AreEqual(2, storedProcedure.Command.Parameters.Count);
            Assert.AreEqual("CUR_OUT", ((IDataParameter)storedProcedure.Command.Parameters["CUR_OUT"]).ParameterName);
            Assert.AreEqual("VCUSTOMERID", ((IDataParameter)storedProcedure.Command.Parameters["VCUSTOMERID"]).ParameterName);
        }

        [Test]
        public void IsCacheUsed()
        {
            TestCache testCache = new TestCache();
            testCache.FillParameters(storedProcedure, ':');

            DBCommandWrapper storedProcDuplicate = db.GetStoredProcCommandWrapper("CustOrdersOrders", "ALFKI", null);
            storedProcDuplicate.Command.Connection = connection;
            testCache.FillParameters(storedProcDuplicate, ':');

            Assert.IsTrue(testCache.CacheUsed, "Cache is not used");
        }

        [Test]
        public void CanDiscoverFeaturesWhileInsideTransaction()
        {
            using (IDbConnection connection = db.GetConnection())
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                DBCommandWrapper storedProcedure = db.GetStoredProcCommandWrapper("CustOrdersOrders", "ALFKI");
                storedProcedure.Command.Connection = transaction.Connection;
                storedProcedure.Command.Transaction = transaction;

                storedProcedure.DiscoverParameters(':');

                Assert.AreEqual(2, storedProcedure.Command.Parameters.Count);
            }
        }
    }
}

#endif