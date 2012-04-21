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
    [TestFixture]
    public class OracleExecuteNonQueryFixture : ExecuteNonQueryFixture
    {
        /// <summary>
        /// Initial test setup.  Insert a row into the Region table and select back the count of rows after the insert
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDatabase("OracleTest");
            this.insertString = "insert into Region values (77, 'Elbonia')";
            this.insertionCommand = this.db.GetSqlStringCommandWrapper(this.insertString);

            this.countQuery = "select count(*) from Region";
            this.countCommand = this.db.GetSqlStringCommandWrapper(this.countQuery);
        }

    }
}

#endif