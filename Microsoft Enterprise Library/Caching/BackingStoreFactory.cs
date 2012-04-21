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
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Represents a factory for creating <see cref="IBackingStore"/> object
    /// from the configuration data in a specified <see cref="ConfigurationContext"/>.
    /// </summary>
    public class BackingStoreFactory : ProviderFactory
    {
        internal const string CacheManagerName = "cacheManagerName";

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="BackingStoreFactory"/> class.</para>
        /// </summary>
        public BackingStoreFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="BackingStoreFactory"/> class.
        /// </summary>
        /// <param name="context">Current configuration context</param>
        public BackingStoreFactory(ConfigurationContext context) : base(SR.BackingStoreFactoryName, context, typeof(IBackingStore))
        {
        }

        /// <summary>
        /// Creates an <see cref="IBackingStore"/> from the configuration
        /// data associated with the specified Cache Manager.
        /// </summary>
        /// <param name="cacheManagerName">The name of the cache manager that is creating the <see cref="IBackingStore"/>.</param>
        /// <returns>An <see cref="IBackingStore"/>.</returns>
        public IBackingStore CreateBackingStore(string cacheManagerName)
        {
            CallContext.SetData(CacheManagerName, cacheManagerName);
            return (IBackingStore)CreateInstance(cacheManagerName);
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> of the <see cref="IBackingStore"/> for the factory to create for a <see cref="CacheManager"/>.</para>
        /// </summary>
        /// <param name="cacheManagerName">
        /// <para>The name of the cache manager to create the <see cref="IBackingStore"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="IBackingStore"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string cacheManagerName)
        {
            CacheStorageData cacheStorageData = GetCacheStorageData(cacheManagerName);
            return GetType(cacheStorageData.TypeName);
        }


        /// <summary>
        /// <para>Creates the <see cref="CachingConfigurationView"/> for the factory.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="CachingConfigurationView"/> for the factory.</para>
        /// </returns>
        protected override ConfigurationView CreateConfigurationView()
        {
            return new CachingConfigurationView(base.ConfigurationContext);
        }

        /// <summary>
        /// <para>Initialize the <see cref="IConfigurationProvider"/> by invoking the <see cref="IConfigurationProvider.Initialize"/> method.</para>
        /// </summary>
        /// <param name="cacheManagerName">
        /// <para>The name of the <see cref="CacheManager"/>.</para>
        /// </param>
        /// <param name="provider">
        /// <para>The <see cref="IConfigurationProvider"/> to initialize.</para>
        /// </param>
        protected override void InitializeConfigurationProvider(string cacheManagerName, IConfigurationProvider provider)
        {
            CacheStorageData cacheStorageData = GetCacheStorageData(cacheManagerName);
            provider.ConfigurationName = cacheStorageData.Name;
            ((IBackingStore)provider).CurrentCacheManager = cacheManagerName;
            base.InitializeConfigurationProvider(cacheManagerName, provider);
        }

        private CacheStorageData GetCacheStorageData(string cacheManagerName)
        {
        	CachingConfigurationView view = (CachingConfigurationView)CreateConfigurationView();
            return view.GetCacheStorageDataForCacheManager(cacheManagerName);
        }
    }
}