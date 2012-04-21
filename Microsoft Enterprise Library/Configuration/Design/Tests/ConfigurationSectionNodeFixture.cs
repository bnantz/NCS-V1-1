//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationSectionNodeFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void GetStorageProviderTest()
        {
            ConfigurationSectionNode sectionNode = new ConfigurationSectionNode();
            CreateHierarchyAndAddToHierarchyService(sectionNode, CreateDefaultConfiguration());
            Assert.IsNull(sectionNode.SelectStorageProviderNode());
            XmlFileStorageProviderData data = new XmlFileStorageProviderData("myData", "myPath");
            XmlFileStorageProviderNode providerNode = new XmlFileStorageProviderNode(data);
            sectionNode.Nodes.Add(providerNode);
            StorageProviderNode providerNodeCompare = sectionNode.SelectStorageProviderNode();
            Assert.IsNotNull(providerNodeCompare);
            Assert.AreSame(providerNode, providerNodeCompare);
        }
    }
}

#endif