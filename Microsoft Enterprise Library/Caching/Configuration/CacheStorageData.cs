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

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Configuration data defining CacheStorageData. This configuration section defines the name and type
    /// of the IBackingStore used by a CacheManager
    /// </summary>
    [XmlInclude(typeof(CustomCacheStorageData))]
    [XmlInclude(typeof(IsolatedStorageCacheStorageData))]
    [XmlRoot("cacheStorage", Namespace=CacheManagerSettings.ConfigurationNamespace)]
    public abstract class CacheStorageData : ProviderData
    {
        private StorageEncryptionProviderData storageEncryption;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheStorageData"/> class.
        /// </summary>
        protected CacheStorageData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CacheManagerData"/>.
        /// </param>
        protected CacheStorageData(string name) : this(name, null)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CacheManagerData"/>.
        /// </param>
        /// <param name="storageEncryption">
        /// Storage Encryption data defined in configuration
        /// </param>
        protected CacheStorageData(string name, StorageEncryptionProviderData storageEncryption) : base(name)
        {
            this.storageEncryption = storageEncryption;
        }

        /// <summary>
        /// Storage Encryption data defined in configuration
        /// </summary>
        [XmlElement("storageEncryption")]
        public StorageEncryptionProviderData StorageEncryption
        {
            get { return storageEncryption; }
            set { storageEncryption = value; }
        }
    }
}