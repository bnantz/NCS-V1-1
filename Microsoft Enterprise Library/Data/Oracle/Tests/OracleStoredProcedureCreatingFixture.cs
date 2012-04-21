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
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Use the Data block to execute a create a stored procedure script using ExecNonQuery.
    /// </summary>
    [TestFixture]
    public class OracleStoredProcedureCreatingFixture : StoredProcedureCreatingFixture
    {
        [SetUp]
        public override void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDatabase("OracleTest");
            base.SetUp();
        }

        protected override void CreateStoredProcedure()
        {
            string storedProcedureCreation = "CREATE procedure TestProc " +
                "(vCount OUT INT, vCustomerId Orders.CustomerID%TYPE) as " +
                "BEGIN SELECT count(*)INTO vCount FROM Orders WHERE CustomerId = vCustomerId; END;";

            DBCommandWrapper command = this.db.GetSqlStringCommandWrapper(storedProcedureCreation);
            this.db.ExecuteNonQuery(command);
        }

        protected override void DeleteStoredProcedure()
        {
            string storedProcedureDeletion = "Drop procedure TestProc";
            DBCommandWrapper command = this.db.GetSqlStringCommandWrapper(storedProcedureDeletion);
            this.db.ExecuteNonQuery(command);
        }
    }
}

#endif