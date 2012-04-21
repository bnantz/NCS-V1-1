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
using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class CacheItemFixture
    {
        [Test]
        public void CanExpireCacheItemFromSingleExpirationObject()
        {
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null, new AlwaysExpired());
            Assert.IsTrue(cacheItem.HasExpired(),
                          "Item should be considered expired if its expiration object is expired");
        }

        [Test]
        public void ItemShouldNotBeExpiredIfNoExpirationObjectIsExpired()
        {
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null, new NeverExpired());
            Assert.IsFalse(cacheItem.HasExpired(),
                           "No expiration object is expired, cache object should not be expired");
        }

        [Test]
        public void ItemNotExpiredIfAllExpirationsAreFalse()
        {
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null, new NeverExpired(), new NeverExpired());
            Assert.IsFalse(cacheItem.HasExpired(),
                           "No expiration in collection is expired, cache object should not be expired");
        }

        [Test]
        public void ItemIsExpiredIfAllExpirationsAreTrue()
        {
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null, new AlwaysExpired(), new AlwaysExpired());
            Assert.IsTrue(cacheItem.HasExpired(),
                          "All expirations in collection are expired, cache object is expired");
        }

        [Test]
        public void ItemIsExpiredIfAnyExpirationsAreTrue()
        {
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null, new NeverExpired(), new AlwaysExpired());
            Assert.IsTrue(cacheItem.HasExpired(),
                          "Any expirations in collection are expired, cache object is expired");
        }

        [Test]
        public void ItemIsNotExpiredIfNoExpirationsAreGiven()
        {
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null);
            Assert.IsFalse(cacheItem.HasExpired(),
                           "Cache objects with no expirations are never expired");
        }

        [Test]
        public void ItemIsNotExpiredIfEmptyExpirationArrayIsGiven()
        {
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null, new ICacheItemExpiration[0]);
            Assert.IsFalse(cacheItem.HasExpired(),
                           "Cache object with no expirations are never expired");
        }

        [Test]
        public void SingleExpirationIsNotifiedWhenCacheItemUpdated()
        {
            RecordingExpiration recordingExpiration = new RecordingExpiration();
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null, recordingExpiration);
            cacheItem.TouchedByUserAction(true);

            Assert.IsTrue(recordingExpiration.wasNotified);
        }

        [Test]
        public void AllExpirationsNotifiedWhenCacheItemUpdated()
        {
            RecordingExpiration firstExpiration = new RecordingExpiration();
            RecordingExpiration secondExpiration = new RecordingExpiration();
            CacheItem cacheItem = new CacheItem("key", "value", CacheItemPriority.Normal, null, firstExpiration, secondExpiration);
            cacheItem.TouchedByUserAction(true);

            Assert.IsTrue(firstExpiration.wasNotified);
            Assert.IsTrue(secondExpiration.wasNotified);
        }

        [Test]
        public void CtorForLoadingInitializesExpirationsToMatchLastAccessedTime()
        {
            DateTime historicalTimestamp = DateTime.Now - TimeSpan.FromHours(1.0);
            CacheItem item = new CacheItem(historicalTimestamp, "foo", "bar", CacheItemPriority.NotRemovable, null,
                                           new SlidingTime(TimeSpan.FromHours(17.0), DateTime.Now));

            SlidingTime expiration = item.Expirations[0] as SlidingTime;
            Assert.AreEqual(historicalTimestamp, expiration.TimeLastUsed);
        }

        [Serializable]
        private class RecordingExpiration : ICacheItemExpiration
        {
            public bool wasNotified = false;

            public bool HasExpired()
            {
                return false;
            }

            public void Notify()
            {
                wasNotified = true;
            }

            public void Initialize(CacheItem owningCacheItem)
            {
            }
        }
    }
}

#endif