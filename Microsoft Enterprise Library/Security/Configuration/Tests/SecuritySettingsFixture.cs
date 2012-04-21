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

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Tests
{
    [TestFixture]
    public class SecuritySettingsFixture
    {
		private ConfigurationContext context = new TestConfigurationContext();
        private SecuritySettings settings;

		[TestFixtureSetUp]
        public void FixtureSetup()
        {
            settings = (SecuritySettings)context.GetConfiguration(SecuritySettings.SectionName);
        }
        

        [Test]
        public void Properties()
        {
            string authzDefault = "authzDefault";
            SecuritySettings settings = new SecuritySettings();
            settings.DefaultAuthorizationProviderName = authzDefault;
            Assert.AreEqual(authzDefault, settings.DefaultAuthorizationProviderName, "authz default");
        }

        [Test]
        public void AuthenticationCollection()
        {
            SecuritySettings settings = new SecuritySettings();

            AuthenticationProviderDataCollection authProviders = new AuthenticationProviderDataCollection();
            CustomAuthenticationProviderData data = new CustomAuthenticationProviderData();
            data.Name = "name";
            authProviders.Add(data);
            foreach (AuthenticationProviderData authenticationProviderData in authProviders)
            {
                settings.AuthenticationProviders.Add(authenticationProviderData);
            }
            Assert.AreEqual(settings.AuthenticationProviders.Count, 1);
        }

        [Test]
        public void AuthorizationCollection()
        {
            SecuritySettings settings = new SecuritySettings();

            AuthorizationProviderDataCollection authProviders = new AuthorizationProviderDataCollection();
            CustomAuthorizationProviderData data = new CustomAuthorizationProviderData();
            data.Name = "name";
            authProviders.Add(data);
            foreach (AuthorizationProviderData providerData in authProviders)
            {
                settings.AuthorizationProviders.Add(providerData);
            }
            Assert.AreEqual(settings.AuthorizationProviders.Count, 1);
        }

        [Test]
        public void DefaultAuthorization()
        {
            string defaultName = "Authorization Provider";
            Assert.AreEqual(defaultName, settings.DefaultAuthorizationProviderName);
        }

        [Test]
        public void DefaultAuthentication()
        {
            string defaultName = "Authentication Provider";
            Assert.AreEqual(defaultName, settings.DefaultAuthenticationProviderName);
        }

        [Test]
        public void DefaultRole()
        {
            string defaultName = "Roles Provider";
            Assert.AreEqual(defaultName, settings.DefaultRolesProviderName);
        }

        [Test]
        public void DefaultProfile()
        {
            string defaultName = "Profile Provider";
            Assert.AreEqual(defaultName, settings.DefaultProfileProviderName);
        }

        [Test]
        public void DefaultSecurityCache()
        {
            string defaultName = "Security Cache Provider1";
            Assert.AreEqual(defaultName, settings.DefaultSecurityCacheProviderName);
        }
    }
}

#endif