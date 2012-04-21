//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration
{
    /// <summary>
    /// Configuration data for the Security Cache.
    /// </summary>
    [XmlRoot("cachingStoreProviderData", Namespace=SecuritySettings.ConfigurationNamespace)]
    public class CachingStoreProviderData : SecurityCacheProviderData
    {
        private string cacheManager;
        private int slidingExpiration;
        private int absoluteExpiration;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CachingStoreProviderData"/> class.</para>
        /// </summary>
        public CachingStoreProviderData()
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CachingStoreProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the provider.</para>
        /// </param>
        public CachingStoreProviderData(string name) : this(name, 0, 0)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CachingStoreProviderData"/> class with a name, sliding expiration and absolute expiration.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the provider.</para>
        /// </param>
        /// <param name="slidingExpiration">
        /// <para>The sliding expiration value.</para>
        /// </param>
        /// <param name="absoluteExpiration">
        /// <para>The absolute experation value.</para>
        /// </param>
        public CachingStoreProviderData(string name, int slidingExpiration, int absoluteExpiration) : base(name)
        {
            this.slidingExpiration = slidingExpiration;
            this.absoluteExpiration = absoluteExpiration;
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CachingStoreProviderData"/> class with a name, sliding expiration, absolute expiration and <see cref="CacheManager"/> name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the provider.</para>
        /// </param>
        /// <param name="slidingExpiration">
        /// <para>The sliding expiration value.</para>
        /// </param>
        /// <param name="absoluteExpiration">
        /// <para>The absolute experation value.</para>
        /// </param>
        /// <param name="cacheManager">
        /// <para>The name of the <see cref="CacheManager"/> to cache the information.</para>
        /// </param>
        public CachingStoreProviderData(string name, int slidingExpiration, int absoluteExpiration, string cacheManager) : base(name)
        {
            this.cacheManager = cacheManager;
            this.slidingExpiration = slidingExpiration;
            this.absoluteExpiration = absoluteExpiration;
        }

        /// <summary>
        /// Gets or sets the Caching Block Cache instance name.
        /// </summary>
        /// <value>Caching Block Cache Instance Name.</value>
        [XmlAttribute(AttributeName = "cacheManagerInstanceName")]
        public string CacheManager
        {
            get { return this.cacheManager; }
            set { this.cacheManager = value; }
        }

        /// <summary>
        /// Gets or sets the Sliding Session Expiration duration.
        /// </summary>
        /// <value>Sliding Session Expiration duration</value>
        [XmlAttribute(AttributeName = "defaultSlidingSessionExpirationInMinutes")]
        public int SlidingExpiration
        {
            get { return this.slidingExpiration; }
            set { this.slidingExpiration = value; }
        }

        /// <summary>
        /// Gets or sets the Absolute Session Expiration duration.
        /// </summary>
        /// <value>Absolute Session Expiration duration</value>
        [XmlAttribute(AttributeName = "defaultAbsoluteSessionExpirationInMinutes")]
        public int AbsoluteExpiration
        {
            get { return this.absoluteExpiration; }
            set { this.absoluteExpiration = value; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="System.Type"/> name of the provider.</para>
        /// </summary>
        /// <value>
        /// <para>The type name of the provider. The default is an empty string.</para>
        /// </value>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(CachingStoreProvider).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}