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
using System;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
    [TestFixture]
    public class SharedDatabaseBackingStoreFixture : ConfigurationContextFixtureBase
    {
        private DataBackingStore firstCache;
        private DataBackingStore secondCache;

        public override void FixtureSetup()
        {
            base.FixtureSetup ();
            DataCacheStorageData firstCacheData = new DataCacheStorageData();
            firstCacheData.DatabaseInstanceName = "CachingDatabase";
            firstCacheData.PartitionName = "Partition1";

            firstCache = new DataBackingStore();
            firstCache.Initialize(new TestCachingConfigurationView(firstCacheData, Context));

            DataCacheStorageData secondCacheData = new DataCacheStorageData();
            secondCacheData.DatabaseInstanceName = "CachingDatabase";
            secondCacheData.PartitionName = "Partition2";

            secondCache = new DataBackingStore();
            secondCache.Initialize(new TestCachingConfigurationView(secondCacheData, Context));
        }

        [SetUp]
        public void ClearCaches()
        {
            firstCache.Flush();
            secondCache.Flush();
        }

        [TestFixtureTearDown]
        public void CloseCaches()
        {
            firstCache.Flush();
            secondCache.Flush();
        }

        [Test]
        public void AddInsertsToCorrectDatabasePartition()
        {
            firstCache.Add(new CacheItem("key", "value", CacheItemPriority.Low, null));
            Assert.AreEqual(1, firstCache.Count);
            Assert.AreEqual(0, secondCache.Count);

            secondCache.Add(new CacheItem("key2", "value2", CacheItemPriority.High, null));
            Assert.AreEqual(1, firstCache.Count);
            Assert.AreEqual(1, secondCache.Count);
        }

        [Test]
        public void LoadRetrievesFromCorrectDatabasePartition()
        {
            firstCache.Add(new CacheItem("key1", "value1", CacheItemPriority.Low, null));

            Hashtable firstDatabaseContents = firstCache.Load();
            Assert.AreEqual(1, firstDatabaseContents.Count);

            Hashtable secondDatabaseContents = secondCache.Load();
            Assert.AreEqual(0, secondDatabaseContents.Count);
        }

        [Test]
        public void FlushOnClearsProperPartition()
        {
            secondCache.Add(new CacheItem("k1", "v1", CacheItemPriority.Low, null));

            firstCache.Flush();
            Assert.AreEqual(1, secondCache.Count);

            secondCache.Flush();
            Assert.AreEqual(0, secondCache.Count);
        }

        [Test]
        public void RemoveOnRemovesFromCorrectPartition()
        {
            firstCache.Add(new CacheItem("key", "value", CacheItemPriority.Low, null));

            secondCache.Remove("key");
            Assert.AreEqual(1, firstCache.Count);

            firstCache.Remove("key");
            Assert.AreEqual(0, firstCache.Count);
        }

        [Test]
        public void UpdateLastAccessTimeOnlyAffectsItemInCorrectPartition()
        {
            CacheItem item = new CacheItem("key", "value", CacheItemPriority.Low, null);
            DateTime originalTimeStamp = item.LastAccessedTime;

            firstCache.Add(item);

            secondCache.UpdateLastAccessedTime("key", DateTime.Now + TimeSpan.FromHours(1.0));

            Hashtable firstCacheContents = firstCache.Load();
            CacheItem retrievedItem = firstCacheContents["key"] as CacheItem;
            Assert.AreEqual(originalTimeStamp.ToString(), retrievedItem.LastAccessedTime.ToString());

            DateTime newTimestamp = DateTime.Now + TimeSpan.FromMinutes(2.0);
            firstCache.UpdateLastAccessedTime("key", newTimestamp);

            Hashtable afterChangeContents = firstCache.Load();
            CacheItem changedItem = afterChangeContents["key"] as CacheItem;
            Assert.AreEqual(newTimestamp.ToString(), changedItem.LastAccessedTime.ToString());
        }
    }
}

#endif