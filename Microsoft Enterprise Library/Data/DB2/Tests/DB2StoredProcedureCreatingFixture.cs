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
using System;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests
{
    [TestFixture]
    public class DB2StoredProcedureCreatingFixture : StoredProcedureCreatingFixture
    {
        [SetUp]
        public override void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new Db2TestConfigurationContext());
            this.db = factory.CreateDatabase("DB2Test");
            base.SetUp();
        }

        protected override void CreateStoredProcedure()
        {
            string storedProcedureCreation =
                "CREATE PROCEDURE TestProc ( OUT vCount INTEGER, IN vCustomerId CHAR(5)) " + Environment.NewLine +
                    "P1: BEGIN " + Environment.NewLine +
                    "DECLARE var0_TMP INTEGER DEFAULT 0; " + Environment.NewLine +
                    "SET var0_TMP = (SELECT COUNT(*) FROM Orders WHERE CustomerId = vCustomerId); " + Environment.NewLine +
                    "SET vCount = var0_TMP; " + Environment.NewLine +
                    "END P1";

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