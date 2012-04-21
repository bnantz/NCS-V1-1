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
    public class AddChildNodeCommandFixture : ConfigurationDesignHostTestBase
    {
        private AddChildNodeCommand addChildNodeCommand;
        private TestConfigurationNode node;

        public override void SetUp()
        {
            base.SetUp();
            node = new TestConfigurationNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            addChildNodeCommand = new AddChildNodeCommand(Host, typeof(TestConfigurationNode));
        }

        [Test]
        public void ExecuteTest()
        {
            addChildNodeCommand.Execute(node);
            Assert.AreEqual(typeof(TestConfigurationNode), addChildNodeCommand.ChildNode.GetType());
            Assert.AreEqual("My Test", addChildNodeCommand.ChildNode.Name);
            Assert.AreEqual(1, node.Nodes.Count);
        }

        private class TestConfigurationNode : ConfigurationNode
        {
            public TestConfigurationNode() : base()
            {
            }

            protected override void OnSited()
            {
                base.OnSited();
                Site.Name = "My Test";
            }

        }
    }
}

#endif