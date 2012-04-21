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

using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
    /// <summary>
    /// Implementation of IBackingStore that stores its CacheItems into IsolatedStorage.
    /// </summary>
    /// <remarks>
    /// This class assumes a tree-structured storage schema. Each named instance of an Isolated Storage area creates a 
    /// separate, top-level directory in Isolated Storage. This is to allow a user to segregate different areas in IsoStore
    /// to allow multiple applications to use their own logically separate areas. Inside each of these areas, each CacheItem is stored
    /// in its own subdirectory, with separate files in those subdirectories representing the different pieces of a CacheItem. 
    /// The item was split like this to allow for several optimizations. The first optimization is that now, the essence of a CacheItem
    /// can be restored indepedently of the underlying value. It is the deserialization of the value object that could conceivably 
    /// be very time consuming, so by splitting it off into its own file, that deserialization process could be delayed until the value is 
    /// actually needed. The second optimization is that we are now able to update the last accessed time for a CacheItem without 
    /// to bring the entire CacheItem into memory, make the update, and then reserialize it.
    /// </remarks>
    public class IsolatedStorageBackingStore : BaseBackingStore
    {
        private string storageAreaName;
        private IsolatedStorageFile store;
        private IStorageEncryptionProvider encryptionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        [IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed=IsolatedStorageContainment.DomainIsolationByUser)]
        public IsolatedStorageBackingStore() : base()
        {
        }

        /// <summary>
        /// Initialize Isolated Storage for this CacheItem by creating the directory where it will be stored. This 
        /// constructor should only be used for testing, and never called from production code.
        /// </summary>
        /// <param name="storageAreaName">Identifier for this Isolated Storage area. May not be null.</param>
        [IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed=IsolatedStorageContainment.DomainIsolationByUser)]
        internal IsolatedStorageBackingStore(string storageAreaName)
        {
            this.storageAreaName = storageAreaName;
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageBackingStore"/> class given the configuration data
        /// </summary>
        /// <param name="configurationView">An <see cref="CachingConfigurationView"></see> object</param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof (CachingConfigurationView));

            CachingConfigurationView cachingConfigurationView = (CachingConfigurationView) configurationView;
            IsolatedStorageCacheStorageData isoStoreConfigurationData = (IsolatedStorageCacheStorageData) cachingConfigurationView.GetCacheStorageDataForCacheManager(CurrentCacheManager);

            if (isoStoreConfigurationData.PartitionName == null)
            {
                throw new ConfigurationException(SR.IsolatedStoreAreaNameNullException(isoStoreConfigurationData.Name));
            }

            if (isoStoreConfigurationData.PartitionName.Length == 0)
            {
                throw new ConfigurationException(SR.IsolatedStoreAreaNameEmptyException(isoStoreConfigurationData.Name));
            }

            this.storageAreaName = isoStoreConfigurationData.PartitionName;

            if (isoStoreConfigurationData.StorageEncryption != null)
            {
                StorageEncryptionFactory encryptionFactory = new StorageEncryptionFactory(cachingConfigurationView.ConfigurationContext);
                this.encryptionProvider = encryptionFactory.CreateSymmetricProvider(CurrentCacheManager);
            }

            Initialize();
        }

        /// <summary>
        /// Returns the number of CacheItems in the Isolated Storage segment identified by the name passed in at construction
        /// </summary>
        public override int Count
        {
            get { return GetSize(); }
        }

        /// <summary>
        /// Removes all items from this Isolated Storage area.
        /// </summary>
        public override void Flush()
        {
            lock (store)
            {
                string[] directories = store.GetDirectoryNames(GenerateSearchString(storageAreaName));
                foreach (string itemLocation in directories)
                {
                    string itemRoot = GenerateItemLocation(itemLocation);
                    RemoveItem(itemRoot);
                }
            }
        }

        /// <summary>
        /// Removes the named item from Isolated Storage.
        /// </summary>
        /// <param name="storageKey">Identifier for CacheItem to remove.</param>
        protected override void Remove(int storageKey)
        {
            lock (store)
            {
                if (ItemExists(storageKey))
                {
                    RemoveItem(GenerateItemLocation(storageKey));
                }
            }
        }

        /// <summary>
        /// Updates the last accessed time for the specified CacheItem stored in Isolated Storage
        /// </summary>
        /// <param name="storageKey">Identifer for CacheItem to remove.</param>
        /// <param name="newTimestamp">New timestamp for CacheItem.</param>
        protected override void UpdateLastAccessedTime(int storageKey, DateTime newTimestamp)
        {
            lock (store)
            {
                string itemLocation = GenerateItemLocation(storageKey);
                IsolatedStorageCacheItem storageItem =
                    new IsolatedStorageCacheItem(store, itemLocation, this.encryptionProvider);
                storageItem.UpdateLastAccessedTime(newTimestamp);
            }
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="IsolatedStorageBackingStore"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != store)
                {
                    store.Dispose();
                }
            }
        }

        /// <summary>
        /// Loads data from persistence store
        /// </summary>
        /// <returns>A Hashtable containing the cache items.</returns>
        protected override Hashtable LoadDataFromStore()
        {
            lock (store)
            {
                Hashtable itemsLoadedFromStore = new Hashtable();

                string[] itemNames = store.GetDirectoryNames(GenerateSearchString(storageAreaName));
                foreach (string itemLocation in itemNames)
                {
                    string itemName = GenerateItemLocation(itemLocation);

                    IsolatedStorageCacheItem loadedItem =
                        new IsolatedStorageCacheItem(store, itemName, this.encryptionProvider);
                    CacheItem itemLoadedFromStore = loadedItem.Load();
                    itemsLoadedFromStore.Add(itemLoadedFromStore.Key, itemLoadedFromStore);
                }
                return itemsLoadedFromStore;
            }
        }

        /// <summary>
        /// Remove existing item stored in persistence store with same key as new item
        /// </summary>
        /// <param name="storageKey">Item being removed from cache.</param>
        protected override void RemoveOldItem(int storageKey)
        {
            lock (store)
            {
                Remove(storageKey);
            }
        }

        /// <summary>
        /// Adds new item to persistence store
        /// </summary>
        /// <param name="storageKey">Unique key for storage item</param>
        /// <param name="newItem">Item to be added to cache. May not be null.</param>
        protected override void AddNewItem(int storageKey, CacheItem newItem)
        {
            lock (store)
            {
                string storageLocation = GenerateItemLocation(storageKey);
                IsolatedStorageCacheItem cacheItem =
                    new IsolatedStorageCacheItem(store, storageLocation, this.encryptionProvider);
                cacheItem.Store(newItem);
            }
        }

        private bool ItemExists(int possibleItem)
        {
            string itemLocation = GenerateItemLocation(possibleItem);
            string[] d = store.GetDirectoryNames(itemLocation);
            return d.Length == 1;
        }

        private void RemoveItem(string itemLocation)
        {
            string[] files = store.GetFileNames(GenerateSearchString(itemLocation));
            foreach (string fileName in files)
            {
                string fileToDelete = Path.Combine(itemLocation, fileName);
                store.DeleteFile(fileToDelete);
            }
            store.DeleteDirectory(itemLocation);
        }

        private int GetSize()
        {
            string[] itemsInIsolatedStorage = store.GetDirectoryNames(GenerateSearchString(storageAreaName));
            return itemsInIsolatedStorage.Length;
        }

        private string GenerateSearchString(string searchRoot)
        {
            return Path.Combine(searchRoot, "*");
        }

        private string GenerateItemLocation(string itemToLocate)
        {
            return Path.Combine(storageAreaName, itemToLocate);
        }

        private string GenerateItemLocation(int storageKey)
        {
            return GenerateItemLocation(storageKey.ToString(NumberFormatInfo.InvariantInfo));
        }

        private void Initialize()
        {
            store = IsolatedStorageFile.GetUserStoreForDomain();
            store.CreateDirectory(storageAreaName);
        }
    }
}