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
    /// Allows for custom <c>CacheStorageData</c> configuration.
    /// </summary>
    [XmlRoot("cacheStorage", Namespace=CacheManagerSettings.ConfigurationNamespace)]
    public class CustomCacheStorageData : CacheStorageData
    {
        private NameValueItemCollection extensions;
        private string typeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCacheStorageData"/> class.
        /// </summary>
        public CustomCacheStorageData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomCacheStorageData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CustomCacheStorageData"/>.
        /// </param>
        public CustomCacheStorageData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomCacheStorageData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CustomCacheStorageData"/>.
        /// </param>
        /// <param name="typeName">
        /// Gets the type for the <see cref="CustomCacheStorageData"/>.
        /// </param>
        public CustomCacheStorageData(string name, string typeName) : this(name, typeName, null)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CacheManagerData"/>.
        /// </param>
        /// <param name="typeName">
        /// Gets the type for the <see cref="CustomCacheStorageData"/>.
        /// </param>
        /// <param name="storageEncryption">
        /// Storage Encryption data defined in configuration
        /// </param>
        public CustomCacheStorageData(string name, string typeName, StorageEncryptionProviderData storageEncryption) : base(name, storageEncryption)
        {
            this.typeName = typeName;
            this.extensions = new NameValueItemCollection();
        }

        /// <summary>
        /// Retrieves custom configuration attributes
        /// </summary>
        [XmlArray(ElementName="extensions")]
        [XmlArrayItem(ElementName="extension", Type=typeof(NameValueItem))]
        public NameValueItemCollection Extensions
        {
            get { return this.extensions; }
        }

        /// <summary>
        /// Gets the type for the <see cref="CustomCacheStorageData"/>.
        /// </summary>
        [XmlAttribute("type")]
        public override string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
    }
}