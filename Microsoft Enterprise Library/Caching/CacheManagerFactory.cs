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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Factory for CacheManagers. This class is responsible for creating all the internal
    /// classes needed to implement a CacheManager.
    /// </summary>
    public class CacheManagerFactory : ConfigurationFactory
    {
        private BackingStoreFactory backingStoreFactory;
        private Hashtable cacheManagers = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CacheManagerFactory"/> class.</para>
        /// </summary>
        public CacheManagerFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// Creates a CacheManager from the given ConfigurationContext
        /// </summary>
        /// <param name="configurationContext">Configuration information to use when creating CacheManagers. May not be null.</param>
        public CacheManagerFactory(ConfigurationContext configurationContext) : base(SR.CacheManagerFactoryName, configurationContext)
        {
            this.backingStoreFactory = new BackingStoreFactory(configurationContext);
        }

        /// <summary>
        /// Gets the default CacheManager as defined in the currently used context
        /// </summary>
        /// <returns>Default CacheManager</returns>
        public CacheManager GetCacheManager()
        {
            return (CacheManager)CreateInstance(GetDefaultInstanceName());
        }

        /// <summary>
        /// Gets the CacheManager specified by the given name
        /// </summary>
        /// <param name="cacheManagerName">CacheManager to return</param>
        /// <returns>Specified CacheManager instance. Note that calling this method multiple times
        /// for the same named CacheManager will return the same object each time.</returns>
        public CacheManager GetCacheManager(string cacheManagerName)
        {
            return (CacheManager)CreateInstance(cacheManagerName);
        }

        /// <summary>
        /// <para>Construct an instance of a named configuration <see cref="CacheManager"/> by the <paramref name="type"/>.</para>
        /// </summary>
        /// <param name="cacheManagerName">
        /// <para>The name from configuration of the <see cref="CacheManager"/> to create.</para>
        /// </param>
        /// <param name="type"><para>The <see cref="Type"/> to create.</para></param>
        /// <returns><para>A <see cref="CacheManager"/> object.</para>.</returns>
        protected override object CreateObject(string cacheManagerName, Type type)
        {
            return CreateCacheManager(cacheManagerName);
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> of the <see cref="CacheManager"/> for the factory to create.</para>
        /// </summary>
        /// <param name="cacheManagerName">
        /// <para>The name from configuration of the <see cref="CacheManager"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="CacheManager"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string cacheManagerName)
        {
        	CachingConfigurationView view = new CachingConfigurationView(ConfigurationContext);
            CacheManagerData cacheManagerData = view.GetCacheManagerData(cacheManagerName);
            return GetType(cacheManagerData.TypeName);
        }

        /// <summary>
        /// Method protected for testing purposes. Do not call this method
        /// </summary>
        /// <param name="cacheOperations">For testing only.</param>
        /// <returns>For testing only.</returns>
        internal virtual ExpirationTask CreateExpirationTask(ICacheOperations cacheOperations)
        {
            return new ExpirationTask(cacheOperations);
        }

        private CacheManager CreateCacheManager(string cacheManagerName)
        {
            CacheManager cacheManager = cacheManagers[cacheManagerName] as CacheManager;
            if (cacheManager != null)
            {
                return cacheManager;
            }

        	CachingConfigurationView view = new CachingConfigurationView(ConfigurationContext);
            CacheManagerData cacheManagerData = view.GetCacheManagerData(cacheManagerName);
            CacheCapacityScavengingPolicy scavengingPolicy =
                new CacheCapacityScavengingPolicy(cacheManagerName, view);

            IBackingStore backingStore = backingStoreFactory.CreateBackingStore(cacheManagerName);
            Cache cache = new Cache(backingStore, scavengingPolicy);

            ExpirationPollTimer timer = new ExpirationPollTimer();
            ExpirationTask expirationTask = CreateExpirationTask(cache);
            ScavengerTask scavengerTask = new ScavengerTask(cacheManagerName, view, scavengingPolicy, cache);
            BackgroundScheduler scheduler = new BackgroundScheduler(expirationTask, scavengerTask);
            cache.Initialize(scheduler);

            scheduler.Start();
            timer.StartPolling(new TimerCallback(scheduler.ExpirationTimeoutExpired), cacheManagerData.ExpirationPollFrequencyInSeconds * 1000);

            cacheManager = new CacheManager(cache, scheduler, timer);
            cacheManagers.Add(cacheManagerName, cacheManager);
            return cacheManager;
        }

        private string GetDefaultInstanceName()
        {
        	CachingConfigurationView view = new CachingConfigurationView(ConfigurationContext);
            return view.GetDefaultCacheManager();
        }
    }
}