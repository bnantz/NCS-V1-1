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
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Configuration data defining IsolatedStorageCacheStorageData. This configuration section adds the name
    /// of the Isolated Storage area to use to store data.
    /// </summary>
    [XmlRoot("cacheStorage", Namespace=CacheManagerSettings.ConfigurationNamespace)]
    public class IsolatedStorageCacheStorageData : CacheStorageData
    {
        private string partitionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageCacheStorageData"/> class.
        /// </summary>
        public IsolatedStorageCacheStorageData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="IsolatedStorageCacheStorageData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="IsolatedStorageCacheStorageData"/>.
        /// </param>
        public IsolatedStorageCacheStorageData(string name) : this(name, null, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="IsolatedStorageCacheStorageData"/>.
        /// </param>
        /// <param name="storageEncryption">
        /// Storage Encryption data defined in configuration
        /// </param>
        /// <param name="partitionName">
        /// Name of the Isolated Storage area to use.
        /// </param>
        public IsolatedStorageCacheStorageData(string name, StorageEncryptionProviderData storageEncryption, string partitionName) : base(name, storageEncryption)
        {
            this.partitionName = partitionName;
        }

        /// <summary>
        /// Name of the Isolated Storage area to use.
        /// </summary>
        [XmlAttribute("partitionName")]
        public string PartitionName
        {
            get { return partitionName; }
            set { partitionName = value; }
        }

        /// <summary>
        /// Gets the IsolatedStorageBackingStore type.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(IsolatedStorageBackingStore).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}