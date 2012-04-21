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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class CacheManagerThreadTestFixture : ConfigurationContextFixtureBase
    {
        public static int callbackCount;
        public static CacheItemRemovedReason callbackReason;
        public static string exceptionMessage;
        public static object sharedLock;

        private CacheManagerFactory factory;
        private CacheManager cacheManager;

        [SetUp]
        public void StartCacheProcesses()
        {
            sharedLock = new object();
            callbackCount = 0;
            callbackReason = CacheItemRemovedReason.Unknown;
            exceptionMessage = "";

            factory = new MockCacheManagerFactory(Context);
            cacheManager = factory.GetCacheManager("ShortInMemoryPersistence");
        }

        [TearDown]
        public void StopCacheProcesses()
        {
            cacheManager.Dispose();
        }

        [Test]
        public void CanCreateSystem()
        {
            Thread.Sleep(500);
            lock (sharedLock)
            {
                Monitor.Pulse(sharedLock);
            }
            Thread.Sleep(100);

            string noExceptionExpectedMessage = "";
            Assert.AreEqual(noExceptionExpectedMessage, exceptionMessage);
        }

        [Test]
        public void CanAddItemBackIntoCacheAsItIsBeingExpired()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, null, new AlwaysExpired());
            Thread.Sleep(1500);

            lock (sharedLock)
            {
                cacheManager.Add("key1", "value2", CacheItemPriority.Normal, null, new NeverExpired());
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(100);

            Assert.AreEqual("value2", cacheManager.GetData("key1"));
        }

        [Test]
        public void ItemRemovedFromCacheDuringExpirationOnlyCausesRemovedCallBack()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired());
            Thread.Sleep(1500);

            lock (sharedLock)
            {
                cacheManager.Remove("key1");
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(10);

            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(CacheItemRemovedReason.Removed, callbackReason);
        }

        [Test]
        public void GetDataForExpiredItemRemovesItemFromCacheAndPreventsExpirationOfItemAsWell()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired());
            cacheManager.GetData("key1");

            Thread.Sleep(1500);

            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(CacheItemRemovedReason.Expired, callbackReason);

            lock (sharedLock)
            {
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(500);

            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(CacheItemRemovedReason.Expired, callbackReason);
        }

        [Test]
        public void FlushDoesNotCauseCallbacksInCache()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired());
            Thread.Sleep(1500);

            lock (sharedLock)
            {
                cacheManager.Flush();
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(100);

            Assert.AreEqual(0, callbackCount, "Should never be called back if flush called during expiration");
        }

        [Test]
        public void GetDataShouldCauseItemToExpireImmediatelyAndCauseExpirationCallbackToHappen()
        {
            cacheManager.Add("key1", "value1", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired());
            Thread.Sleep(1500);

            lock (sharedLock)
            {
                cacheManager.GetData("key1");
                Monitor.Pulse(sharedLock);
            }

            Thread.Sleep(100);

            Assert.AreEqual(1, callbackCount);
            Assert.AreEqual(CacheItemRemovedReason.Expired, callbackReason);
            Assert.IsNull(cacheManager.GetData("key1"), "GetData should have expired the item immediately");
        }

        [Test]
        public void TwoThreadsExecutingOneInAddAndOneInFlushShouldNotCauseDeadlock()
        {
            ThreadStart addingThreadProc = new ThreadStart(AddToCache);
            ThreadStart flushingThreadProc = new ThreadStart(FlushCache);

            Thread addingThread = new Thread(addingThreadProc);
            Thread flushingThread = new Thread(flushingThreadProc);

            addingThread.Start();
            flushingThread.Start();

            addingThread.Join();
            flushingThread.Join();

            Assert.IsTrue(true, "We finished and didn't deadlock");
        }

        private void FlushCache()
        {
            cacheManager.Flush();
        }

        private void AddToCache()
        {
            cacheManager.Add("key1", "value1");
        }

        private class RaceConditionSimulatingExpirationTask : ExpirationTask
        {
            public RaceConditionSimulatingExpirationTask(ICacheOperations cacheOperations) : base(cacheOperations)
            {
            }

            public override void PrepareForSweep()
            {
                lock (sharedLock)
                {
                    try
                    {
                        Monitor.Wait(sharedLock);
                    }
                    catch (Exception e)
                    {
                        CacheManagerThreadTestFixture.exceptionMessage = e.Message;
                    }
                }
            }
        }

        private class MockCacheManagerFactory : CacheManagerFactory
        {
            public MockCacheManagerFactory(ConfigurationContext context) : base(context)
            {
            }

            internal override ExpirationTask CreateExpirationTask(ICacheOperations cacheOperations)
            {
                return new RaceConditionSimulatingExpirationTask(cacheOperations);
            }
        }

        private class MockRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
            {
                CacheManagerThreadTestFixture.callbackCount++;
                CacheManagerThreadTestFixture.callbackReason = removalReason;
            }
        }
    }
}

#endif