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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests
{
	[TestFixture]
	public class DB2DatabaseFixture
	{
		private static ConfigurationContext context = new Db2TestConfigurationContext();

		[Test]
		public void ConnectionTest()
		{
			DB2Database db2Database = new DB2Database();
			db2Database.ConfigurationName = "DB2Test";
			db2Database.Initialize(new DatabaseConfigurationView(context));

			IDbConnection connection = db2Database.GetConnection();
			Assert.IsNotNull(connection);
			Assert.IsTrue(connection is DB2Connection);
			connection.Open();
		    DBCommandWrapper cmd = db2Database.GetSqlStringCommandWrapper("Select * from Region");
			cmd.CommandTimeout = 60;
			Assert.AreEqual(cmd.CommandTimeout, 60);
		}
	}
}

#endif