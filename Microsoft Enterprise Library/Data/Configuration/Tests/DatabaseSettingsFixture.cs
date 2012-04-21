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

#if   UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
    [TestFixture]
    public class DatabaseSettingsFixture
    {
        private DatabaseSettings databaseSettings;

        [TestFixtureSetUp]
        public void Initialize()
        {
            databaseSettings = DatabaseSettingsBuilder.Create();
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(databaseSettings);
        }

        [Test]
        public void CountDatabaseTypes()
        {
            Assert.AreEqual(3, databaseSettings.DatabaseTypes.Count);
        }

        [Test]
        public void DefaultInstanceTest()
        {
            Assert.AreEqual("Service_Dflt", databaseSettings.DefaultInstance);
        }

        [Test]
        public void CountDatabases()
        {
            Assert.AreEqual(2, databaseSettings.Instances.Count);
        }

        [Test]
        public void CountConnectionStrings()
        {
            Assert.AreEqual(1, databaseSettings.ConnectionStrings.Count);
        }
    }
}

#endif