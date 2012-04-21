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
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class RefreshActionInvokerFixture
    {
        private static bool callbackHappened;
        private static string removedKey;
        private static object removedValue;
        private static CacheItemRemovedReason callbackReason;

        [SetUp]
        public void Reset()
        {
            removedValue = "Known bad value";
            callbackHappened = false;
            callbackReason = CacheItemRemovedReason.Unknown;
            removedKey = null;
        }

        [Test]
        public void NoCallHappensIfRefreshActionIsNull()
        {
            CacheItem emptyCacheItem = new CacheItem("key", "value", CacheItemPriority.Low, null);
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired);
            Assert.IsFalse(callbackHappened);
        }

        [Test]
        public void CallbackDoesHappenIfRefreshActionIsSet()
        {
            CacheItem emptyCacheItem = new CacheItem("key", "value", CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired);
            Thread.Sleep(100);
            Assert.IsTrue(callbackHappened);
        }

        [Test]
        public void RemovedValueIsGivenToCallbackMethod()
        {
            CacheItem emptyCacheItem = new CacheItem("key", "value", CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired);
            Thread.Sleep(100);
            Assert.AreEqual("value", removedValue);
        }

        [Test]
        public void RemovedValueCanBeNullDuringCallback()
        {
            CacheItem emptyCacheItem = new CacheItem("key", null, CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired);
            Thread.Sleep(100);
            Assert.IsNull(removedValue);
        }

        [Test]
        public void CallbackReasonIsGivenToCallbackMethod()
        {
            CacheItem emptyCacheItem = new CacheItem("key", null, CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Expired);
            Thread.Sleep(100);
            Assert.AreEqual(CacheItemRemovedReason.Expired, callbackReason);
        }

        [Test]
        public void CallbackReasonScavengedIsGivenToCallbackMethod()
        {
            CacheItem emptyCacheItem = new CacheItem("key", null, CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Scavenged);
            Thread.Sleep(100);
            Assert.AreEqual(CacheItemRemovedReason.Scavenged, callbackReason);
        }

        [Test]
        public void KeyIsGivenToCallbackMethod()
        {
            CacheItem emptyCacheItem = new CacheItem("key", null, CacheItemPriority.Low, new MockRefreshAction());
            RefreshActionInvoker.InvokeRefreshAction(emptyCacheItem, CacheItemRemovedReason.Scavenged);
            Thread.Sleep(100);
            Assert.AreEqual("key", removedKey);
        }

        private class MockRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {
                callbackHappened = true;
                removedValue = expiredValue;
                callbackReason = removalReason;
                removedKey = key;
            }
        }
    }
}

#endif