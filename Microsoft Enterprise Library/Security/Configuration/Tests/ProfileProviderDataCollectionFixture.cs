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
    public class ProfileProviderDataCollectionFixture
    {
        private class MockProfileProviderData : ProfileProviderData
        {
            private string typeName;

            public MockProfileProviderData()
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
            ProfileProviderDataCollection collection =
                new ProfileProviderDataCollection();
            Assert.AreEqual(0, collection.Count);
            MockProfileProviderData providerData =
                new MockProfileProviderData();
            providerData.Name = "provider1";
            collection.Add(providerData);
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void IndexerTest()
        {
            ProfileProviderDataCollection collection =
                new ProfileProviderDataCollection();
            MockProfileProviderData providerData =
                new MockProfileProviderData();
            providerData.Name = "provider1";
            collection[providerData.Name] = providerData;
            Assert.AreEqual(1, collection.Count);
            ProfileProviderData compareData =
                collection["provider1"];
            Assert.AreSame(providerData, compareData);
        }

        [Test]
        public void SerializationTest()
        {
            ProfileProviderDataCollection collection =
                new ProfileProviderDataCollection();
            Utility.SerializationTest(collection);
        }
    }
}

#endif