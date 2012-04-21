using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
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

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Static factory class used to get instances of a specified CacheManager
    /// </summary>
    public sealed class CacheFactory
    {
        private static CacheManagerFactory factory = new CacheManagerFactory(ConfigurationManager.GetCurrentContext());
        private static object lockObject = new object();

        private CacheFactory()
        {
        }

        /// <summary>
        /// Returns the default CacheManager instance. The same instance should be returned each time this method
        /// is called. The name of the instance to treat as the default CacheManager is defined in the configuration file.
        /// Guaranteed to return an intialized CacheManager if no exception thrown
        /// </summary>
        /// <returns>Default cache manager instance.</returns>
        /// <exception cref="ConfigurationException">Unable to create default CacheManager</exception>
        public static CacheManager GetCacheManager()
        {
            lock (lockObject)
            {
                return factory.GetCacheManager();
            }
        }

        /// <summary>
        /// Returns the named CacheManager instance. Guaranteed to return an initialized CacheManager if no exception thrown.
        /// </summary>
        /// <param name="cacheManagerName">Name defined in configuration for the cache manager to instantiate</param>
        /// <returns>The requested CacheManager instance.</returns>
        /// <exception cref="ArgumentNullException">cacheManagerName is null</exception>
        /// <exception cref="ArgumentException">cacheManagerName is empty</exception>
        /// <exception cref="ConfigurationException">Could not find instance specified in cacheManagerName</exception>
        /// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
        public static CacheManager GetCacheManager(string cacheManagerName)
        {
            lock (lockObject)
            {
                return factory.GetCacheManager(cacheManagerName);
            }
        }
    }
}