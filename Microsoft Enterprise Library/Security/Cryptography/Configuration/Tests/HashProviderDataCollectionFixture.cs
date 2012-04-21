//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Tests
{
    [TestFixture]
    public class HashProviderDataCollectionFixture
    {
        private class MockHashProviderData : HashProviderData
        {
            private string typeName;

            public MockHashProviderData()
            {
                typeName = typeof(MockHashProvider).AssemblyQualifiedName;
            }

            public override string TypeName
            {
                get { return typeName; }
                set { typeName = value; }
            }

        }

        [Test]
        public void AddItem()
        {
            HashProviderDataCollection collection = new HashProviderDataCollection();
            Assert.AreEqual(0, collection.Count);
            MockHashProviderData providerData = new MockHashProviderData();
            providerData.Name = "provider1";
            collection.Add(providerData);
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void Indexer()
        {
            HashProviderDataCollection collection = new HashProviderDataCollection();
            MockHashProviderData providerData = new MockHashProviderData();
            providerData.Name = "provider1";
            collection[providerData.Name] = providerData;
            Assert.AreEqual(1, collection.Count, "count");
            HashProviderData compareData1 = collection["provider1"];
            Assert.AreSame(providerData, compareData1, "string indexer");

            HashProviderData compareData2 = collection[0];
            Assert.AreSame(providerData, compareData2, "int indexer");
        }
    }
}

#endif