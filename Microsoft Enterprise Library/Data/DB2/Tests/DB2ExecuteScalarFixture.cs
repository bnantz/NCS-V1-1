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
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests
{
    /// <summary>
    /// Test the ExecuteScalar method on the Database class
    /// </summary>
    [TestFixture]
    public class DB2ExecuteScalarFixture : ExecuteScalarFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new Db2TestConfigurationContext());
            this.db = factory.CreateDatabase("DB2Test");

            this.sqlCommand = "Select count(*) from region";
            this.command = this.db.GetSqlStringCommandWrapper(this.sqlCommand);
        }
    }

}

#endif