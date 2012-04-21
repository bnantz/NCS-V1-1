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
using System.Configuration;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestFixture]
    public class AuthenticationProviderFactoryFixture
    {
        private class MockAuthenticationProvider : ConfigurationProvider, IAuthenticationProvider
        {
            public MockAuthenticationProvider()
            {
            }

            public override void Initialize(ConfigurationView configurationView)
            {
            }

            public bool Authenticate(object credentials, out IIdentity identity)
            {
                identity = new GenericIdentity(String.Empty);
                return false;
            }
        }

        private class Mock2AuthenticationProvider : ConfigurationProvider, IAuthenticationProvider
        {
            private bool initialized;

            public Mock2AuthenticationProvider()
            {
            }

            public override void Initialize(ConfigurationView configurationView)
            {
                initialized = true;
            }

            public bool Authenticate(object credentials, out IIdentity identity)
            {
                identity = new MockIdentity();
                return true;
            }

            public bool Initialized
            {
                get { return initialized; }
            }
        }

        private class MockIdentity : IIdentity
        {
            public bool IsAuthenticated
            {
                get { return true; }
            }

            public string Name
            {
                get { return "Mock"; }
            }

            public string AuthenticationType
            {
                get { return "Mock"; }
            }
        }

        private AuthenticationProviderFactory factory;

        [TestFixtureSetUp]
        public void Setup()
        {
            SecuritySettings data = new SecuritySettings();
            data.DefaultAuthenticationProviderName = "provider2";

            CustomAuthenticationProviderData provider1Data =
                new CustomAuthenticationProviderData();
            provider1Data.Name = "provider1";
            provider1Data.TypeName = typeof(MockAuthenticationProvider).AssemblyQualifiedName;
            data.AuthenticationProviders.Add(provider1Data);

            CustomAuthenticationProviderData provider2Data =
                new CustomAuthenticationProviderData();
            provider2Data.Name = "provider2";
            provider2Data.TypeName = typeof(Mock2AuthenticationProvider).AssemblyQualifiedName;
            // provider2Data.Default = true;
            data.AuthenticationProviders.Add(provider2Data);

            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            dictionary[SecuritySettings.SectionName] = data;
            ConfigurationContext context = ConfigurationManager.CreateContext(dictionary);
            this.factory = new AuthenticationProviderFactory(context);
        }

        [Test]
        public void GetDefaultTest()
        {
            IAuthenticationProvider provider = factory.GetAuthenticationProvider();
            Assert.IsNotNull(provider);

            Mock2AuthenticationProvider mockProvider =
                provider as Mock2AuthenticationProvider;

            Assert.IsNotNull(mockProvider);
            Assert.IsTrue(mockProvider.Initialized);
        }

        [Test]
        public void GetByNameTest()
        {
            IAuthenticationProvider provider = factory.GetAuthenticationProvider("provider1");
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is MockAuthenticationProvider);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ProviderNotFoundTest()
        {
            factory.GetAuthenticationProvider("provider3");
        }
    }
}

#endif