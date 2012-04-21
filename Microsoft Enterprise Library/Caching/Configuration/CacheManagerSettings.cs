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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Overall configuration settings for Caching
    /// </summary>
    [XmlRoot("enterpriseLibrary.cacheSettings", Namespace=CacheManagerSettings.ConfigurationNamespace)]
    public class CacheManagerSettings
    {
        private string defaultCacheManager;
        private CacheManagerDataCollection cacheManagers = new CacheManagerDataCollection();

        /// <summary>
        /// <para>Gets the Xml namespace for this root node.</para>
        /// </summary>
        /// <value>
        /// <para>The Xml namespace for this root node.</para>
        /// </value>
        public const string ConfigurationNamespace = "http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/cache";

        /// <summary>
        /// Configuration key for cache manager settings.
        /// </summary>
        public const string SectionName = "cachingConfiguration";

        /// <summary>
        /// Defines the default manager instance to use when no other manager is specified
        /// </summary>
        [XmlAttribute("defaultCacheManager")]
        public string DefaultCacheManager
        {
            get { return defaultCacheManager; }
            set { defaultCacheManager = value; }
        }

        /// <summary>
        /// Defines the collection of CacheManagerData instances
        /// </summary>
        [XmlArray(ElementName="cacheManagers")]
        [XmlArrayItem(ElementName="cacheManager", Type=typeof(CacheManagerData))]

        public CacheManagerDataCollection CacheManagers
        {
            get { return cacheManagers; }
        }
    }
}