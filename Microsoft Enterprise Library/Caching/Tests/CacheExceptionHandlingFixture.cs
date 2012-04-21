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
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class CacheExceptionHandlingFixture : ICacheScavenger
    {
        [Test]
        public void ExceptionThrownDuringAddResultsInObjectBeingRemovedFromCacheCompletely()
        {
            TestConfigurationContext context = new TestConfigurationContext();
            MockBackingStore backingStore = new MockBackingStore();
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", new CachingConfigurationView(context));

            Cache cache = new Cache(backingStore, scavengingPolicy);
            cache.Initialize(this);

            try
            {
                cache.Add("foo", "bar");
                Assert.Fail("Should have thrown exception thrown internally to Cache.Add");
            }
            catch (Exception)
            {
                Assert.IsFalse(cache.Contains("foo"));
                Assert.AreEqual(1, backingStore.removalCount);
            }
        }

        [Test]
        public void ExceptionThrownDuringAddIntoIsolatedStorageAllowsItemToBeReaddedLater()
        {
            TestConfigurationContext context = new TestConfigurationContext();
            using (IsolatedStorageBackingStore backingStore = new IsolatedStorageBackingStore("foo"))
            {
                CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", new CachingConfigurationView(context));

                Cache cache = new Cache(backingStore, scavengingPolicy);
                cache.Initialize(this);

                try
                {
                    cache.Add("my", new NonSerializableClass());
                    Assert.Fail("Should have thrown exception internally to Cache.Add");
                }
                catch (Exception)
                {
                    cache.Add("my", new SerializableClass());
                    Assert.IsTrue(cache.Contains("my"));
                }
            }
        }

        [Test]
        public void ItemAddedPreviousToFailedAddIsRemovedCompletelyIfSecondAddFails()
        {
            TestConfigurationContext context = new TestConfigurationContext();
            using (IsolatedStorageBackingStore backingStore = new IsolatedStorageBackingStore("foo"))
            {
                CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy("test", new CachingConfigurationView(context));

                Cache cache = new Cache(backingStore, scavengingPolicy);
                cache.Initialize(this);

                cache.Add("my", new SerializableClass());

                try
                {
                    cache.Add("my", new NonSerializableClass());
                    Assert.Fail("Should have thrown exception internally to Cache.Add");
                }
                catch (Exception)
                {
                    Assert.IsFalse(cache.Contains("my"));
                    Assert.AreEqual(0, backingStore.Count);

                    Hashtable isolatedStorageContents = backingStore.Load();
                    Assert.AreEqual(0, isolatedStorageContents.Count);
                }
            }
        }

        private class NonSerializableClass
        {
        }

        [Serializable]
        private class SerializableClass
        {
        }

        private class MockBackingStore : ConfigurationProvider, IBackingStore
        {
            public int removalCount = 0;

            public string CurrentCacheManager
            {
                get { return string.Empty; }
                set {  }
            }

            public int Count
            {
                get
                {
                    // TODO:  Add MockBackingStore.Count getter implementation
                    return 0;
                }
            }

            public void Add(CacheItem newCacheItem)
            {
                throw new Exception();
            }

            public void Remove(string key)
            {
                this.removalCount++;
            }

            public void UpdateLastAccessedTime(string key, DateTime timestamp)
            {
                // TODO:  Add MockBackingStore.UpdateLastAccessedTime implementation
            }

            public void Flush()
            {
                // TODO:  Add MockBackingStore.Flush implementation
            }

            public Hashtable Load()
            {
                // TODO:  Add MockBackingStore.Load implementation
                return new Hashtable();
            }

            public override void Initialize(ConfigurationView configurationView)
            {
                // TODO:  Add MockBackingStore.Initialize implementation
            }

            public void Dispose()
            {
                // TODO:  Add MockBackingStore.Dispose implementation
            }
        }

        public void StartScavenging()
        {
            // TODO:  Add CacheExceptionHandlingFixture.StartScavenging implementation
        }
    }
}

#endif