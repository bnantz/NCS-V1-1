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

#if  UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests
{
	public class TestConfigurationContext : ConfigurationContext
	{
		private static ConfigurationDictionary dictionary;

		public TestConfigurationContext()
			: base(GenerateConfigurationDictionary())
		{
		}

		private static ConfigurationDictionary GenerateConfigurationDictionary()
		{
			if (dictionary == null)
			{
				dictionary = new ConfigurationDictionary();
				dictionary.Add(ConfigurationSettings.SectionName, GenerateConfigurationSettings());
				dictionary.Add(SecuritySettings.SectionName, GenerateSecuritySettings());
				dictionary.Add(CacheManagerSettings.SectionName, GenerateCachingSettings());
			}
			return dictionary;
		}

		private static ConfigurationSettings GenerateConfigurationSettings()
		{
			ConfigurationSettings settings = new ConfigurationSettings();
			XmlSerializerTransformerData transformer = new XmlSerializerTransformerData("Transformer");
			transformer.XmlIncludeTypes.Add(new XmlIncludeTypeData("CachingStoreProviderData", typeof(CachingStoreProviderData).AssemblyQualifiedName));
			settings.ConfigurationSections.Add(new ConfigurationSectionData(SecuritySettings.SectionName, false, new XmlFileStorageProviderData("Storage", "EnterpriseLibrary.Security.config"), transformer));

			XmlSerializerTransformerData transformer2 = new XmlSerializerTransformerData("DataBuilder");
			transformer2.XmlIncludeTypes.Add(new XmlIncludeTypeData("Isolated Storage Configuration Data", typeof(IsolatedStorageCacheStorageData).AssemblyQualifiedName));
			settings.ConfigurationSections.Add( new ConfigurationSectionData(CacheManagerSettings.SectionName, false, new XmlFileStorageProviderData("XmlStorage", "EnterpriseLibrary.Caching.config"), transformer2	));
			return settings;
		}

		private static SecuritySettings GenerateSecuritySettings()
		{
			SecuritySettings settings = new SecuritySettings();
			settings.DefaultSecurityCacheProviderName = "Security Cache Provider1";
			settings.SecurityCacheProviders.Add( new CachingStoreProviderData("Security Cache Provider1",5, 120,"InMemoryPersistence"));

			return settings;
		}

		private static CacheManagerSettings GenerateCachingSettings()
		{
			CacheManagerSettings settings = new CacheManagerSettings();

			settings.DefaultCacheManager = "ShortInMemoryPersistence";

//<cacheManager name="InMemoryPersistence" expirationPollFrequencyInSeconds="60" maximumElementsInCacheBeforeScavenging="100"
//    numberToRemoveWhenScavenging="10">
//    <cacheStorage xsi:type="CustomCacheStorageData" name="inMemory" type="Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.NullBackingStore, Microsoft.Practices.EnterpriseLibrary.Caching" />
//</cacheManager>
			settings.CacheManagers.Add( new CacheManagerData("InMemoryPersistence", 60, 100, 10, new CustomCacheStorageData("inMemory", typeof(NullBackingStore).AssemblyQualifiedName)) );


//<cacheManager name="SmallInMemoryPersistence" cacheStorageName="inMemory" expirationPollFrequencyInSeconds="1"
//    maximumElementsInCacheBeforeScavenging="3" numberToRemoveWhenScavenging="2">
//    <cacheStorage xsi:type="CustomCacheStorageData" name="inMemory" type="Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.NullBackingStore, Microsoft.Practices.EnterpriseLibrary.Caching" />
//</cacheManager>
			settings.CacheManagers.Add( new CacheManagerData("SmallInMemoryPersistence", 1, 3, 2, new CustomCacheStorageData("inMemory", typeof(NullBackingStore).AssemblyQualifiedName)));


//<cacheManager name="ShortInMemoryPersistence" cacheStorageName="inMemory" expirationPollFrequencyInSeconds="1"
//    maximumElementsInCacheBeforeScavenging="10" numberToRemoveWhenScavenging="2">
//    <cacheStorage xsi:type="CustomCacheStorageData" name="inMemory" type="Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.NullBackingStore, Microsoft.Practices.EnterpriseLibrary.Caching" />
//</cacheManager>
			settings.CacheManagers.Add( new CacheManagerData("ShortInMemoryPersistence", 1, 10, 2, new CustomCacheStorageData("inMemory", typeof(NullBackingStore).AssemblyQualifiedName)));

			return settings;
		}

	}
}

#endif