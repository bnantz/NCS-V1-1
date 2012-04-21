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
    /// Configuration data defining CacheManagerData. Defines the information needed to properly configure
    /// a CacheManager instance.
    /// </summary>
    [XmlRoot("cacheManager", Namespace=CacheManagerSettings.ConfigurationNamespace)]
    public class CacheManagerData : ProviderData
    {
        private int expirationPollFrequencyInSeconds;
        private int maximumElementsInCacheBeforeScavenging;
        private int numberToRemoveWhenScavenging;
        private CacheStorageData cacheStorage;

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerData"/> class.
        /// </summary>
        public CacheManagerData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CacheManagerData"/>.
        /// </param>
        public CacheManagerData(string name) : this(name, 0, 0, 0)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CacheManagerData"/>.
        /// </param>
        /// <param name="expirationPollFrequencyInSeconds">
        /// Frequency in seconds of expiration polling cycle
        /// </param>
        /// <param name="maximumElementsInCacheBeforeScavenging">
        /// Maximum number of items in cache before an add causes scavenging to take place
        /// </param>
        /// <param name="numberToRemoveWhenScavenging">
        /// Number of items to remove from cache when scavenging
        /// </param>
        public CacheManagerData(string name, int expirationPollFrequencyInSeconds, int maximumElementsInCacheBeforeScavenging, int numberToRemoveWhenScavenging) : this(name, 0, 0, 0, null)
        {
            this.expirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
            this.maximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
            this.numberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CacheManagerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="CacheManagerData"/>.
        /// </param>
        /// <param name="expirationPollFrequencyInSeconds">
        /// Frequency in seconds of expiration polling cycle
        /// </param>
        /// <param name="maximumElementsInCacheBeforeScavenging">
        /// Maximum number of items in cache before an add causes scavenging to take place
        /// </param>
        /// <param name="numberToRemoveWhenScavenging">
        /// Number of items to remove from cache when scavenging
        /// </param>
        /// <param name="cacheStorage">
        /// CacheStorageData object from configuration describing how data is stored 
        /// in the cache.
        /// </param>
        public CacheManagerData(string name, int expirationPollFrequencyInSeconds, int maximumElementsInCacheBeforeScavenging, int numberToRemoveWhenScavenging, CacheStorageData cacheStorage) : base(name)
        {
            this.expirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
            this.maximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
            this.numberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
            this.cacheStorage = cacheStorage;
        }

        /// <summary>
        /// Overriden. Always returns the type name of the <see cref="CacheManager"/> class.
        /// This property is ignored by the XmlSerializer.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(CacheManager).AssemblyQualifiedName; }
            set
            {
            }
        }

        /// <summary>
        /// Frequency in seconds of expiration polling cycle
        /// </summary>
        [XmlAttribute("expirationPollFrequencyInSeconds")]
        public int ExpirationPollFrequencyInSeconds
        {
            get { return expirationPollFrequencyInSeconds; }
            set { expirationPollFrequencyInSeconds = value; }
        }

        /// <summary>
        /// Maximum number of items in cache before an add causes scavenging to take place
        /// </summary>
        [XmlAttribute("maximumElementsInCacheBeforeScavenging")]
        public int MaximumElementsInCacheBeforeScavenging
        {
            get { return maximumElementsInCacheBeforeScavenging; }
            set { maximumElementsInCacheBeforeScavenging = value; }
        }

        /// <summary>
        /// Number of items to remove from cache when scavenging
        /// </summary>
        [XmlAttribute("numberToRemoveWhenScavenging")]
        public int NumberToRemoveWhenScavenging
        {
            get { return numberToRemoveWhenScavenging; }
            set { numberToRemoveWhenScavenging = value; }
        }

        /// <summary>
        /// CacheStorageData object from configuration describing how data is stored 
        /// in the cache.
        /// </summary>
        [XmlElement("cacheStorage")]
        public CacheStorageData CacheStorage
        {
            get { return cacheStorage; }
            set { cacheStorage = value; }
        }
    }
}