//===============================================================================
// Microsoft patterns & practices Enterprise Library
// XXXXX Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    public class TestCachingConfigurationView : CachingConfigurationView
    {
        public CacheStorageData data;

        public TestCachingConfigurationView(CacheStorageData data, ConfigurationContext context) : base(context)
        {
            this.data = data;
        }

        public override CacheStorageData GetCacheStorageDataForCacheManager(string cacheManagerName)
        {
            return data;
        }
    }
}

#endif