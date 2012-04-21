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

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.Tests
{
    [TestFixture]
    public class SecurityCacheInstanceNodeFixture : ConfigurationDesignHostTestBase
    {

        public override void SetUp()
        {
            base.SetUp();
            CachingConfigurationDesignManager cacheManager = new CachingConfigurationDesignManager();
            cacheManager.Register(Host);
        }

        [Test]
        public void NodeTest()
        {
            int absoluteExpiration = 72;
            CacheManagerNode cacheManager = new CacheManagerNode();
            int slidingExpiration = 13;

            CachingStoreProviderNode node = new CachingStoreProviderNode();
            GeneratedApplicationNode.Nodes.Add(node);
            Assert.AreEqual(SR.SecurityInstance, node.Name);

            node.AbsoluteExpiration = absoluteExpiration;
            Assert.AreEqual(absoluteExpiration, node.AbsoluteExpiration);

            node.CacheManager = cacheManager;
            Assert.AreEqual(cacheManager, node.CacheManager);

            node.SlidingExpiration = slidingExpiration;
            Assert.AreEqual(slidingExpiration, node.SlidingExpiration);

            CachingStoreProviderData data = (CachingStoreProviderData)node.SecurityCacheProviderData;
            Assert.AreEqual(absoluteExpiration, data.AbsoluteExpiration);
            Assert.AreEqual(cacheManager.Name, data.CacheManager);
            Assert.AreEqual(slidingExpiration, data.SlidingExpiration);
        }

        [Test]
        public void DataTest()
        {
            int absoluteExpiration = 72;
            string cacheManager = "Cache Manager";
            int slidingExpiration = 13;

            CachingStoreProviderData data = new CachingStoreProviderData();
            data.AbsoluteExpiration = absoluteExpiration;
            data.CacheManager = cacheManager;
            data.SlidingExpiration = slidingExpiration;

            CachingStoreProviderNode node = new CachingStoreProviderNode(data);
            GeneratedApplicationNode.Nodes.AddWithDefaultChildren(node);

            Assert.AreEqual(absoluteExpiration, data.AbsoluteExpiration);
            Assert.AreEqual(cacheManager, data.CacheManager);
            Assert.AreEqual(slidingExpiration, data.SlidingExpiration);
        }
    }
}

#endif