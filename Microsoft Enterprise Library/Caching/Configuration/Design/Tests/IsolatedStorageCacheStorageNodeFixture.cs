//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestFixture]
    public class IsolatedStorageCacheStorageNodeFixture
    {
        [Test]
        public void NodeTest()
        {
            string isolatedStorageAreaName = "testStorageAreaName";

            IsolatedStorageCacheStorageNode node = new IsolatedStorageCacheStorageNode();

            node.PartitionName = isolatedStorageAreaName;
            Assert.AreEqual(isolatedStorageAreaName, node.PartitionName);

            IsolatedStorageCacheStorageData data = (IsolatedStorageCacheStorageData)node.CacheStorageData;
            Assert.AreEqual(isolatedStorageAreaName, data.PartitionName);

        }

        [Test]
        public void DataTest()
        {
            string isolatedStorageAreaName = "testStorageAreaName";

            IsolatedStorageCacheStorageData data = new IsolatedStorageCacheStorageData();
            data.PartitionName = isolatedStorageAreaName;

            IsolatedStorageCacheStorageNode node = new IsolatedStorageCacheStorageNode(data);
            Assert.AreEqual(isolatedStorageAreaName, node.PartitionName);
        }
    }
}

#endif