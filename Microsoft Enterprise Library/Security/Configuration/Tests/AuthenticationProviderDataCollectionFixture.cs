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
    public class AuthenticationProviderDataCollectionFixture
    {
        private class MockAuthenticationProviderData : AuthenticationProviderData
        {
            private string typeName;

            public MockAuthenticationProviderData()
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
            AuthenticationProviderDataCollection collection =
                new AuthenticationProviderDataCollection();
            Assert.AreEqual(0, collection.Count);
            MockAuthenticationProviderData providerData =
                new MockAuthenticationProviderData();
            providerData.Name = "provider1";
            collection.Add(providerData);
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void IndexerTest()
        {
            AuthenticationProviderDataCollection collection =
                new AuthenticationProviderDataCollection();
            MockAuthenticationProviderData providerData =
                new MockAuthenticationProviderData();
            providerData.Name = "provider1";
            collection[providerData.Name] = providerData;
            Assert.AreEqual(1, collection.Count);
            AuthenticationProviderData compareData =
                collection["provider1"];
            Assert.AreSame(providerData, compareData);
        }

        //        [Test]
        //        public void DefaultProviderTest()
        //        {
        //            AuthenticationProviderDataCollection collection =
        //                new AuthenticationProviderDataCollection();
        //            Assert.IsNull(collection.DefaultProvider);
        //            collection.DefaultProvider = "provider1";
        //            Assert.IsNotNull(collection.DefaultProvider);
        //        }

        [Test]
        public void SerializationTest()
        {
            AuthenticationProviderDataCollection collection =
                new AuthenticationProviderDataCollection();
            Utility.SerializationTest(collection);
        }
    }
}

#endif