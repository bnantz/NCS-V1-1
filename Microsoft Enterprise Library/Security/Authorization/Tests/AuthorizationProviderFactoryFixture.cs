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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests
{
    [TestFixture]
    public class AuthorizationProviderFactoryFixture
    {
        private AuthorizationProviderFactory factory;

        [TestFixtureSetUp]
        public void Setup()
        {
            this.factory = new AuthorizationProviderFactory(new TestConfigurationContext());
        }

        [Test]
        public void GetDefaultTest()
        {
            IAuthorizationProvider provider = factory.GetAuthorizationProvider();
            Assert.IsNotNull(provider);
            MockAuthorizationProvider mockProvider = provider as MockAuthorizationProvider;
            Assert.IsNotNull(mockProvider);
            Assert.IsTrue(mockProvider.Initialized);
        }

        [Test]
        public void GetByNameTest()
        {
            IAuthorizationProvider provider = factory.GetAuthorizationProvider("provider1");
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is MockAuthorizationProvider);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ProviderNotFoundTest()
        {
            factory.GetAuthorizationProvider("provider3");
        }
    }
}

#endif