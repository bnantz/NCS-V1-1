//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Tests
{
    [TestFixture]
    public class RolesProviderDataCollectionFixture
    {
        private class MockRolesProviderData : RolesProviderData
        {
            private string typeName;

            public MockRolesProviderData()
            {
            }

            public override string TypeName
            {
                get { return typeName; }
                set { typeName = value; }
            }

        }

        [Test]
        public void AddTest()
        {
            RolesProviderDataCollection collection =
                new RolesProviderDataCollection();
            Assert.AreEqual(0, collection.Count);
            MockRolesProviderData providerData =
                new MockRolesProviderData();
            providerData.Name = "provider1";
            collection.Add(providerData);
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void IndexerTest()
        {
            RolesProviderDataCollection collection =
                new RolesProviderDataCollection();
            MockRolesProviderData providerData =
                new MockRolesProviderData();
            providerData.Name = "provider1";
            collection[providerData.Name] = providerData;
            Assert.AreEqual(1, collection.Count);
            RolesProviderData compareData =
                collection["provider1"];
            Assert.AreSame(providerData, compareData);
        }

        [Test]
        public void SerializationTest()
        {
            RolesProviderDataCollection collection =
                new RolesProviderDataCollection();
            Utility.SerializationTest(collection);
        }
    }
}

#endif