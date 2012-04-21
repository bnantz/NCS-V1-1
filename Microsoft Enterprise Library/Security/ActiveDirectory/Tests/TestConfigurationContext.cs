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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Tests
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
			}
			return dictionary;
		}

		private static ConfigurationSettings GenerateConfigurationSettings()
		{
			ConfigurationSettings settings = new ConfigurationSettings();
			XmlSerializerTransformerData transformer = new XmlSerializerTransformerData("Transformer");
			transformer.XmlIncludeTypes.Add( new XmlIncludeTypeData("AdRolesIncludeType", typeof(AdRolesProviderData).AssemblyQualifiedName));
			settings.ConfigurationSections.Add(new ConfigurationSectionData(SecuritySettings.SectionName, false, new XmlFileStorageProviderData("Storage", "EnterpriseLibrary.Security.config"), transformer));
			return settings;
		}

		private static SecuritySettings GenerateSecuritySettings()
		{
			SecuritySettings settings = new SecuritySettings();

			AdRolesProviderData providerData = new AdRolesProviderData("AdRolesProviderName", "LDAP", "entlibbldwchr:389", "CN=EntLibUsers,O=EntLib,C=US");
			providerData.AccountName = "CN";
			providerData.TypeName = typeof(AdRolesProvider).AssemblyQualifiedName;
			settings.RolesProviders.Add( providerData );

			return settings;
		}
	}
}

#endif