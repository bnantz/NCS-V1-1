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
    public class RolesProviderFactoryFixture
    {
        private RolesProviderFactory factory;

        [TestFixtureSetUp]
        public void Setup()
        {
            SecuritySettings settings = new SecuritySettings();
            settings.DefaultRolesProviderName = "provider2";

            CustomRolesProviderData provider1Data =
                new CustomRolesProviderData();
            provider1Data.Name = "provider1";
            provider1Data.TypeName = typeof(MockRolesProvider).AssemblyQualifiedName;
            settings.RolesProviders.Add(provider1Data);

            CustomRolesProviderData provider2Data =
                new CustomRolesProviderData();
            //    provider2Data.Default = true;
            provider2Data.Name = "provider2";
            provider2Data.TypeName = typeof(Mock2RolesProvider).AssemblyQualifiedName;
            settings.RolesProviders.Add(provider2Data);

            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            dictionary[SecuritySettings.SectionName] = settings;
            ConfigurationContext context = ConfigurationManager.CreateContext(dictionary);
            this.factory = new RolesProviderFactory(context);
        }

        [Test]
        public void GetDefaultTest()
        {
            IRolesProvider provider = factory.GetRolesProvider();
            Assert.IsNotNull(provider);
            Mock2RolesProvider mockProvider =
                provider as Mock2RolesProvider;
            Assert.IsNotNull(mockProvider);
            Assert.IsTrue(mockProvider.Initialized);
        }

        [Test]
        public void GetByNameTest()
        {
            IRolesProvider provider = factory.GetRolesProvider("provider1");
            Assert.IsNotNull(provider);
            Assert.IsTrue(provider is MockRolesProvider);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ProviderNotFoundTest()
        {
            factory.GetRolesProvider("provider3");
        }
    }
}

#endif