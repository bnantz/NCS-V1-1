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
    public class DB2DataAccessTestsFixture : DataAccessTestsFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new Db2TestConfigurationContext());
            this.db = factory.CreateDatabase("DB2Test");
            this.dataSet = new DataSet();
            this.command = this.db.GetSqlStringCommandWrapper(this.sqlQuery);
        }

        [Test]
        public void CanGetResultSetBackWithParamaterizedQuery()
        {
            string sqlCommand = "SELECT RegionDescription FROM Region where regionId = ?";
            DataSet ds = new DataSet();
            DBCommandWrapper cmd = this.db.GetSqlStringCommandWrapper(sqlCommand);
            cmd.AddInParameter("?", DbType.Int32, 4);

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
    }
}

#endif