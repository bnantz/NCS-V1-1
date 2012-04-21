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
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EntLibData = Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database
{
    /// <summary>
    /// Implementation of a BackingStore to allow CacheItems to be stored through Data block
    /// </summary>
    public class DataBackingStore : BaseBackingStore
    {
        private Data.Database database;
        private IStorageEncryptionProvider encryptionProvider;
        private string partitionName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataBackingStore() : base()
        {
        }

        /// <summary>
        /// This is public purely for unit testing purposes and should never be called by application code
        /// </summary>
        /// <param name="database">Database to use for persistence</param>
        /// <param name="databasePartitionName">Partition name in database</param>
        /// <param name="encryptionProvider">Provider used for encryption</param>
        public DataBackingStore(Data.Database database, string databasePartitionName, IStorageEncryptionProvider encryptionProvider)
        {
            this.database = database;
            this.partitionName = databasePartitionName;
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBackingStore"/> class
        /// with the specified configuration information.
        /// </summary>
        /// <param name="configurationView">A <see cref="CachingConfigurationView"></see> object</param>
        /// <exception cref="System.Configuration.ConfigurationException">Reflects any failures to read configuration information</exception>
        /// <remarks>Other exceptions thrown depend on the implementation of the underlying database.</remarks>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof (CachingConfigurationView));

            CachingConfigurationView cachingConfigurationView = (CachingConfigurationView) configurationView;
            DataCacheStorageData dataConfiguration = (DataCacheStorageData) cachingConfigurationView.GetCacheStorageDataForCacheManager(CurrentCacheManager);
            partitionName = dataConfiguration.PartitionName;
            DatabaseProviderFactory dataFactory = new DatabaseProviderFactory(cachingConfigurationView.ConfigurationContext);
            database = dataFactory.CreateDatabase(dataConfiguration.DatabaseInstanceName);
            if (dataConfiguration.StorageEncryption != null)
            {
                StorageEncryptionFactory encryptionFactory = new StorageEncryptionFactory(cachingConfigurationView.ConfigurationContext);
                encryptionProvider = encryptionFactory.CreateSymmetricProvider(CurrentCacheManager);
            }
        }

        /// <summary>
        /// Returns number of items stored in database
        /// </summary>
        public override int Count
        {
            get
            {
                DBCommandWrapper query = database.GetStoredProcCommandWrapper("GetItemCount");
                query.AddInParameter("@partitionName", DbType.String, partitionName);
                return (int) database.ExecuteScalar(query);
            }
        }

        /// <summary>
        /// Removes the item identified by the key from the database
        /// </summary>
        /// <param name="storageKey">Key of CacheItem to remove.</param>
        /// <remarks>Exceptions thrown depend on the implementation of the underlying database.</remarks>
        protected override void Remove(int storageKey)
        {
            DBCommandWrapper deleteCommand = database.GetStoredProcCommandWrapper("RemoveItem");
            deleteCommand.AddInParameter("@partitionName", DbType.String, partitionName);
            deleteCommand.AddInParameter("@storageKey", DbType.Int32, storageKey);

            database.ExecuteNonQuery(deleteCommand);
        }

        /// <summary>
        /// Updates the last accessed time for the CacheItem identified by the key
        /// </summary>
        /// <param name="storageKey">Key of item to update</param>
        /// <param name="lastAccessedTime">New timestamp for updated item</param>
        /// <remarks>Exceptions thrown depend on the implementation of the underlying database.</remarks>
        protected override void UpdateLastAccessedTime(int storageKey, DateTime lastAccessedTime)
        {
            DBCommandWrapper updateCommand = database.GetStoredProcCommandWrapper("UpdateLastAccessedTime");
            updateCommand.AddInParameter("@partitionName", DbType.String, partitionName);
            updateCommand.AddInParameter("@lastAccessedTime", DbType.DateTime, lastAccessedTime);
            updateCommand.AddInParameter("@storageKey", DbType.Int32, storageKey);

            database.ExecuteNonQuery(updateCommand);
        }

        /// <summary>
        /// Flushes all CacheItems from database. If an exception is thrown during the flush, the database is left unchanged.
        /// </summary>
        /// <remarks>Exceptions thrown depend on the implementation of the underlying database.</remarks>
        public override void Flush()
        {
            DBCommandWrapper flushCommand = database.GetStoredProcCommandWrapper("Flush");
            flushCommand.AddInParameter("@partitionName", DbType.String, partitionName);
            database.ExecuteNonQuery(flushCommand);
        }

        /// <summary>
        /// Loads data from persistence store.
        /// </summary>
        /// <returns>Unfiltered hashtable of cache items loaded from persistence store.</returns>
        protected override Hashtable LoadDataFromStore()
        {
            DBCommandWrapper loadDataCommand = database.GetStoredProcCommandWrapper("LoadItems");
            loadDataCommand.AddInParameter("@partitionName", DbType.String, partitionName);
            DataSet dataToLoad = database.ExecuteDataSet(loadDataCommand);

            Hashtable dataToReturn = new Hashtable();
            foreach (DataRow row in dataToLoad.Tables[0].Rows)
            {
                CacheItem cacheItem = CreateCacheItem(row);
                dataToReturn.Add(cacheItem.Key, cacheItem);
            }
            return dataToReturn;
        }

        private CacheItem CreateCacheItem(DataRow dataToLoad)
        {
            string key = (string) dataToLoad["Key"];
            object value = DeserializeValue(dataToLoad);

            CacheItemPriority scavengingPriority = (CacheItemPriority) dataToLoad["ScavengingPriority"];
            object refreshAction = DeserializeObject(dataToLoad, "RefreshAction");
            object expirations = SerializationUtility.ToObject((byte[]) dataToLoad["Expirations"]);
            object timestamp = (DateTime) dataToLoad["LastAccessedTime"];

            CacheItem cacheItem = new CacheItem((DateTime) timestamp, key, value, scavengingPriority, (ICacheItemRefreshAction) refreshAction, (ICacheItemExpiration[]) expirations);

            return cacheItem;
        }

        /// <summary>
        /// Removed existing item stored in persistence store with same key as new item
        /// </summary>
        /// <param name="storageKey">Item being removed from cache.</param>
        protected override void RemoveOldItem(int storageKey)
        {
            Remove(storageKey);
        }

        /// <summary>
        /// Adds new item to persistence store
        /// </summary>
        /// <param name="storageKey">Unique key for storage item.</param>
        /// <param name="newItem">Item to be added to cache. May not be null.</param>
        protected override void AddNewItem(int storageKey, CacheItem newItem)
        {
            string key = newItem.Key;
            byte[] valueBytes = SerializationUtility.ToBytes(newItem.Value);
            if (this.encryptionProvider != null)
            {
                valueBytes = this.encryptionProvider.Encrypt(valueBytes);
            }

            byte[] expirationBytes = SerializationUtility.ToBytes(newItem.Expirations);
            byte[] refreshActionBytes = SerializationUtility.ToBytes(newItem.RefreshAction);
            CacheItemPriority scavengingPriority = newItem.ScavengingPriority;
            DateTime lastAccessedTime = newItem.LastAccessedTime;

            DBCommandWrapper insertCommand = database.GetStoredProcCommandWrapper("AddItem");
            insertCommand.AddInParameter("@partitionName", DbType.String, partitionName);
            insertCommand.AddInParameter("@storageKey", DbType.Int32, storageKey);
            insertCommand.AddInParameter("@key", DbType.String, key);
            insertCommand.AddInParameter("@value", DbType.Binary, valueBytes);
            insertCommand.AddInParameter("@expirations", DbType.Binary, expirationBytes);
            insertCommand.AddInParameter("@refreshAction", DbType.Binary, refreshActionBytes);
            insertCommand.AddInParameter("@scavengingPriority", DbType.Int32, scavengingPriority);
            insertCommand.AddInParameter("@lastAccessedTime", DbType.DateTime, lastAccessedTime);

            database.ExecuteNonQuery(insertCommand);
        }

        private object DeserializeValue(DataRow dataToLoad)
        {
            object value = dataToLoad["Value"];

            if (value == DBNull.Value)
            {
                value = null;
            }
            else
            {
                byte[] valueBytes = (byte[]) value;
                if (this.encryptionProvider != null)
                {
                    valueBytes = this.encryptionProvider.Decrypt(valueBytes);
                }
                value = SerializationUtility.ToObject(valueBytes);
            }
            return value;
        }

        private object DeserializeObject(DataRow data, string columnName)
        {
            object byteArrayAsObject = data[columnName];
            if (byteArrayAsObject == DBNull.Value)
            {
                return null;
            }
            else
            {
                return SerializationUtility.ToObject((byte[]) byteArrayAsObject);
            }
        }
    }
}