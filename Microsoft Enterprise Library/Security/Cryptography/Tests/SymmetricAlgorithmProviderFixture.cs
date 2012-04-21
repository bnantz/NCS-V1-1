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
    public class SymmetricAlgorithmProviderFixture : SymmetricProviderBaseFixture
    {
		private ConfigurationContext context = new TestConfigurationContext();
        private string symmetricInstanceName = "symmetricAlgorithm1";

        private ISymmetricCryptoProvider symmProvider;

		[TestFixtureSetUp]
        public void FixtureSetup()
        {
            byte[] key = new byte[32];
            CryptographyUtility.GetRandomBytes(key);
            CryptographySettings settings = (CryptographySettings)context.GetConfiguration(CryptographySettings.SectionName);
            ((SymmetricAlgorithmProviderData)settings.SymmetricCryptoProviders[symmetricInstanceName]).Key = key;

            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);
            symmProvider = factory.CreateSymmetricCryptoProvider(symmetricInstanceName);
        }

        [Test]
        [ExpectedException(typeof(CryptographicException))]
        public void DecryptBadText()
        {
            DefaultSymmProvider.Decrypt(new byte[] {0, 1, 2, 3, 4});
        }

        public override ISymmetricCryptoProvider DefaultSymmProvider
        {
            get { return symmProvider; }
        }
    }
}

#endif