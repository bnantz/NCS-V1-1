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
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestFixture]
    public class SqlDataAccessTestsFixture : DataAccessTestsFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDefaultDatabase();
            this.dataSet = new DataSet();
            this.command = this.db.GetSqlStringCommandWrapper(this.sqlQuery);
        }

        [Test]
        public void CanGetResultSetBackWithParamaterizedQuery()
        {
            string sqlCommand = "SELECT RegionDescription FROM Region where regionId = @ID";
            DataSet ds = new DataSet();
            DBCommandWrapper cmd = this.db.GetSqlStringCommandWrapper(sqlCommand);
            cmd.AddInParameter("@ID", DbType.Int32, 4);

            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(cmd, ds, "Foo", transaction);
                    Assert.AreEqual(1, ds.Tables[0].Rows.Count);
                }
            }
        }

        [Test]
        public void OneTransactionLocksOutAnother()
        {
            DBCommandWrapper firstCommand = this.db.GetSqlStringCommandWrapper("insert into region values (99, 'Midwest')");
            DBCommandWrapper secondCommand = this.db.GetSqlStringCommandWrapper("Select * from Region");

            IDbConnection connection1 = this.db.GetConnection();
            connection1.Open();
            IDbTransaction transaction1 = connection1.BeginTransaction(IsolationLevel.Serializable);

            IDbConnection connection2 = this.db.GetConnection();
            connection2.Open();
            IDbTransaction transaction2 = connection2.BeginTransaction(IsolationLevel.Serializable);
            DataSet dataSet2 = new DataSet();
            secondCommand.Command.CommandTimeout = 1;

            try
            {
                this.db.ExecuteNonQuery(firstCommand, transaction1);
                this.db.LoadDataSet(secondCommand, dataSet2, "Foo", transaction2);
                Assert.Fail("should have thrown some funky exception");
            }
            catch (SqlException)
            {
            }
            finally
            {
                transaction1.Rollback();
                transaction1.Dispose();
                transaction2.Dispose();
                connection1.Dispose();
                connection2.Dispose();
            }
        }

        [Test]
        public void ResultsFromSelectOverTwoTablesMustReturnDataInNamedTables()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            string[] tableNames = new string[] {"RegionData", "OrderData"};

            DBCommandWrapper command = db.GetSqlStringCommandWrapper(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, tableNames);

            Assert.IsNotNull(dataSet.Tables["RegionData"]);
            Assert.IsNotNull(dataSet.Tables["OrderData"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTableNameArrayCausesException()
        {
            string selectSql = "Select * from Region; Select * from Orders";

            DBCommandWrapper command = db.GetSqlStringCommandWrapper(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, (string[])null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TableNameArrayWithNoEntriesCausesException()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            string[] tableNames = new string[0];

            DBCommandWrapper command = db.GetSqlStringCommandWrapper(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, tableNames);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTableNameInArrayCausesException()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            string[] tableNames = new string[] {"foo", null, "bar"};

            DBCommandWrapper command = db.GetSqlStringCommandWrapper(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, tableNames);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyTableNameInArrayCausesException()
        {
            string selectSql = "Select * from Region; Select * from Orders";
            string[] tableNames = new string[] {"foo", "", "bar"};

            DBCommandWrapper command = db.GetSqlStringCommandWrapper(selectSql);
            DataSet dataSet = new DataSet();
            db.LoadDataSet(command, dataSet, tableNames);
        }

    }
}

#endif