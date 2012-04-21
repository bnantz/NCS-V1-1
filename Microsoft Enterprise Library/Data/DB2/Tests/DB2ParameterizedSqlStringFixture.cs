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
using IBM.Data.DB2;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests
{
    /// <summary>
    /// Tests SqlDatabase.GetSqlStringCommandWrapper when using a parameterized sql string
    /// </summary>
    [TestFixture]
    public class DB2ParameterizedSqlStringFixture
    {
        private Database db;

        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new Db2TestConfigurationContext());
            this.db = factory.CreateDatabase("DB2Test");
        }

        [Test]
        public void ExecuteSqlStringCommandWithParameters()
        {
            string sql = "select * from Region where (RegionID=?) and RegionDescription=?";
            DBCommandWrapper cmd = this.db.GetSqlStringCommandWrapper(sql);
            cmd.AddInParameter("@Param1", DbType.Int32, 1);
            cmd.AddInParameter("@Param2", DbType.String, "Eastern");
            DataSet ds = this.db.ExecuteDataSet(cmd);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
        }

        [Test]
        [ExpectedException(typeof(DB2Exception))]
        public void ExecuteSqlStringCommandWithNotEnoughParameterValues()
        {
            string sql = "select * from Region where RegionID=? and RegionDescription=?";
            DBCommandWrapper cmd = this.db.GetSqlStringCommandWrapper(sql);
            cmd.AddInParameter("@Param1", DbType.Int32, 1);
            DataSet ds = this.db.ExecuteDataSet(cmd);
        }

        [Test]
        //[ExpectedException(typeof(DB2Exception))]
            public void ExecuteSqlStringCommandWithTooManyParameterValues()
        {
            string sql = "select * from Region where RegionID=? and RegionDescription=?";
            DBCommandWrapper cmd = this.db.GetSqlStringCommandWrapper(sql);
            cmd.AddInParameter("@Param1", DbType.Int32, 1);
            cmd.AddInParameter("@Param2", DbType.String, "Eastern");
            cmd.AddInParameter("@Param3", DbType.String, "Western");
            DataSet ds = this.db.ExecuteDataSet(cmd);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
        }

        [Test]
        //[ExpectedException(typeof(DB2Exception))]
            public void ExecuteSqlStringWithoutParametersButWithValues()
        {
            string sql = "select * from Region";
            DBCommandWrapper cmd = this.db.GetSqlStringCommandWrapper(sql);
            cmd.AddInParameter("@Param1", DbType.Int32, 1);
            cmd.AddInParameter("@Param2", DbType.String, "Eastern");
            DataSet ds = this.db.ExecuteDataSet(cmd);
            Assert.AreEqual(4, ds.Tables[0].Rows.Count);
        }
    }
}

#endif