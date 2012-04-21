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
    public class ConfigurationDesignManagerFixture : ConfigurationDesignHostTestBase
    {
        private ConfigurationDesignManager manager;

        public override void SetUp()
        {
            base.SetUp();
            manager = new ConfigurationDesignManager();
        }

        [Test]
        public void RegisterTest()
        {
            manager.Register(Host);
            INodeCreationService service = GetService(typeof(INodeCreationService)) as INodeCreationService;
            Assert.IsNotNull(service);
        }

        [Test]
        public void OpenTest()
        {
            Assert.AreEqual(0, GeneratedApplicationNode.Nodes.Count);
            manager.Register(Host);
            manager.Open(Host);
            Assert.AreEqual(SR.DefaultConfigurationSectionCollectionNodeName, GeneratedApplicationNode.Nodes[0].Name);
            ConfigurationSectionCollectionNode sectionsNode = GeneratedApplicationNode.Nodes[0] as ConfigurationSectionCollectionNode;
            Assert.IsNotNull(sectionsNode);
            Assert.AreEqual("Test", sectionsNode.Nodes[1].Name);
        }

        [Test]
        public void SaveTest()
        {
            manager.Register(Host);
            manager.Open(Host);
            Assert.AreEqual(SR.DefaultConfigurationSectionCollectionNodeName, GeneratedApplicationNode.Nodes[0].Name);
            ConfigurationSectionCollectionNode sectionsNode = GeneratedApplicationNode.Nodes[0] as ConfigurationSectionCollectionNode;
            Assert.IsNotNull(sectionsNode);
            Assert.AreEqual("Test", sectionsNode.Nodes[1].Name);
            manager.Save(Host);
            GeneratedApplicationNode.Nodes.Clear();
            Assert.AreEqual(0, GeneratedApplicationNode.childNodeLookup.Count);
            ApplicationConfigurationNode node = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            base.HierarchyService.SelectedHierarchy = hierarchy;
            manager.Open(Host);
            Assert.AreEqual(1, node.Nodes.Count);
            Assert.AreEqual(SR.DefaultConfigurationSectionCollectionNodeName, node.Nodes[0].Name);
            sectionsNode = node.Nodes[0] as ConfigurationSectionCollectionNode;
            Assert.IsNotNull(sectionsNode);
            Assert.AreEqual("Test", sectionsNode.Nodes[1].Name);
            manager.Save(Host);
        }

        [Test]
        public void BuildContext()
        {
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            Assert.AreEqual(0, dictionary.Count);
            manager.BuildContext(Host, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(typeof(ConfigurationSettings), dictionary[ConfigurationSettings.SectionName].GetType());
        }
    }
}

#endif