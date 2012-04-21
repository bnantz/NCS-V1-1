//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if     UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestFixture]
    public class CacheStorageNodeFixture : ConfigurationDesignHostTestBase
    {
        private ApplicationConfigurationNode applicationNode;

        public override void SetUp()
        {
            base.SetUp();
            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            applicationNode = new ApplicationConfigurationNode(data);
            CreateHierarchyAndAddToHierarchyService(applicationNode, CreateDefaultConfiguration());
        }

        [Test]
        public void CustomNodeTest()
        {
            string name = "testName";
            string type = "testType";
            NameValueItemCollection extensions = new NameValueItemCollection();
            extensions.Add(new NameValueItem("test", "value"));

            CustomCacheStorageNode node = new CustomCacheStorageNode();
            applicationNode.Nodes.Add(node);
            Assert.AreEqual(SR.DefaultCacheStorageNodeName, node.Name);

            node.Type = type;
            Assert.AreEqual(type, node.Type);

            node.Name = name;
            Assert.AreEqual(name, node.Name);

            node.Extensions.Add(extensions[0]);
            Assert.AreEqual(extensions[0], node.Extensions[0]);

            CustomCacheStorageData nodeData = (CustomCacheStorageData)node.CacheStorageData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.TypeName);
            Assert.AreEqual(extensions[0], nodeData.Extensions[0]);

        }

        [Test]
        public void CustomDataTest()
        {
            string name = "testName";
            string type = "testType";
            NameValueItemCollection extensions = new NameValueItemCollection();
            extensions.Add(new NameValueItem("test", "value"));

            CustomCacheStorageData data = new CustomCacheStorageData();
            data.Name = name;
            data.TypeName = type;
            data.Extensions.Add(extensions[0]);

            CustomCacheStorageNode node = new CustomCacheStorageNode(data);
            applicationNode.Nodes.Add(node);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type, node.Type);
            Assert.AreEqual(extensions[0], node.Extensions[0]);
        }
    }
}

#endif