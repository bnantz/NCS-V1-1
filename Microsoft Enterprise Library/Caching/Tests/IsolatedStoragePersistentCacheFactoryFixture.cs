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

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class IsolatedStoragePersistentCacheFactoryFixture : ConfigurationContextFixtureBase
    {
        private CacheManagerFactory factory;
        private CacheManager cacheManager;

        [SetUp]
        public void CreateCacheManager()
        {
            factory = new CacheManagerFactory(Context);
            cacheManager = factory.GetCacheManager("InIsoStorePersistence");
        }

        [TearDown]
        public void ReleaseCacheManager()
        {
            cacheManager.Flush();
            cacheManager.Dispose();
        }

        [Test]
        public void CanCreateIsolatedStorageCacheManager()
        {
            cacheManager.Add("bab", "foo");
            Assert.AreEqual(1, cacheManager.Count);

            CacheManagerFactory differentFactory = new CacheManagerFactory(Context);
            CacheManager differentCacheManager = differentFactory.GetCacheManager("InIsoStorePersistence");
            int count = differentCacheManager.Count;
            differentCacheManager.Dispose();

            Assert.AreEqual(1, count, "If we actually persisted added item, different cache manager should see item, too.");
        }
    }
}

#endif