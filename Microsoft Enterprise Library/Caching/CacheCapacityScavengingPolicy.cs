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
    internal class CacheCapacityScavengingPolicy
    {
        private readonly CachingConfigurationView latestConfigurationData;
        private readonly string cacheManagerName;

        public CacheCapacityScavengingPolicy(string cacheManagerName, CachingConfigurationView latestConfigurationData)
        {
            this.latestConfigurationData = latestConfigurationData;
            this.cacheManagerName = cacheManagerName;
        }

        public int MaximumItemsAllowedBeforeScavenging
        {
            get { return latestConfigurationData.GetCacheManagerData(cacheManagerName).MaximumElementsInCacheBeforeScavenging; }
        }

        public bool IsScavengingNeeded(int currentCacheItemCount)
        {
            return currentCacheItemCount > MaximumItemsAllowedBeforeScavenging;
        }
    }
}