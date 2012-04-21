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
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    [TestFixture]
    public class OracleDatabaseFixture
    {
		private static ConfigurationContext context = new TestConfigurationContext();

		[Test]
        public void ConnectionTest()
        {
            OracleDatabase oracleDatabase = new OracleDatabase();
            oracleDatabase.ConfigurationName = "OracleTest";
            oracleDatabase.Initialize(new DatabaseConfigurationView(context));
            IDbConnection connection = oracleDatabase.GetConnection();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection is OracleConnection);
            connection.Open();
		    DBCommandWrapper cmd = oracleDatabase.GetSqlStringCommandWrapper("Select * from Region");
            cmd.CommandTimeout = 0;
            Assert.AreEqual(cmd.CommandTimeout, -1);
        }
    }
}

#endif