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

#if       UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class AddConfigurationSectionCommandFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void ExecuteTest()
        {
            AddConfigurationSectionCommand command = new AddConfigurationSectionCommand(Host, typeof(MockConfigurationNode), "mySection");
            command.Execute(GeneratedApplicationNode);
            Assert.AreEqual(2, GeneratedApplicationNode.Nodes.Count);
            ConfigurationSectionCollectionNode collectionNode = (ConfigurationSectionCollectionNode)GeneratedHierarchy.FindNodeByType(GeneratedApplicationNode, typeof(ConfigurationSectionCollectionNode));
            Assert.IsNotNull(collectionNode);
            ConfigurationSectionNode sectionNode = GeneratedHierarchy.FindNodeByName(collectionNode, "mySection") as ConfigurationSectionNode;
            Assert.IsNotNull(sectionNode);
        }

        private class MockConfigurationNode : ConfigurationNode
        {
            public MockConfigurationNode() : base()
            {
            }
        }
    }
}

#endif