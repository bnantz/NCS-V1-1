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
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestFixture]
    public class DpapiSymmProviderMachineModeFixture : SymmetricProviderBaseFixture
    {
        private TestCryptographyConfigurationView configurationView;

        [SetUp]
        public void SetUp()
        {
        	configurationView = new TestCryptographyConfigurationView();
        }

        public override ISymmetricCryptoProvider DefaultSymmProvider
        {
            get
            {
                DpapiSymmetricCryptoProvider symm = new DpapiSymmetricCryptoProvider();

                symm.Initialize(configurationView);

                return symm;
            }
        }

        private class TestCryptographyConfigurationView : CryptographyConfigurationView
        {
            private byte[] entropy;

            public TestCryptographyConfigurationView() : base(new ConfigurationContext(new ConfigurationDictionary()))
            {
                entropy = new byte[16];
                CryptographyUtility.GetRandomBytes(entropy);
            }

            public override SymmetricCryptoProviderData GetSymmetricCryptoProviderData(string instanceName)
            {
                DpapiSymmetricCryptoProviderData data = new DpapiSymmetricCryptoProviderData();
                data.Name = "name";
                DpapiSettingsData settingsData = new DpapiSettingsData();
                settingsData.Mode = DpapiStorageMode.Machine;
                settingsData.Entropy = this.entropy;
                data.DataProtectionMode = settingsData;
                return data;
            }

        }
    }
}

#endif