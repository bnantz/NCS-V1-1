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
using System.Runtime.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.DatabaseAndCryptography.Tests
{
    [TestFixture]
    public class DataBackingStoreWithEncryptionFixture : ConfigurationContextFixtureBase
    {
        private DataBackingStore unencryptedBackingStore;
        private Data.Database db;
        private const string CacheManagerName = "EncryptedCacheInDatabase";

        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory dbFactory = new DatabaseProviderFactory(Context);
            db = dbFactory.CreateDefaultDatabase();
            unencryptedBackingStore = new DataBackingStore(db, "encryptionTests", null);
            unencryptedBackingStore.Flush();
        }

        public override void FixtureTeardown()
        {
            unencryptedBackingStore.Flush();
            base.FixtureTeardown ();
        }
        
        [Test]
        public void DataCanBeRetrievedWithNoEncryptionProvider()
        {
            unencryptedBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));

            Hashtable dataInCache = unencryptedBackingStore.Load();

            CacheItem retrievedItem = (CacheItem)dataInCache["key"];
            Assert.AreEqual("key", retrievedItem.Key);
            Assert.AreEqual("value", retrievedItem.Value);
            Assert.AreEqual(CacheItemPriority.Normal, retrievedItem.ScavengingPriority);
            Assert.AreEqual(typeof(MockRefreshAction), retrievedItem.RefreshAction.GetType());
            Assert.AreEqual(typeof(AlwaysExpired), retrievedItem.Expirations[0].GetType());
        }

        [Test]
        [ExpectedException(typeof(SerializationException))]
        public void AttemptingToReadEncryptedDataWithoutDecryptingThrowsException()
        {
            StorageEncryptionFactory factory = new StorageEncryptionFactory(Context);

            IStorageEncryptionProvider encryptionProvider = factory.CreateSymmetricProvider(CacheManagerName);
            DataBackingStore encryptingBackingStore = new DataBackingStore(db, "encryptionTests", encryptionProvider);

            encryptingBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));
            Hashtable dataInCache = unencryptedBackingStore.Load();
        }

        [Test]
        public void DecryptedDataCanBeReadBackFromDatabase()
        {
            StorageEncryptionFactory factory = new StorageEncryptionFactory(Context);

            IStorageEncryptionProvider encryptionProvider = factory.CreateSymmetricProvider(CacheManagerName);
            DataBackingStore encryptingBackingStore = new DataBackingStore(db, "encryptionTests", encryptionProvider);

            encryptingBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));
            Hashtable dataInCache = encryptingBackingStore.Load();

            CacheItem retrievedItem = (CacheItem)dataInCache["key"];
            Assert.AreEqual("key", retrievedItem.Key);
            Assert.AreEqual("value", retrievedItem.Value);
            Assert.AreEqual(CacheItemPriority.Normal, retrievedItem.ScavengingPriority);
            Assert.AreEqual(typeof(MockRefreshAction), retrievedItem.RefreshAction.GetType());
            Assert.AreEqual(typeof(AlwaysExpired), retrievedItem.Expirations[0].GetType());
        }

        [Serializable]
        private class MockRefreshAction : ICacheItemRefreshAction
        {
            #region ICacheItemRefreshAction Members
            public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
            {
                // TODO:  Add MockRefreshAction.Refresh implementation
            }
            #endregion
        }

    }
}

#endif