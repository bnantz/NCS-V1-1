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
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class CacheFeatureTestFixture : ConfigurationContextFixtureBase, ICacheItemRefreshAction
    {
        private CacheManagerFactory factory;
        private CacheManager shortCacheManager;
        private CacheManager smallCacheManager;

        private string expiredKeys = "";
        private string expiredValues = "";
        private string removalReasons = "";

        [SetUp]
        public void StartCacheProcesses()
        {
            factory = new CacheManagerFactory(Context);
            shortCacheManager = factory.GetCacheManager("ShortInMemoryPersistence");
            smallCacheManager = factory.GetCacheManager("SmallInMemoryPersistence");

            expiredKeys = "";
            expiredValues = "";
            removalReasons = "";
        }

        [TearDown]
        public void StopCacheProcesses()
        {
            shortCacheManager.Dispose();
            smallCacheManager.Dispose();
        }

        [Test]
        public void CanConstructSystem()
        {
            Thread.Sleep(2000);
        }

        [Test]
        public void ExpirationWillRemoveItemFromCache()
        {
            shortCacheManager.Add("ExpiresImmediately", "value1", CacheItemPriority.Normal, null, new AlwaysExpired());
            Thread.Sleep(1500);
            Assert.IsNull(shortCacheManager.GetData("ExpiresImmediately"), "Expiration should have removed item from cache");
        }

        [Test]
        public void PutItThroughSomeExpirations()
        {
            shortCacheManager.Add("ExpiresImmediately", "Value1", CacheItemPriority.NotRemovable, this, new AlwaysExpired());
            shortCacheManager.Add("NeverExpires", "Value2", CacheItemPriority.NotRemovable, this, new NeverExpired());
            shortCacheManager.Add("ExpiresInFiveSeconds", "Value3", CacheItemPriority.NotRemovable, this, new AbsoluteTime(TimeSpan.FromSeconds(5.0)));
            shortCacheManager.Add("ExpiresInTwoSeconds", "Value4", CacheItemPriority.NotRemovable, this, new AbsoluteTime(TimeSpan.FromSeconds(2.0)));

            Thread.Sleep(3500);

            Assert.IsNull(shortCacheManager.GetData("ExpiresImmediately"), "This should have been expired during the first expiration run");
            Assert.IsNull(shortCacheManager.GetData("ExpiresInTwoSeconds"), "This should have been expired about 2 seconds after test started");
            Assert.IsNotNull(shortCacheManager.GetData("ExpiresInFiveSeconds"), "This should not be expired yet");

            Thread.Sleep(4000);

            Assert.IsNull(shortCacheManager.GetData("ExpiresInFiveSeconds"), "Its time had come and it should be gone");
            Assert.IsNotNull(shortCacheManager.GetData("NeverExpires"), "This item should never expire from the cache");
            Assert.AreEqual("ExpiresImmediatelyExpiresInTwoSecondsExpiresInFiveSeconds", expiredKeys);
            Assert.AreEqual("Value1Value4Value3", expiredValues);
            Assert.AreEqual("ExpiredExpiredExpired", removalReasons);
        }

        [Test]
        public void MakeItScavenge()
        {
            smallCacheManager.Add("key1", "value1", CacheItemPriority.NotRemovable, this, new NeverExpired());
            smallCacheManager.Add("key2", "value2", CacheItemPriority.High, this, new NeverExpired());
            smallCacheManager.Add("key3", "value3", CacheItemPriority.Low, this, new NeverExpired());
            smallCacheManager.Add("key4", "value4", CacheItemPriority.Normal, this, new NeverExpired());

            Thread.Sleep(1000);

            Assert.AreEqual(2, smallCacheManager.Count);
            Assert.IsNotNull(smallCacheManager.GetData("key1"));
            Assert.IsNotNull(smallCacheManager.GetData("key2"));

            Assert.AreEqual("key3key4", expiredKeys);
            Assert.AreEqual("value3value4", expiredValues);
            Assert.AreEqual("ScavengedScavenged", removalReasons);
        }

        public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
        {
            expiredKeys += key;
            expiredValues += expiredValue;
            removalReasons += removalReason.ToString();
        }
    }
}

#endif