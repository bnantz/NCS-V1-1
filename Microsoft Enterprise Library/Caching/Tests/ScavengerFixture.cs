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
using System.Collections;
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class ScavengerFixture : ICacheOperations
    {
        private string scavengedKeys = "";
        private Hashtable inMemoryCache;

        [SetUp]
        public void SetUp()
        {
            scavengedKeys = "";
            inMemoryCache = new Hashtable();
        }

        [Test]
        public void WillRemoveSingleItemFromCache()
        {
            TestConfigurationContext context = new TestConfigurationContext();
            CachingConfigurationView view = new CachingConfigurationView(context);
            view.GetCacheManagerSettings().CacheManagers["test"].MaximumElementsInCacheBeforeScavenging = 0;

            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", view);
            ScavengerTask scavenger = new ScavengerTask("test", view, scavengingPolicy, this);
            CacheItem itemToRemove = new CacheItem("key", "value", CacheItemPriority.Low, null);
            itemToRemove.MakeEligibleForScavenging();
            AddCacheItem("key", itemToRemove);

            scavenger.DoScavenging();

            Assert.AreEqual("key", scavengedKeys);
        }

        [Test]
        public void WillNotRemoveItemIfNotEligibleForScavenging()
        {
            TestConfigurationContext context = new TestConfigurationContext();
            CachingConfigurationView view = new CachingConfigurationView(context);
            view.GetCacheManagerSettings().CacheManagers["test"].MaximumElementsInCacheBeforeScavenging = 1;
            view.GetCacheManagerSettings().CacheManagers["test"].NumberToRemoveWhenScavenging = 0;

            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", view);
            ScavengerTask scavenger = new ScavengerTask("test", view, scavengingPolicy, this);
            CacheItem itemToRemove = new CacheItem("key", "value", CacheItemPriority.Low, null);
            itemToRemove.MakeNotEligibleForScavenging();
            AddCacheItem("key", itemToRemove);

            scavenger.DoScavenging();

            Assert.AreEqual("", scavengedKeys);
        }

        [Test]
        public void WillRemoveMultipleEligibleForScavenging()
        {
            TestConfigurationContext context = new TestConfigurationContext();
            CachingConfigurationView view = new CachingConfigurationView(context);
            view.GetCacheManagerSettings().CacheManagers["test"].MaximumElementsInCacheBeforeScavenging = 2;
            view.GetCacheManagerSettings().CacheManagers["test"].NumberToRemoveWhenScavenging = 3;

            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", view);
            ScavengerTask scavenger = new ScavengerTask("test", view, scavengingPolicy, this);
            CacheItem itemToRemove = new CacheItem("key1", "value", CacheItemPriority.High, null);
            CacheItem itemToRemain = new CacheItem("key2", "value", CacheItemPriority.Low, null);
            CacheItem itemToRemoveAlso = new CacheItem("key3", "value", CacheItemPriority.Normal, null);

            itemToRemove.MakeEligibleForScavenging();
            itemToRemain.MakeEligibleForScavenging();
            itemToRemoveAlso.MakeEligibleForScavenging();

            AddCacheItem("key1", itemToRemove);
            AddCacheItem("key2", itemToRemain);
            AddCacheItem("key3", itemToRemoveAlso);

            scavenger.DoScavenging();

            Assert.AreEqual("key2key3key1", scavengedKeys);
        }

        [Test]
        public void WillStopRemovingAtLimitForScavenging()
        {
            TestConfigurationContext context = new TestConfigurationContext();
            CachingConfigurationView view = new CachingConfigurationView(context);
            view.GetCacheManagerSettings().CacheManagers["test"].MaximumElementsInCacheBeforeScavenging = 2;
            view.GetCacheManagerSettings().CacheManagers["test"].NumberToRemoveWhenScavenging = 2;
       
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", view);
            ScavengerTask scavenger = new ScavengerTask("test", view, scavengingPolicy, this);
            CacheItem itemToRemove = new CacheItem("key1", "value", CacheItemPriority.High, null);
            CacheItem itemToRemain = new CacheItem("key2", "value", CacheItemPriority.Low, null);
            CacheItem itemToRemoveAlso = new CacheItem("key3", "value", CacheItemPriority.Normal, null);

            itemToRemove.MakeEligibleForScavenging();
            itemToRemain.MakeEligibleForScavenging();
            itemToRemoveAlso.MakeEligibleForScavenging();

            AddCacheItem("key1", itemToRemove);
            AddCacheItem("key2", itemToRemain);
            AddCacheItem("key3", itemToRemoveAlso);

            scavenger.DoScavenging();

            Assert.AreEqual("key2key3", scavengedKeys);
        }

        [Test]
        public void WillNotDieIfNotEnoughItemsToScavenge()
        {
            TestConfigurationContext context = new TestConfigurationContext();
            CachingConfigurationView view = new CachingConfigurationView(context);
            view.GetCacheManagerSettings().CacheManagers["test"].MaximumElementsInCacheBeforeScavenging = 2;
            view.GetCacheManagerSettings().CacheManagers["test"].NumberToRemoveWhenScavenging = 4;
      
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", view);
            ScavengerTask scavenger = new ScavengerTask("test", view, scavengingPolicy, this);
            CacheItem itemToRemove = new CacheItem("key1", "value", CacheItemPriority.High, null);
            CacheItem itemToRemain = new CacheItem("key2", "value", CacheItemPriority.Low, null);
            CacheItem itemToRemoveAlso = new CacheItem("key3", "value", CacheItemPriority.Normal, null);

            itemToRemove.MakeEligibleForScavenging();
            itemToRemain.MakeEligibleForScavenging();
            itemToRemoveAlso.MakeEligibleForScavenging();

            AddCacheItem("key1", itemToRemove);
            AddCacheItem("key2", itemToRemain);
            AddCacheItem("key3", itemToRemoveAlso);

            scavenger.DoScavenging();

            Assert.AreEqual("key2key3key1", scavengedKeys);

        }

        [Test]
        public void WillScavenge()
        {
            CacheItem item1 = new CacheItem("key1", "value1", CacheItemPriority.NotRemovable, null);
            CacheItem item2 = new CacheItem("key2", "value2", CacheItemPriority.High, null);
            CacheItem item3 = new CacheItem("key3", "value3", CacheItemPriority.Normal, null);
            CacheItem item4 = new CacheItem("key4", "value4", CacheItemPriority.Low, null);

            AddCacheItem("key1", item1);
            AddCacheItem("key2", item2);
            AddCacheItem("key3", item3);
            AddCacheItem("key4", item4);

            TestConfigurationContext context = new TestConfigurationContext();
            CachingConfigurationView view = new CachingConfigurationView(context);
            view.GetCacheManagerSettings().CacheManagers["test"].MaximumElementsInCacheBeforeScavenging = 1;
            view.GetCacheManagerSettings().CacheManagers["test"].NumberToRemoveWhenScavenging = 2;

            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", view);
            ScavengerTask scavenger = new ScavengerTask("test", view, scavengingPolicy, this);
            scavenger.DoScavenging();

            Assert.AreEqual("key4key3", scavengedKeys);
        }

        [Test]
        public void CanScavengeInBackground()
        {
            CacheItem item1 = new CacheItem("key1", "value1", CacheItemPriority.Low, null);
            CacheItem item2 = new CacheItem("key2", "value2", CacheItemPriority.Normal, null);
            CacheItem item3 = new CacheItem("key3", "value3", CacheItemPriority.High, null);

            AddCacheItem("key1", item1);
            AddCacheItem("key2", item2);
            AddCacheItem("key3", item3);

            TestConfigurationContext context = new TestConfigurationContext();
            CachingConfigurationView view = new CachingConfigurationView(context);
            view.GetCacheManagerSettings().CacheManagers["test"].MaximumElementsInCacheBeforeScavenging = 2;
            view.GetCacheManagerSettings().CacheManagers["test"].NumberToRemoveWhenScavenging = 1;

            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", view);
            ScavengerTask scavenger = new ScavengerTask("test", view, scavengingPolicy, this);
            BackgroundScheduler scheduler = new BackgroundScheduler(null, scavenger);
            scheduler.Start();

            Thread.Sleep(500);
            scheduler.StartScavenging();
            Thread.Sleep(250);

            scheduler.Stop();
            Thread.Sleep(250);

            Assert.AreEqual("key1", scavengedKeys);
        }

        private void AddCacheItem(string key, CacheItem cacheItem)
        {
            inMemoryCache[key] = cacheItem;
        }

        public Hashtable GetCurrentCacheState()
        {
            return inMemoryCache;
        }

        public void RemoveItemFromCache(string keyToRemove, CacheItemRemovedReason removalReason)
        {
            scavengedKeys += keyToRemove;
        }
    }
}

#endif