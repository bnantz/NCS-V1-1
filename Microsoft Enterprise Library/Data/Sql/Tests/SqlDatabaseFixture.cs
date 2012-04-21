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
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
	[TestFixture]
	public class SqlDatabaseFixture
	{
		private static ConfigurationContext context = new TestConfigurationContext();

		[Test]
		public void ConnectionTest()
		{
			SqlDatabase sqlDatabase = new SqlDatabase();
			sqlDatabase.ConfigurationName = "NewDatabase";
			sqlDatabase.Initialize(new DatabaseConfigurationView(context));

			IDbConnection connection = sqlDatabase.GetConnection();
			Assert.IsNotNull(connection);
			Assert.IsTrue(connection is SqlConnection);
			connection.Open();
		    DBCommandWrapper cmd = sqlDatabase.GetSqlStringCommandWrapper("Select * from Region");
			cmd.CommandTimeout = 60;
			Assert.AreEqual(cmd.CommandTimeout, 60);
		}

		[Test]
		public void CanGetConnectionWithoutCredentials()
		{
			SqlDatabase sqlDatabase = new SqlDatabase();
			sqlDatabase.ConfigurationName = "DbWithSqlServerAuthn";
			sqlDatabase.Initialize(new DatabaseConfigurationView(context));
			Assert.AreEqual("server=localhost;database=northwind;", sqlDatabase.GetConnectionStringWithoutCredentials());
		}

		[Test]
		public void CanGetConnectionForStringWithNoCredentials()
		{
			SqlDatabase sqlDatabase = new SqlDatabase();
			sqlDatabase.ConfigurationName = "NewDatabase";
			sqlDatabase.Initialize(new DatabaseConfigurationView(context));

			Assert.AreEqual("server=localhost;database=northwind;integrated security=true;", sqlDatabase.GetConnectionStringWithoutCredentials());
		}
	}
}

#endif