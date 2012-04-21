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
using Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
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
			settings.ConfigurationSections.Add(new ConfigurationSectionData(SecuritySettings.SectionName, false, new XmlFileStorageProviderData("Storage", "EnterpriseLibrary.Security.config"), new XmlSerializerTransformerData("securityConfiguration")));
			return settings;
		}

		private static SecuritySettings GenerateSecuritySettings()
		{
			SecuritySettings settings = new SecuritySettings();
			settings.DefaultAuthorizationProviderName = "Authorization Provider";
			settings.DefaultAuthenticationProviderName = "Authentication Provider";
			settings.DefaultProfileProviderName = "Profile Provider";
			settings.DefaultRolesProviderName = "Roles Provider";
			settings.DefaultSecurityCacheProviderName = "Security Cache Provider1";

			AuthorizationRuleProviderData providerData = new AuthorizationRuleProviderData("RuleProvider");
			providerData.Rules.Add(new AuthorizationRuleData("Rule1", "I:TestUser OR R:Admin"));
			settings.AuthorizationProviders.Add(providerData);

			settings.AuthorizationProviders.Add(new CustomAuthorizationProviderData("Authorization Provider", typeof (MockAuthorizationProvider).AssemblyQualifiedName));
			settings.AuthorizationProviders.Add(new CustomAuthorizationProviderData("provider1", typeof(MockAuthorizationProvider).AssemblyQualifiedName ));
			settings.AuthorizationProviders.Add(new CustomAuthorizationProviderData("provider2", typeof(Mock2AuthorizationProvider).AssemblyQualifiedName ));

			settings.AuthenticationProviders.Add(new CustomAuthenticationProviderData("Authentiction Provider", typeof (MockAuthenticationProvider).AssemblyQualifiedName));

			settings.RolesProviders.Add(new CustomRolesProviderData("Roles Provider", typeof (MockRolesProvider).AssemblyQualifiedName));

			settings.ProfileProviders.Add(new CustomProfileProviderData("Profile Provider", typeof (MockProfileProvider).AssemblyQualifiedName));

			settings.SecurityCacheProviders.Add(new CustomSecurityCacheProviderData("Security Cache Provider1", typeof (MockSecurityCacheProvider).AssemblyQualifiedName));

			return settings;
		}
	}
}

#endif