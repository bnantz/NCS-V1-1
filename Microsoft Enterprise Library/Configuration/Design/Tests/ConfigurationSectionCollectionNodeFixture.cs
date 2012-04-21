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
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;
using NUnit.Framework;
                                                                                                            
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationSectionCollectionNodeFixture : ConfigurationDesignHostTestBase
    {
        private ConfigurationSectionCollectionNode configurationSectionsNode;

        public override void SetUp()
        {
            base.SetUp();
            INodeCreationService nodeCreationService = GetService(typeof(INodeCreationService)) as INodeCreationService;
            Assert.IsNotNull(nodeCreationService);
            
            Type nodeType = typeof(CustomKeyAlgorithmStorageProviderNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(CustomKeyAlgorithmPairStorageProviderData), SR.CustomKeyAlgorithmPairStorageProviderNodeDefaultName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomTransformerNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(CustomTransformerData), SR.CustomTransformerNodeDefaultName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomStorageProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(CustomStorageProviderData), SR.CustomStorageProviderNodeDefaultName);
            nodeCreationService.AddNodeCreationEntry(entry);


            nodeType = typeof(XmlFileStorageProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(XmlFileStorageProviderData), SR.XMLStorageProviderNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(XmlSerializerTransformerNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(XmlSerializerTransformerData), SR.XmlSerializerTransformerNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(ConfigurationSectionNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(ConfigurationSectionData), SR.ConfigurationSectionNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            nodeType = typeof(ReadOnlyConfigurationSectionNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(ReadOnlyConfigurationSectionData), SR.ReadOnlyConfigurationSectionNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(FileKeyAlgorithmPairStorageProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddFileKeyAlgorithmPairNodeCommand(Host, nodeType), nodeType, typeof(FileKeyAlgorithmPairStorageProviderData), SR.FileKeyAlgorithmStorageProviderNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);
            
            configurationSectionsNode = new ConfigurationSectionCollectionNode(ConfigurationSettingsBuilder.Create());
            Assert.IsNotNull(configurationSectionsNode);
            CreateHierarchyAndAddToHierarchyService(configurationSectionsNode, CreateDefaultConfiguration());
        }

        [Test]
        public void CountSectionsTest()
        {
            Assert.AreEqual(2, configurationSectionsNode.Nodes.Count);
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual(SR.DefaultConfigurationSectionCollectionNodeName, configurationSectionsNode.Name);
        }

        [Test]
        public void DefaultNodeTest()
        {
            ConfigurationSectionCollectionNode node = new ConfigurationSectionCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(1, node.Nodes.Count);
            Assert.AreEqual(SR.DefaultEncryptionSettingsNodeName, node.Nodes[0].Name);
        }
    }
}

#endif