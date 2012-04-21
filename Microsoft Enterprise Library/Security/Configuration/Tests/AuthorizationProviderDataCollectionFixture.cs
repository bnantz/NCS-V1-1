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
    public class AuthorizationProviderDataCollectionFixture
    {
        private class MockAuthorizationProviderData : AuthorizationProviderData
        {
            private string typeName;

            public MockAuthorizationProviderData()
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
        public void AddTest()
        {
            AuthorizationProviderDataCollection collection =
                new AuthorizationProviderDataCollection();
            Assert.AreEqual(0, collection.Count);
            MockAuthorizationProviderData providerData =
                new MockAuthorizationProviderData();
            providerData.Name = "provider1";
            collection.Add(providerData);
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void IndexerTest()
        {
            AuthorizationProviderDataCollection collection =
                new AuthorizationProviderDataCollection();
            MockAuthorizationProviderData providerData =
                new MockAuthorizationProviderData();
            providerData.Name = "provider1";
            collection[providerData.Name] = providerData;
            Assert.AreEqual(1, collection.Count);
            AuthorizationProviderData compareData =
                collection["provider1"];
            Assert.AreSame(providerData, compareData);
        }

        [Test]
        public void SerializationTest()
        {
            AuthorizationProviderDataCollection collection =
                new AuthorizationProviderDataCollection();
            Utility.SerializationTest(collection);
        }
    }
}

#endif