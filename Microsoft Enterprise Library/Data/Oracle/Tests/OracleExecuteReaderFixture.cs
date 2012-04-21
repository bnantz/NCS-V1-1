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
    /// <summary>
    /// Test the ExecuteReader method on the Database class
    /// </summary>
    [TestFixture]
    public class OracleExecuteReaderFixture : ExecuteReaderFixture
    {
        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDatabase("OracleTest");
            this.queryString = "Select * from Region";
            this.insertString = "Insert into Region values (99, 'Midwest')";
            this.insertCommand = this.db.GetSqlStringCommandWrapper(this.insertString);
            this.queryCommand = this.db.GetSqlStringCommandWrapper(this.queryString);
        }

        [Test]
        public void Bug869Test()
        {
            object[] paramarray = new object[2];
            paramarray[0] = "BLAUS";
            paramarray[1] = null;

            using (IDataReader dataReader = db.ExecuteReader("GetCustomersTest", paramarray))
            {
                while (dataReader.Read())
                {
                    // Get the value of the 'Name' column in the DataReader
                    Assert.IsNotNull(dataReader["ContactName"]);
                }
                dataReader.Close();

            }
        }
    }
}

#endif