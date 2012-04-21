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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestFixture]
    public class CacheManagersNodeFixture : ConfigurationDesignHostTestBase
    {
        private ApplicationConfigurationNode applicationNode;

        public override void SetUp()
        {
            base.SetUp();
            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            applicationNode = new ApplicationConfigurationNode(data);
            CreateHierarchyAndAddToHierarchyService(applicationNode, CreateDefaultConfiguration());
            CachingConfigurationDesignManager manager = new CachingConfigurationDesignManager();
            manager.Register(Host);
        }

        [Test]
        public void NodeTest()
        {
            CacheManagerCollectionNode node = new CacheManagerCollectionNode();
            applicationNode.Nodes.Add(node);
            Assert.AreEqual(SR.DefaultCacheManagerCollectionNodeName, node.Name);

            CacheManagerNode cacheManagerNode = new CacheManagerNode();
            node.Nodes.Add(cacheManagerNode);
            cacheManagerNode.Name = "tesotvetyevt";

            CacheManagerDataCollection managers = node.CacheManagerDataCollection;
            Assert.IsNotNull(managers[cacheManagerNode.Name]);
        }

        [Test]
        public void DataTest()
        {
            CacheStorageData cacheStorage = new CustomCacheStorageData();
            cacheStorage.Name = "testevtu8entv";
            CacheManagerDataCollection data = new CacheManagerDataCollection();
            CacheManagerData cacheManagerData = new CacheManagerData();
            cacheManagerData.CacheStorage = cacheStorage;
            cacheManagerData.Name = "tesotvetyevt";

            data.Add(cacheManagerData);

            CacheManagerCollectionNode node = new CacheManagerCollectionNode(data);
            applicationNode.Nodes.Add(node);
            CacheManagerDataCollection nodeData = node.CacheManagerDataCollection;

            Assert.AreEqual(data.Count, nodeData.Count);
            Assert.AreEqual(data[cacheManagerData.Name].CacheStorage.Name, nodeData[cacheManagerData.Name].CacheStorage.Name);
        }
    }
}

#endif