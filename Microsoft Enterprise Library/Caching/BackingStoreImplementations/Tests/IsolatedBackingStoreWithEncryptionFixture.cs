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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    [TestFixture]
    public class IsolatedBackingStoreWithEncryptionFixture : ConfigurationContextFixtureBase
    {
        private IsolatedStorageBackingStore backingStore;

        [SetUp]
        public void CreateIsolatdStorageArea()
        {
            backingStore = new IsolatedStorageBackingStore("EntLib");
        }

        public override void FixtureTeardown()
        {
            CleanOutIsolatedStorageArea();
            base.FixtureTeardown ();
        }
        
        private void CleanOutIsolatedStorageArea()
        {
            backingStore.Flush();
            backingStore.Dispose();
        }

        [Test]
        public void NullEncryptor()
        {
            MockStorageEncryptionProvider.Encrypted = false;
            MockStorageEncryptionProvider.Decrypted = false;

            CacheManagerTest("InIsoStorePersistenceWithNullEncryption");

            // second instance should load up encrypted data into in memory store
            CacheManagerTest("InIsoStorePersistenceWithNullEncryption2");

            Assert.IsTrue(MockStorageEncryptionProvider.Encrypted);
            Assert.IsTrue(MockStorageEncryptionProvider.Decrypted);
        }

        [Test]
        public void NoEncryptorDefined()
        {
            MockStorageEncryptionProvider.Encrypted = false;
            MockStorageEncryptionProvider.Decrypted = false;

            CacheManagerTest("InIsoStorePersistence");

            Assert.IsFalse(MockStorageEncryptionProvider.Encrypted);
            Assert.IsFalse(MockStorageEncryptionProvider.Decrypted);
        }

        private void CacheManagerTest(string instanceName)
        {
            CacheManagerFactory factory = new CacheManagerFactory(Context);
            CacheManager mgr = factory.GetCacheManager(instanceName);

            string key = "key1";
            string val = "value123";

            mgr.Add(key, val);

            string result = (string)mgr.GetData(key);
            Assert.AreEqual(val, result, "result");
        }
    }
}

#endif