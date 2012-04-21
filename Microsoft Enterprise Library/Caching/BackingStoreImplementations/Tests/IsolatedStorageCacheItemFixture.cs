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
using System.Diagnostics;
using System.IO.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    [TestFixture]
    public class IsolatedStorageCacheItemFixture : ConfigurationContextFixtureBase
    {
        private IsolatedStorageFile storage;
        private const string DirectoryName = "1234567890123";
        private const string DirectoryRoot = DirectoryName + @"\";

        [SetUp]
        public void CreateIsolatedStorage()
        {
            storage = IsolatedStorageFile.GetUserStoreForDomain();
            storage.CreateDirectory(DirectoryName);
        }

        [TearDown]
        public void FlushIsolatedStorage()
        {
            string[] directories = storage.GetDirectoryNames(DirectoryRoot + "*");
            foreach (string directoryName in directories)
            {
                string directoryRoot = DirectoryRoot + directoryName;
                string[] files = storage.GetFileNames(directoryRoot + @"\*");
                foreach (string fileName in files)
                {
                    string fileToDelete = directoryRoot + @"\" + fileName;
                    storage.DeleteFile(fileToDelete);
                }
                storage.DeleteDirectory(directoryRoot);
            }

            storage.DeleteDirectory(DirectoryName);
        }

        [Test]
        public void StoreMinimalCacheItemIntoIsolatedStorage()
        {
            CacheItem itemToStore = new CacheItem("key", "value", CacheItemPriority.NotRemovable, null);

            string itemRoot = DirectoryRoot + itemToStore.Key;
            IsolatedStorageCacheItem item = new IsolatedStorageCacheItem(storage, itemRoot, null);
            item.Store(itemToStore);

            Assert.AreEqual(1, storage.GetDirectoryNames(DirectoryRoot + @"\*").Length);
        }

        [Test]
        public void ReadMinimalCacheItemFromIsolatedStorage()
        {
            ReadMinimalCacheItemFromIsolatedStorageEncrypted(false);
        }

        private void ReadMinimalCacheItemFromIsolatedStorageEncrypted(bool encrypted)
        {
            DateTime historicalTimestamp = DateTime.Now - TimeSpan.FromHours(1.0);

            CacheItem itemToStore = new CacheItem("key1", "value1", CacheItemPriority.Normal, null);
            itemToStore.SetLastAccessedTime(historicalTimestamp);

            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, encrypted);

            Assert.AreEqual("key1", readItem.Key);
            Assert.AreEqual("value1", readItem.Value);
            Assert.AreEqual(CacheItemPriority.Normal, readItem.ScavengingPriority);
            Assert.IsNull(readItem.RefreshAction);
            Assert.AreEqual(0, readItem.Expirations.Length);
            Assert.AreEqual(historicalTimestamp, readItem.LastAccessedTime);
        }

        [Test]
        public void ReadCacheItemWithNullValue()
        {
            CacheItem itemToStore = new CacheItem("key", null, CacheItemPriority.Normal, null);
            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, false);

            Assert.IsNull(readItem.Value);
            Assert.AreEqual(itemToStore.Key, readItem.Key);
        }

        [Test]
        public void ReadCacheItemWithRefreshAction()
        {
            CacheItem itemToStore = new CacheItem("key", null, CacheItemPriority.Normal, new MockRefreshAction());
            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, false);

            Assert.IsNotNull(readItem.RefreshAction, "MockRefreshAction should have been stored into IsoStore and retrieved.");
        }

        [Test]
        public void ReadCacheItemWithOneExpirationAction()
        {
            CacheItem itemToStore = new CacheItem("monkey", "baboon", CacheItemPriority.NotRemovable, null, new NeverExpired());
            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, false);

            Assert.AreEqual(1, readItem.Expirations.Length);
            Assert.AreEqual(typeof(NeverExpired), readItem.Expirations[0].GetType());
        }

        [Test]
        public void ReadCacheItemWithThreeExpirationActions()
        {
            CacheItem itemToStore = new CacheItem("monkey", "baboon", CacheItemPriority.NotRemovable, null,
                                                  new NeverExpired(), new AlwaysExpired(), new AbsoluteTime(TimeSpan.FromSeconds(1.0)));
            CacheItem readItem = DoCacheItemRoundTripToStorage(itemToStore, false);

            Assert.AreEqual(3, readItem.Expirations.Length);
            Assert.AreEqual(typeof(NeverExpired), readItem.Expirations[0].GetType());
            Assert.AreEqual(typeof(AlwaysExpired), readItem.Expirations[1].GetType());
            Assert.AreEqual(typeof(AbsoluteTime), readItem.Expirations[2].GetType());
        }

        [Test]
        public void UpdateLastAccessedTime()
        {
            CacheItem historicalItem = new CacheItem("foo", "bar", CacheItemPriority.NotRemovable, null);
            historicalItem.SetLastAccessedTime(DateTime.Now - TimeSpan.FromHours(1.0));

            string itemRoot = DirectoryRoot + historicalItem.Key;
            IsolatedStorageCacheItem item = new IsolatedStorageCacheItem(storage, itemRoot, null);
            item.Store(historicalItem);

            DateTime now = DateTime.Now;
            historicalItem.SetLastAccessedTime(DateTime.Now);
            item.UpdateLastAccessedTime(now);

            CacheItem restoredItem = item.Load();

            Assert.AreEqual(restoredItem.LastAccessedTime, now);
        }

        [Test]
        public void StoreAndEncryptItem()
        {
            IStorageEncryptionProvider encryptionProvider = new MockStorageEncryptionProvider();
            encryptionProvider.Initialize(new CachingConfigurationView(Context));

            CacheItem itemToStore = new CacheItem("key", "value", CacheItemPriority.NotRemovable, null);

            string itemRoot = DirectoryRoot + itemToStore.Key;
            IsolatedStorageCacheItem item = new IsolatedStorageCacheItem(storage, itemRoot, encryptionProvider);
            item.Store(itemToStore);

            Assert.AreEqual(1, storage.GetDirectoryNames(DirectoryRoot + @"\*").Length);
        }

        [Test]
        public void ReadMinimalCacheItemFromIsolatedStorageEncrypted()
        {
            ReadMinimalCacheItemFromIsolatedStorageEncrypted(true);
        }

        private CacheItem DoCacheItemRoundTripToStorage(CacheItem itemToStore, bool encrypted)
        {
            IStorageEncryptionProvider encryptionProvider = null;

            if (encrypted)
            {
                encryptionProvider = new MockStorageEncryptionProvider();
                encryptionProvider.Initialize(new CachingConfigurationView(Context));
            }

            string itemRoot = DirectoryRoot + itemToStore.Key;

            IsolatedStorageCacheItem item = new IsolatedStorageCacheItem(storage, itemRoot, encryptionProvider);
            item.Store(itemToStore);

            IsolatedStorageCacheItem itemToRead = new IsolatedStorageCacheItem(storage, itemRoot, encryptionProvider);
            return itemToRead.Load();
        }

        [Serializable]
        private class MockRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
            {
            }
        }
    }
}

#endif