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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestFixture]
    public class SqlExecuteReaderFixture : ExecuteReaderFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDefaultDatabase();
            this.queryString = "Select * from Region";
            this.insertString = "Insert into Region values (99, 'Midwest')";
            this.insertCommand = this.db.GetSqlStringCommandWrapper(this.insertString);
            this.queryCommand = this.db.GetSqlStringCommandWrapper(this.queryString);
        }
    }
}

#endif