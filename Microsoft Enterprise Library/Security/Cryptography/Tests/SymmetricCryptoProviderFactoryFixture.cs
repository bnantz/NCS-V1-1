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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestFixture]
    public class SymmetricCryptoProviderFactoryFixture
    {
        private ConfigurationContext context = new TestConfigurationContext();

        [Test]
        public void GetDefaultTest()
        {
            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);

            ISymmetricCryptoProvider provider = factory.CreateSymmetricCryptoProvider("provider1");
            Assert.IsNotNull(provider);

            MockSymmetricCryptoProvider mockProvider = provider as MockSymmetricCryptoProvider;
            Assert.IsNotNull(mockProvider);
            Assert.IsTrue(mockProvider.Initialized);
        }

        [Test]
        public void GetByNameTest()
        {
            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);

            ISymmetricCryptoProvider provider = factory.CreateSymmetricCryptoProvider("provider1");
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is MockSymmetricCryptoProvider);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ProviderNotFoundTest()
        {
            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);
            factory.CreateSymmetricCryptoProvider("provider3");
        }

        [Test]
        public void GetMockSymmetricCryptoProvider()
        {
            string instance = "mockSymmetric1";
            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);
            MockSymmetricCryptoProvider store = (MockSymmetricCryptoProvider)factory.CreateSymmetricCryptoProvider(instance);

            Assert.IsNotNull(store);
        }

        [Test]
        public void GetDpapiSymmetricProvider()
        {
            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);
            DpapiSymmetricCryptoProvider dpapi =
                (DpapiSymmetricCryptoProvider)factory.CreateSymmetricCryptoProvider("dpapiSymmetric1");

            Assert.IsNotNull(dpapi);
        }

        [Test]
        public void GetSymmetricAlgorithmProvider()
        {
            SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(context);
            SymmetricAlgorithmProvider sym = (SymmetricAlgorithmProvider)factory.CreateSymmetricCryptoProvider("symmetricAlgorithm1");

            Assert.IsNotNull(sym);
        }
    }
}

#endif