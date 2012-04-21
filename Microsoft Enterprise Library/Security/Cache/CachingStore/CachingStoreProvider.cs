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

using System;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore
{
    /// <summary>
    /// Class that caches the athenticated session information and
    /// performs token handling.
    /// </summary>
    public class CachingStoreProvider : ConfigurationProvider, ISecurityCacheProvider
    {
        private SecurityConfigurationView securityConfigurationView;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CachingStoreProvider"/> class.</para>
        /// </summary>
        public CachingStoreProvider()
        {
        }

        /// <summary>
        /// <para>Initializes the provider with a <see cref="SecurityConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="SecurityConfigurationView"/> object.</para>
        /// </param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(SecurityConfigurationView));

            this.securityConfigurationView = (SecurityConfigurationView)configurationView;
        }

        /// <summary>
        /// Caches an authenticated IIdentity object.
        /// </summary>
        /// <param name="identity">
        /// IIdentity object representing an authenticated user.
        /// </param>
        /// <returns>Token to identify the cached Identity</returns>
        public IToken SaveIdentity(IIdentity identity)
        {
            IToken guidToken = new GuidToken();

            SaveIdentity(identity, guidToken);

            return guidToken;
        }

        /// <summary>
        /// Caches an authenticated IPrincipal object using an existing token
        /// enabling the grouping of related items.
        /// </summary>
        /// <param name="identity">
        /// IIdentity object representing an authenticated user.
        /// </param>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        public void SaveIdentity(IIdentity identity, IToken token)
        {
            GetSecurityCacheItem(token, true).Identity = identity;
        }

        /// <summary>
        /// Caches an authenticated IPrincipal object.
        /// </summary>
        /// <param name="principal">
        /// IPrincipal object representing an authenticated user and roles.
        /// </param>
        /// <returns>Token to identify the cached IPrincipal object</returns>
        public IToken SavePrincipal(IPrincipal principal)
        {
            IToken guidToken = new GuidToken();

            SavePrincipal(principal, guidToken);

            return guidToken;
        }

        /// <summary>
        /// Caches an authenticated IPrincipal object using an existing token
        /// enabling the grouping of related items.
        /// </summary>
        /// <param name="principal">
        /// IPrincipal object representing an authenticated user.
        /// </param>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        public void SavePrincipal(IPrincipal principal, IToken token)
        {
            GetSecurityCacheItem(token, true).Principal = principal;
        }

        /// <summary>
        /// Caches a profile for an authenticated user.
        /// </summary>
        /// <param name="profile">
        /// Object representing the profile of authenticated user.
        /// </param>
        /// <returns>Token to identify the cached Profile object</returns>
        public IToken SaveProfile(object profile)
        {
            GuidToken guidToken = new GuidToken();

            SaveProfile(profile, guidToken);

            return guidToken;
        }

        /// <summary>
        /// Caches a profile for an authenticated user using an existing token
        /// (enabling the grouping of related items).
        /// </summary>
        /// <param name="profile">
        /// Object representing the profile of authenticated user.
        /// </param>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        public void SaveProfile(object profile, IToken token)
        {
            GetSecurityCacheItem(token, true).Profile = profile;
        }

        /// <summary>
        /// Purges an existing IIdentity object from the cache.
        /// </summary>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        public void ExpireIdentity(IToken token)
        {
            ExpireSecurityCacheItem(token, SecurityEntityType.Identity);
        }

        /// <summary>
        /// Purges an existing IPrincipal object from the cache.
        /// </summary>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        public void ExpirePrincipal(IToken token)
        {
            ExpireSecurityCacheItem(token, SecurityEntityType.Principal);
        }

        /// <summary>
        /// Purges an existing Profile object from the cache.
        /// </summary>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        public void ExpireProfile(IToken token)
        {
            ExpireSecurityCacheItem(token, SecurityEntityType.Profile);
        }

        /// <summary>
        /// Gets an existing IIdentity object from the cache.
        /// </summary>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        /// <returns>A cached IIdentity object</returns>
        public IIdentity GetIdentity(IToken token)
        {
            SecurityCacheItem item = GetSecurityCacheItem(token);
            IIdentity identity = item == null ? null : item.Identity;
            SecurityCacheReadEvent.Fire(identity != null);

            return identity;
        }

        /// <summary>
        /// Gets an existing IPrincipal object from the cache.
        /// </summary>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        /// <returns>A cached IPrincipal object</returns>
        public IPrincipal GetPrincipal(IToken token)
        {
            SecurityCacheItem item = GetSecurityCacheItem(token);
            IPrincipal principal = item == null ? null : item.Principal;
            SecurityCacheReadEvent.Fire(principal != null);

            return principal;
        }

        /// <summary>
        /// Gets an existing Profile object from the cache.
        /// </summary>
        /// <param name="token">
        /// Token identifying an existing cached entity.
        /// </param>
        /// <returns>A cached Profile object</returns>
        public object GetProfile(IToken token)
        {
            SecurityCacheItem item = GetSecurityCacheItem(token);
            object profile = item == null ? null : item.Profile;
            SecurityCacheReadEvent.Fire(profile != null);

            return profile;
        }

        private void ExpireSecurityCacheItem(IToken token, SecurityEntityType entityType)
        {
            CacheManager securityCacheManager = GetCacheManagerInstance();
            SecurityCacheItem item = GetSecurityCacheItem(token);
            if (item != null)
            {
                ClearCachItemEntity(item, entityType);

                if (item.IsRemoveable)
                {
                    securityCacheManager.Remove(token.Value);
                }
            }
        }

        private void ClearCachItemEntity(SecurityCacheItem item, SecurityEntityType entityType)
        {
            switch (entityType)
            {
                case SecurityEntityType.Identity:
                    item.Identity = null;
                    break;
                case SecurityEntityType.Principal:
                    item.Principal = null;
                    break;
                case SecurityEntityType.Profile:
                    item.Profile = null;
                    break;
            }
        }

        private SecurityCacheItem GetSecurityCacheItem(IToken token)
        {
            return GetSecurityCacheItem(token, false);
        }

        private SecurityCacheItem GetSecurityCacheItem(IToken token, bool createIfNull)
        {
            CacheManager securityCacheManager = GetCacheManagerInstance();
            SecurityCacheItem item = null;
            item = securityCacheManager.GetData(token.Value) as SecurityCacheItem;

            if (item == null && createIfNull)
            {
                item = new SecurityCacheItem();
                securityCacheManager.Add(token.Value, item, CacheItemPriority.Normal, null, GetCacheExpirations());
            }

            return item;
        }

        private ICacheItemExpiration[] GetCacheExpirations()
        {
            CachingStoreProviderData cacheStorageProviderData = GetCacheStorageProviderData();
            ICacheItemExpiration[] cachingExpirations = new ICacheItemExpiration[2];
            cachingExpirations[0] = new AbsoluteTime(new TimeSpan(0, 0, ConvertExpirationTimeToSeconds(cacheStorageProviderData.AbsoluteExpiration)));
            cachingExpirations[1] = new SlidingTime(new TimeSpan(0, 0, ConvertExpirationTimeToSeconds(cacheStorageProviderData.SlidingExpiration)));
            return cachingExpirations;
        }

        private int ConvertExpirationTimeToSeconds(int expirationInMinutes)
        {
            return expirationInMinutes * 60;
        }

        private CacheManager GetCacheManagerInstance()
        {
            CachingStoreProviderData cachingStoreProviderData = GetCacheStorageProviderData();

            string cacheManagerInstance = (cachingStoreProviderData.CacheManager.Length > 0) ? cachingStoreProviderData.CacheManager : null;

            return CacheFactory.GetCacheManager(cacheManagerInstance);
        }

        private CachingStoreProviderData GetCacheStorageProviderData()
        {
            SecurityCacheProviderData securityCacheProviderData = securityConfigurationView.GetSecurityCacheProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(securityCacheProviderData, typeof(CachingStoreProviderData));

            return (CachingStoreProviderData)securityCacheProviderData;
        }
    }
}