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
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestFixture]
    public class CacheManagerSettingsFixture : ConfigurationDesignHostTestBase
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
            CacheManagerNode defaultCacheManager = new CacheManagerNode();
            applicationNode.Nodes.Add(defaultCacheManager);
            defaultCacheManager.Name = "testName";

            CacheManagerSettingsNode node = new CacheManagerSettingsNode();
            applicationNode.Nodes.AddWithDefaultChildren(node);
            Assert.AreEqual(SR.DefaultCacheManagerSettingsNodeName, node.Name);
            
            node.DefaultCacheManager = defaultCacheManager;
            Assert.AreEqual(defaultCacheManager, node.DefaultCacheManager);

            CacheManagerSettings data = node.CacheManagerSettings;

            Assert.AreEqual(defaultCacheManager.Name, data.DefaultCacheManager);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MakeSureThatYouCanOnlyAddOneCacheManagerCollectionNode()
        {
            CacheManagerNode defaultCacheManager = new CacheManagerNode();
            applicationNode.Nodes.Add(defaultCacheManager);
            defaultCacheManager.Name = "testName";

            CacheManagerSettingsNode node = new CacheManagerSettingsNode();
            applicationNode.Nodes.AddWithDefaultChildren(node);
            CacheManagerCollectionNode collectionNode = new CacheManagerCollectionNode();
            node.Nodes.Add(collectionNode);
        }

        [Test]
        public void DataTest()
        {
            CacheStorageData cacheStorage = new CustomCacheStorageData();
            cacheStorage.Name = "testCacheStorage";

            CacheManagerDataCollection cacheManagers = new CacheManagerDataCollection();
            CacheManagerData testManager1 = new CacheManagerData();
            testManager1.Name = "testName";
            testManager1.CacheStorage = cacheStorage;
            cacheManagers.Add(testManager1);

            CacheManagerSettings data = new CacheManagerSettings();
            data.CacheManagers.Clear();
            data.CacheManagers.AddRange(cacheManagers);

            CacheManagerSettingsNode node = new CacheManagerSettingsNode(data);
            applicationNode.Nodes.Add(node);
            CacheManagerSettings nodeData = node.CacheManagerSettings;

            Assert.AreSame(testManager1, nodeData.CacheManagers[testManager1.Name]);
        }

        [Test]
        public void AddChildrenTest()
        {
            CacheManagerSettingsNode node = new CacheManagerSettingsNode();
            applicationNode.Nodes.AddWithDefaultChildren(node);
            Assert.AreEqual(1, node.Nodes.Count);
            Assert.AreEqual(typeof(CacheManagerCollectionNode), node.Nodes[0].GetType());
        }
    }
}

#endif