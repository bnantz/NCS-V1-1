//===============================================================================
// Microsoft patterns & practices Enterprise Library
// XXXXX Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class AddXmlIncludeTypesCommandFixture : ConfigurationDesignHostTestBase
    {
        private static readonly string section = "mySection";

        public override void SetUp()
        {
            base.SetUp();
            XmlIncludeTypeService.AddXmlIncludeType(section, typeof(MyConfigurationData));
            
            Type nodeType = typeof(MyConfigurationNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(MyConfigurationData), "My Configuraiton Node");
            NodeCreationService.AddNodeCreationEntry(entry);
        }

        [Test]
        public void MakeSureThatIncludeTypesAreAddedForTheSection()
        {
            AddConfigurationSectionCommand cmd = new AddConfigurationSectionCommand(Host, typeof(MyConfigurationNode), section);
            cmd.Execute(GeneratedApplicationNode);

            ConfigurationSectionCollectionNode node = (ConfigurationSectionCollectionNode)GeneratedHierarchy.FindNodeByType(typeof(ConfigurationSectionCollectionNode));
            Assert.IsNotNull(node);
            ConfigurationSectionNode sectionNode = (ConfigurationSectionNode)GeneratedHierarchy.FindNodeByName(node, section);
            ConfigurationNode[] types = GeneratedHierarchy.FindNodesByType(sectionNode, typeof(XmlIncludeTypeNode));
            Assert.AreEqual(1, types.Length);
            XmlIncludeTypeNode xmlIncludeTypeNode = types[0] as XmlIncludeTypeNode;
            Assert.IsNotNull(xmlIncludeTypeNode);
            Assert.AreEqual(xmlIncludeTypeNode.Name, typeof(MyConfigurationData).Name);
        }

        [Test]
        public void AddingSameNodeOnlyAddsTheXmlIncludeTypeOnce()
        {
            AddConfigurationSectionCommand cmd = new AddConfigurationSectionCommand(Host, typeof(MyConfigurationNode), section);
            cmd.Execute(GeneratedApplicationNode);
            cmd.Execute(GeneratedApplicationNode);

            ConfigurationSectionCollectionNode node = (ConfigurationSectionCollectionNode)GeneratedHierarchy.FindNodeByType(typeof(ConfigurationSectionCollectionNode));
            Assert.IsNotNull(node);
            ConfigurationSectionNode sectionNode = (ConfigurationSectionNode)GeneratedHierarchy.FindNodeByName(node, section);
            ConfigurationNode[] types = GeneratedHierarchy.FindNodesByType(sectionNode, typeof(XmlIncludeTypeNode));
            Assert.AreEqual(1, types.Length);
            XmlIncludeTypeNode xmlIncludeTypeNode = types[0] as XmlIncludeTypeNode;
            Assert.IsNotNull(xmlIncludeTypeNode);
            Assert.AreEqual(xmlIncludeTypeNode.Name, typeof(MyConfigurationData).Name);
        }

        private abstract class MyAbstractConfiguraitonData : ProviderData
        {
        }

        private class MyConfigurationData : MyAbstractConfiguraitonData
        {
            [XmlIgnore]
            public override string TypeName
            {
                get { return typeof(MyConfigurationData).AssemblyQualifiedName; }
                set
                {
                }
            }
        }

        private abstract class MyAbstractConfigurationNode : ConfigurationNode
        {
        }

        private class MyConfigurationNode : MyAbstractConfigurationNode
        {
        }

    }
}

#endif