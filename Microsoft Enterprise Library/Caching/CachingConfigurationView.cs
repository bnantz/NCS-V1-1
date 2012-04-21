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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// <para>Represents a view to navigate the <see cref="CacheManagerSettings"/> configuration data.</para>
    /// </summary>
    public class CachingConfigurationView : ConfigurationView
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CachingConfigurationView"/> with a <see cref="ConfigurationContext"/>.</para>
        /// </summary>
        /// <param name="configurationContext">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        public CachingConfigurationView(ConfigurationContext configurationContext) : base(configurationContext)
        {
        }

        /// <summary>
        /// <para>Gets the <see cref="CacheStorageData"/> from configuration for the named <see cref="CacheManager"/></para>
        /// </summary>
        /// <param name="cacheManagerName">
        /// <para>The name of the <see cref="CacheManager"/>.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="CacheStorageData"/> object.</para>
        /// </returns>
        public virtual CacheStorageData GetCacheStorageDataForCacheManager(string cacheManagerName)
        {
            CacheManagerData cacheManagerData = GetCacheManagerData(cacheManagerName);
            CacheStorageData cacheStorageData = cacheManagerData.CacheStorage;
            if (cacheStorageData == null)
            {
                throw new ConfigurationException(SR.CannotInstantiateCacheStorage(cacheManagerName));
            }
            return cacheStorageData;
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="CacheManagerData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="CacheManagerData"/>.</para>
        /// </returns>
        public virtual string GetDefaultCacheManager()
        {
            CacheManagerSettings configSettings = GetCacheManagerSettings();
            string defaultCacheManagerName = configSettings.DefaultCacheManager;
            if (defaultCacheManagerName == null || defaultCacheManagerName.Length == 0)
            {
                throw new ConfigurationException(SR.ExceptionMessageNoDefault);
            }
            return defaultCacheManagerName;
        }

        /// <summary>
        /// <para>Gets the <see cref="CacheManagerSettings"/> configuration data.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="CacheManagerSettings"/> configuration data.</para>
        /// </returns>
        public virtual CacheManagerSettings GetCacheManagerSettings()
        {
            return (CacheManagerSettings)ConfigurationContext.GetConfiguration(CacheManagerSettings.SectionName);
        }

        /// <summary>
        /// <para>Gets the <see cref="CacheManagerData"/> from configuration for the named <see cref="CacheManager"/></para>
        /// </summary>
        /// <param name="cacheManagerName">
        /// <para>The name of the <see cref="CacheManager"/>.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="CacheManagerData"/> object.</para>
        /// </returns>
        public virtual CacheManagerData GetCacheManagerData(string cacheManagerName)
        {
            CacheManagerSettings configSettings = GetCacheManagerSettings();
            CacheManagerData data = configSettings.CacheManagers[cacheManagerName];
            if (data == null)
            {
                throw new ConfigurationException(SR.UnableToFindCacheManagerInstance(cacheManagerName));
            }
            return data;
        }

        /// <summary>
        /// <para>Gets the <see cref="StorageEncryptionProviderData"/> from configuration for the named <see cref="CacheManager"/></para>
        /// </summary>
        /// <param name="cacheManagerName">
        /// <para>The name of the <see cref="CacheManager"/>.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="StorageEncryptionProviderData"/> object.</para>
        /// </returns>
        public virtual StorageEncryptionProviderData GetStorageEncryptionProviderData(string cacheManagerName)
        {
            CacheStorageData data = GetCacheStorageDataForCacheManager(cacheManagerName);
            return data.StorageEncryption;
        }
    }
}