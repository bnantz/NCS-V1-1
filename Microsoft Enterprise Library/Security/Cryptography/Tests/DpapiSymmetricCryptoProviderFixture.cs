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
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestFixture]
    public class DpapiSymmetricCryptoProviderFixture : SymmetricProviderBaseFixture
    {
		private ConfigurationContext context = new TestConfigurationContext();
        private byte[] plainText = new byte[16];

        [SetUp]
        public void Setup()
        {
            RNGCryptoServiceProvider.Create().GetNonZeroBytes(plainText);
        }

        [Test]
        public void NewProvider()
        {
            DpapiSymmetricCryptoProvider symm = new DpapiSymmetricCryptoProvider();

        	CryptographyConfigurationView cryptoConfigurationView = new TestCryptographyConfigurationView();
            symm.Initialize(cryptoConfigurationView);
            byte[] encBytes = symm.Encrypt(plainText);
            byte[] decBytes = symm.Decrypt(encBytes);
            Assert.IsTrue(CryptographyUtility.CompareBytes(decBytes, plainText));
        }

        [Test]
        public void InitData()
        {
            DpapiSymmetricCryptoProvider cryptoProvider = (DpapiSymmetricCryptoProvider)DefaultSymmProvider;
            Assert.AreEqual(cryptoProvider.DpapiCrypto.StorageMode, DpapiStorageMode.User);
        }

        public override ISymmetricCryptoProvider DefaultSymmProvider
        {
            get
            {
                // user mode dpapi provider defined in config
                return GetCryptoProvider("dpapiSymmetric1");
            }
        }

        private DpapiSymmetricCryptoProvider GetCryptoProvider(string instanceName)
        {
            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);
            return (DpapiSymmetricCryptoProvider)factory.CreateSymmetricCryptoProvider(instanceName);
        }

        private class TestCryptographyConfigurationView : CryptographyConfigurationView
        {
            public TestCryptographyConfigurationView() : base(new ConfigurationContext(new ConfigurationDictionary()))
            {
            }

            public override SymmetricCryptoProviderData GetSymmetricCryptoProviderData(string instanceName)
            {
                DpapiSymmetricCryptoProviderData data = new DpapiSymmetricCryptoProviderData();
                data.Name = "name";
                DpapiSettingsData settingsData = new DpapiSettingsData();
                settingsData.Mode = DpapiStorageMode.User;
                data.DataProtectionMode = settingsData;
                return data;
            }

        }
    }
}

#endif