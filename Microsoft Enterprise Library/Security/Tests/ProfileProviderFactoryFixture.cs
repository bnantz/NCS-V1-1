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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestFixture]
    public class ProfileProviderFactoryFixture
    {
        private ProfileProviderFactory factory;

        [TestFixtureSetUp]
        public void Setup()
        {
            SecuritySettings settings = new SecuritySettings();
            settings.DefaultProfileProviderName = "provider2";

            CustomProfileProviderData provider1Data =
                new CustomProfileProviderData();
            provider1Data.Name = "provider1";
            provider1Data.TypeName = typeof(MockProfileProvider).AssemblyQualifiedName;
            settings.ProfileProviders.Add(provider1Data);

            CustomProfileProviderData provider2Data =
                new CustomProfileProviderData();
            // provider2Data.Default = true;
            provider2Data.Name = "provider2";
            provider2Data.TypeName = typeof(Mock2ProfileProvider).AssemblyQualifiedName;
            settings.ProfileProviders.Add(provider2Data);

            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            dictionary[SecuritySettings.SectionName] = settings;
            ConfigurationContext context = ConfigurationManager.CreateContext(dictionary);
            this.factory = new ProfileProviderFactory(context);
        }

        [Test]
        public void GetDefaultTest()
        {
            IProfileProvider provider = factory.GetProfileProvider();
            Assert.IsNotNull(provider);
            Mock2ProfileProvider mockProvider =
                provider as Mock2ProfileProvider;
            Assert.IsNotNull(mockProvider);
            Assert.IsTrue(mockProvider.Initialized);
        }

        [Test]
        public void GetByNameTest()
        {
            IProfileProvider provider = factory.GetProfileProvider("provider1");
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is MockProfileProvider);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ProviderNotFoundTest()
        {
            factory.GetProfileProvider("provider3");
        }
    }
}

#endif