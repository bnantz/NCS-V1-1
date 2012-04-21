//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System.Security.Cryptography;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
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
				dictionary.Add(CryptographySettings.SectionName, GenerateBlockSettings());
			}
			return dictionary;
		}

		private static ConfigurationSettings GenerateConfigurationSettings()
		{
			ConfigurationSettings settings = new ConfigurationSettings();
			settings.ConfigurationSections.Add(new ConfigurationSectionData(CryptographySettings.SectionName, false, new XmlFileStorageProviderData("Storage", "EnterpriseLibrary.Security.Cryptography.config"), new XmlSerializerTransformerData("Transformer")));
			return settings;
		}

		private static CryptographySettings GenerateBlockSettings()
		{
			CryptographySettings settings = new CryptographySettings();

			settings.HashProviders.Add(new CustomHashProviderData("mockHashProvider1", typeof (MockHashProvider).AssemblyQualifiedName));
			settings.HashProviders.Add(new KeyedHashAlgorithmProviderData("hmac1", typeof (HMACSHA1).AssemblyQualifiedName, true, new byte[] {}));
			settings.HashProviders.Add(new HashAlgorithmProviderData("hashAlgorithm1", typeof (SHA1Managed).AssemblyQualifiedName, false));
			settings.HashProviders.Add( new CustomHashProviderData("provider1", typeof (MockHashProvider).AssemblyQualifiedName));
			settings.HashProviders.Add(new CustomHashProviderData("provider2", typeof (MockHashProvider).AssemblyQualifiedName));

			settings.SymmetricCryptoProviders.Add(new CustomSymmetricCryptoProviderData("mockSymmetric1", typeof (MockSymmetricCryptoProvider).AssemblyQualifiedName));
			settings.SymmetricCryptoProviders.Add(new DpapiSymmetricCryptoProviderData("dpapiSymmetric1", new DpapiSettingsData(new byte[] {}, DpapiStorageMode.User)));
			settings.SymmetricCryptoProviders.Add(new SymmetricAlgorithmProviderData("symmetricAlgorithm1", typeof (RijndaelManaged).AssemblyQualifiedName, Encoding.UTF8.GetBytes("TODO")));
			settings.SymmetricCryptoProviders.Add(new CustomSymmetricCryptoProviderData("provider1",typeof(MockSymmetricCryptoProvider).AssemblyQualifiedName));
			settings.SymmetricCryptoProviders.Add(new CustomSymmetricCryptoProviderData("provider2", typeof(MockSymmetricCryptoProvider).AssemblyQualifiedName ));

			return settings;
		}
	}
}

#endif