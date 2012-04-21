//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration.Tests
{
    [TestFixture]
    public class DatabaseSinkDataFixture
    {
        [Test]
        public void DatabaseSinkDataPropertiesTest()
        {
            DatabaseSinkData data = new DatabaseSinkData();
            string databaseInstanceName = "Service Name";
            string storedProcName = "spTest";

            data.DatabaseInstanceName = databaseInstanceName;
            data.StoredProcName = storedProcName;

            Assert.AreEqual(databaseInstanceName, data.DatabaseInstanceName);
            Assert.AreEqual(storedProcName, data.StoredProcName);
        }
    }
}

#endif