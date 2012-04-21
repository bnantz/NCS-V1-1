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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class UIHierarchyFixture : ConfigurationDesignHostTestBase
    {
        private UIHierarchy hierarchy;

        public override void SetUp()
        {
            base.SetUp();
            hierarchy = new UIHierarchy(Host, CreateDefaultConfiguration());
            HierarchyService.AddHierarchy(hierarchy);
        }

        [Test]
        public void RootNode()
        {
            ApplicationConfigurationNode node = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            hierarchy.RootNode = node;
            Assert.AreSame(node, hierarchy.RootNode);
        }

        [Test]
        public void SelectedNode()
        {
            ApplicationConfigurationNode node = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            hierarchy.RootNode = node;
            Assert.AreSame(node, hierarchy.SelectedNode);
        }

        [Test]
        public void FindByPathTest()
        {
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            ConfigurationSectionCollectionNode node = new ConfigurationSectionCollectionNode();
            hierarchy.RootNode = appNode;
            appNode.Nodes.Add(node);
            node.Nodes.Add(new ConfigurationSectionNode());
            ConfigurationNode foundNode = hierarchy.FindNodeByPath(node.Path);
            Assert.AreSame(node, foundNode);
        }

        [Test]
        public void FindByTypeTestDepthofOne()
        {
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            hierarchy.RootNode = appNode;
            ConfigurationNode[] foundNodes = hierarchy.FindNodesByType(typeof(ApplicationConfigurationNode));
            Assert.AreEqual(1, foundNodes.Length);
            Assert.AreSame(appNode, foundNodes[0]);
        }

        [Test]
        public void FindByTypeTestChildofParent()
        {
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            hierarchy.RootNode = appNode;
            appNode.Nodes.Add(new ConfigurationSectionCollectionNode());
            appNode.Nodes.Add(new ConfigurationSectionCollectionNode());
            ConfigurationNode[] foundNodes = hierarchy.FindNodesByType(typeof(ConfigurationSectionCollectionNode));
            Assert.AreEqual(2, foundNodes.Length);
        }

        [Test]
        public void FindByTypeGrandChildTest()
        {
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            ConfigurationSectionCollectionNode node = new ConfigurationSectionCollectionNode();
            hierarchy.RootNode = appNode;
            appNode.Nodes.Add(node);
            node.Nodes.Add(new ConfigurationSectionCollectionNode());
            ConfigurationNode[] foundNodes = hierarchy.FindNodesByType(typeof(ConfigurationSectionCollectionNode));
            Assert.AreEqual(2, foundNodes.Length);
        }

        [Test]
        public void FindByTypeGrandGrandChildTest()
        {
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            ConfigurationSectionCollectionNode node = new ConfigurationSectionCollectionNode();
            hierarchy.RootNode = appNode;
            appNode.Nodes.Add(node);
            ConfigurationSectionCollectionNode deepNode = new ConfigurationSectionCollectionNode();
            node.Nodes.Add(deepNode);
            deepNode.Nodes.Add(new ConfigurationSectionNode());
            ConfigurationNode[] foundNodes = hierarchy.FindNodesByType(typeof(ConfigurationSectionNode));
            Assert.AreEqual(1, foundNodes.Length);
        }

        [Test]
        public void FindByTypeTwoTopSearchFromOneLowerChildTest()
        {
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            ConfigurationSectionCollectionNode node = new ConfigurationSectionCollectionNode();
            hierarchy.RootNode = appNode;
            appNode.Nodes.Add(node);
            EncryptionSettingsNode node2 = new EncryptionSettingsNode();
            ConfigurationSectionCollectionNode node3 = new ConfigurationSectionCollectionNode();
            appNode.Nodes.Add(node2);
            node2.Nodes.Add(node3);
            node3.Nodes.Add(new ConfigurationSectionNode());
            node3.Nodes.Add(new ConfigurationSectionNode());
            ConfigurationNode[] foundNodes = hierarchy.FindNodesByType(typeof(ConfigurationSectionNode));
            Assert.AreEqual(2, foundNodes.Length);
        }
    }
}

#endif