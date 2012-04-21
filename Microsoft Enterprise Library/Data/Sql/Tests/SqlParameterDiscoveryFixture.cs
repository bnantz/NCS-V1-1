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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestFixture]
    public class SqlParameterDiscoveryFixture : ParameterDiscoveryFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDefaultDatabase();
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
            Assert.AreEqual("@RETURN_VALUE", ((IDataParameter)storedProcedure.Command.Parameters["@RETURN_VALUE"]).ParameterName);
            Assert.AreEqual("@CustomerID", ((IDataParameter)storedProcedure.Command.Parameters["@CustomerId"]).ParameterName);
        }

        [Test]
        public void UseParameterCachingWithPersistSecurityInfoFalse()
        {
            try
            {
                DeleteUser();
                CreateUser();

                DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
                Database dbsec = factory.CreateDatabase("NorthwindPersistFalse");
                connection = dbsec.GetConnection();
                connection.Open();

                DBCommandWrapper storedProc1 = dbsec.GetStoredProcCommandWrapper("CustOrdersOrders", "ALFKI");
                storedProc1.Command.Connection = connection;
                TestCache testCache = new TestCache();
                testCache.FillParameters(storedProc1, '@');

                DBCommandWrapper storedProc2 = dbsec.GetStoredProcCommandWrapper("CustOrdersOrders", "ALFKI");
                storedProc2.Command.Connection = connection;
                testCache.FillParameters(storedProc2, '@');

                Assert.IsTrue(testCache.CacheUsed);
            }
            finally
            {
                DeleteUser();
            }

        }

        private void CreateUser()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            Database adminDb = factory.CreateDefaultDatabase();
            connection = adminDb.GetConnection();
            connection.Open();
            string query;
            DBCommandWrapper addUser;
            try
            {
                query = "exec sp_addlogin 'entlib', 'sunrain', 'Northwind'";
                addUser = adminDb.GetSqlStringCommandWrapper(query);
                adminDb.ExecuteNonQuery(addUser);
            }
            catch
            {
            }
            try
            {
                query = "exec sp_grantdbaccess 'entlib', 'entlib'";
                addUser = adminDb.GetSqlStringCommandWrapper(query);
                adminDb.ExecuteNonQuery(addUser);
            }
            catch
            {
            }
            try
            {
                query = "exec sp_addrolemember N'db_owner', N'entlib'";
                addUser = adminDb.GetSqlStringCommandWrapper(query);
                adminDb.ExecuteNonQuery(addUser);
            }
            catch
            {
            }
        }

        private void DeleteUser()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            Database adminDb = factory.CreateDefaultDatabase();
            connection = adminDb.GetConnection();
            connection.Open();
            string query;
            DBCommandWrapper dropUser;
            try
            {
                query = "exec sp_revokedbaccess 'entlib'";
                dropUser = adminDb.GetSqlStringCommandWrapper(query);
                adminDb.ExecuteNonQuery(dropUser);
            }
            catch
            {
            }
            try
            {
                query = "exec sp_droplogin 'entlib'";
                dropUser = adminDb.GetSqlStringCommandWrapper(query);
                adminDb.ExecuteNonQuery(dropUser);
            }
            catch
            {
            }
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

                storedProcedure.DiscoverParameters('@');

                Assert.AreEqual(2, storedProcedure.Command.Parameters.Count);
            }
        }
    }
}

#endif