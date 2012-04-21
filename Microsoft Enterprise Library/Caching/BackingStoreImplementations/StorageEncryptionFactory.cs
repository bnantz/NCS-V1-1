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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
    /// <summary>
    /// Not intended for direct use.  Creates cache backing store encryption providers.
    /// </summary>
    public class StorageEncryptionFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the of the <see cref="StorageEncryptionFactory"/> class.</para>
        /// </summary>
        public StorageEncryptionFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// Initializes the factory with a specified configuration context.
        /// </summary>
        /// <param name="configurationContext">The configuration context.</param>
        public StorageEncryptionFactory(ConfigurationContext configurationContext) : base(SR.StorageEncryptionFactoryName, configurationContext, typeof(IStorageEncryptionProvider))
        {
        }

        /// <summary>
        /// Returns the encryption provider specified in configuration
        /// </summary>
        /// <param name="cacheManagerName">Name of cache manager as defined in configuration.</param>
        /// <returns>Encryption provider as defined in configuration</returns>
        public IStorageEncryptionProvider CreateSymmetricProvider(string cacheManagerName)
        {
            return (IStorageEncryptionProvider)base.CreateInstance(cacheManagerName);
        }

        /// <summary>
        /// <para>Creates the <see cref="CachingConfigurationView"/> for the factory.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="CachingConfigurationView"/> for the factory.</para>
        /// </returns>
        protected override ConfigurationView CreateConfigurationView()
        {
            return new CachingConfigurationView(ConfigurationContext);
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> of the <see cref="IStorageEncryptionProvider"/> for the factory to create for a <see cref="CacheManager"/>.</para>
        /// </summary>
        /// <param name="cacheManagerName">
        /// <para>The name of the cache manager to create the <see cref="IStorageEncryptionProvider"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="IStorageEncryptionProvider"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string cacheManagerName)
        {
        	CachingConfigurationView view = (CachingConfigurationView)CreateConfigurationView();
            StorageEncryptionProviderData data = view.GetStorageEncryptionProviderData(cacheManagerName);
            return GetType(data.TypeName);
        }
    }
}