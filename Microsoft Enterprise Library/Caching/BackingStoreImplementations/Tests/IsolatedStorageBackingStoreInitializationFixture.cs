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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    [TestFixture]
    public class IsolatedStorageBackingStoreInitializationFixture : ConfigurationContextFixtureBase
    {
        [Test]
        public void CanInitializeBackingStoreFromConfigurationObjects()
        {
            IsolatedStorageCacheStorageData configData = new IsolatedStorageCacheStorageData();
            configData.Name = "foo";
            configData.TypeName = typeof(IsolatedStorageBackingStore).AssemblyQualifiedName;
            configData.PartitionName = "Storage";

            IsolatedStorageBackingStore localStore = new IsolatedStorageBackingStore();
            localStore.Initialize(new TestCachingConfigurationView(configData, Context));

            localStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, null));
            localStore.Dispose();

            IsolatedStorageBackingStore testStore = new IsolatedStorageBackingStore("Storage");
            Hashtable loadedItems = testStore.Load();
            testStore.Flush();
            testStore.Dispose();

            Assert.AreEqual(1, loadedItems.Count, "One item should have been loaded into Storage backing store");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NullIsolatedStorageAreaNameThrowsException()
        {
            IsolatedStorageCacheStorageData configData = new IsolatedStorageCacheStorageData();
            configData.Name = "foo";
            configData.TypeName = typeof(IsolatedStorageBackingStore).AssemblyQualifiedName;
            configData.PartitionName = null;

            IsolatedStorageBackingStore localStore = new IsolatedStorageBackingStore();
            localStore.Initialize(new TestCachingConfigurationView(configData, Context));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void EmptyIsolatedStorageAreaNameThrowsException()
        {
            IsolatedStorageCacheStorageData configData = new IsolatedStorageCacheStorageData();
            configData.Name = "foo";
            configData.TypeName = typeof(IsolatedStorageBackingStore).AssemblyQualifiedName;
            configData.PartitionName = "";

            IsolatedStorageBackingStore localStore = new IsolatedStorageBackingStore();
            localStore.Initialize(new TestCachingConfigurationView(configData, Context));
        }

    }
}

#endif