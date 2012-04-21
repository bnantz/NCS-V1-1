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
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    [TestFixture]
    public class SlidingTimeFixture
    {
        [Test]
        public void WillUpdateLastTouchedTimeWhenNotified()
        {
            DateTime initialTimeStamp = DateTime.Now - TimeSpan.FromSeconds(5.0);
            TimeSpan expirationWindow = TimeSpan.FromSeconds(5.0);
            SlidingTime slidingTime = new SlidingTime(expirationWindow, initialTimeStamp);

            DateTime now = DateTime.Now;

            slidingTime.Notify();

            Assert.IsTrue(slidingTime.TimeLastUsed >= now);
        }

        [Test]
        public void CanInitializeWithLastUpdatedTimeFromCacheItem()
        {
            CacheItem item = new CacheItem("key", "value", CacheItemPriority.Normal, null);
            DateTime timestampToSave = DateTime.Now + TimeSpan.FromDays(1.0);
            item.SetLastAccessedTime(timestampToSave);

            DateTime initialTimeStamp = DateTime.Now - TimeSpan.FromSeconds(5.0);
            TimeSpan expirationWindow = TimeSpan.FromSeconds(5.0);
            SlidingTime slidingTime = new SlidingTime(expirationWindow, initialTimeStamp);

            slidingTime.Initialize(item);

            Assert.AreEqual(timestampToSave, slidingTime.TimeLastUsed);
        }

        [Test]
        public void WillExpireOnSchedule()
        {
            SlidingTime expiration = new SlidingTime(TimeSpan.FromSeconds(1.5));
            Thread.Sleep(2000);
            Assert.IsTrue(expiration.HasExpired(), "Should have expired after enough time elapsed");
        }
    }
}

#endif