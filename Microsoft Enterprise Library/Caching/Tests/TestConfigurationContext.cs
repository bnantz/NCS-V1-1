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

#if UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    public class TestConfigurationContext : ConfigurationContext
    {
        public TestConfigurationContext()
            : base( GenerateConfigurationDictionary())
        {
        }

        private static ConfigurationDictionary GenerateConfigurationDictionary()
        {
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            dictionary = new ConfigurationDictionary();
            dictionary.Add(ConfigurationSettings.SectionName, GenerateConfigurationSettings());
            dictionary.Add(CacheManagerSettings.SectionName, GenerateCachingSettings());

            return dictionary;
        }

        private static ConfigurationSettings GenerateConfigurationSettings()
        {
            ConfigurationSettings settings = new ConfigurationSettings();
            settings.ConfigurationSections.Add(new ConfigurationSectionData(CacheManagerSettings.SectionName, false, new XmlFileStorageProviderData("XmlStorage", "EnterpriseLibrary.Caching.config"), new XmlSerializerTransformerData("DataBuilder")));
            return settings;
        }

        private static CacheManagerSettings GenerateCachingSettings()
        {
            CacheManagerSettings settings = new CacheManagerSettings();

            settings.CacheManagers.Add(new CacheManagerData("test", 10, 10, 1, new CustomCacheStorageData()));
            return settings;
        }
    }
}
#endif

