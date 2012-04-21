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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Tests
{
    [TestFixture]
    public class SymmetricCryptoProviderDataCollectionFixture
    {
        private class MockSymmetricCryptoProviderData : SymmetricCryptoProviderData
        {
            private string typeName;

            public MockSymmetricCryptoProviderData()
            {
                typeName = string.Empty;
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
            SymmetricCryptoProviderDataCollection collection = new SymmetricCryptoProviderDataCollection();
            Assert.AreEqual(0, collection.Count);
            MockSymmetricCryptoProviderData providerData = new MockSymmetricCryptoProviderData();
            providerData.Name = "provider1";
            collection.Add(providerData);
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void Indexer()
        {
            SymmetricCryptoProviderDataCollection collection = new SymmetricCryptoProviderDataCollection();
            MockSymmetricCryptoProviderData providerData = new MockSymmetricCryptoProviderData();
            providerData.Name = "provider1";
            collection[providerData.Name] = providerData;
            Assert.AreEqual(1, collection.Count, "count");
            SymmetricCryptoProviderData compareData1 = collection["provider1"];
            Assert.AreSame(providerData, compareData1, "string indexer");

            SymmetricCryptoProviderData compareData2 = collection[0];
            Assert.AreSame(providerData, compareData2, "int indexer");
        }
    }
}

#endif