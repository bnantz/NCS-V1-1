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
    public class OracleDataAccessTestsFixture : DataAccessTestsFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDatabase("OracleTest");
            this.dataSet = new DataSet();
            this.command = this.db.GetSqlStringCommandWrapper(this.sqlQuery);
        }

        [Test]
        public void CanGetResultSetBackWithParamaterizedQuery()
        {
            string sqlCommand = "SELECT RegionDescription FROM Region where regionId = :ID";
            DataSet ds = new DataSet();
            DBCommandWrapper cmd = this.db.GetSqlStringCommandWrapper(sqlCommand);
            cmd.AddInParameter(":ID", DbType.Int32, 4);

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