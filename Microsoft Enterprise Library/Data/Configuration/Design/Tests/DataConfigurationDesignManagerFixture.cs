//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestFixture]
    public class DataConfigurationDesignManagerFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void BuildContextTest()
        {
            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            ApplicationConfigurationNode applicationNode = new ApplicationConfigurationNode(data);
            CreateHierarchyAndAddToHierarchyService(applicationNode, CreateDefaultConfiguration());
            DataConfigurationDesignManager b = new DataConfigurationDesignManager();
            b.Register(Host);
            b.Open(Host);
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            b.BuildContext(Host, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.Contains("dataConfiguration"));
        }
    }
}

#endif