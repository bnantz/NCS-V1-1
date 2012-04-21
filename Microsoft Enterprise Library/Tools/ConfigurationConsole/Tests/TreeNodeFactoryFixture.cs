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

#if      UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Tools.ConfigurationConsole.Tests
{
    [TestFixture]
    public class TreeNodeFactoryFixture
    {
        private class MyConfigurationNode : ConfigurationNode
        {
            public MyConfigurationNode() : base("MyConfigurationNode")
            {
            }

            public MyConfigurationNode(string label) : base(label)
            {
            }

        }

        private class MyConfigurationTreeNode : ConfigurationTreeNode
        {
            public MyConfigurationTreeNode(ConfigurationNode node) : base(node)
            {
            }
        }

        [Test]
        public void RegisterTreeNode()
        {
            TreeNodeFactory.Register(typeof(MyConfigurationNode), typeof(MyConfigurationTreeNode));
            MyConfigurationNode node = new MyConfigurationNode("test");
            ConfigurationTreeNode treeNode = TreeNodeFactory.Create(node);
            Assert.AreEqual(typeof(MyConfigurationTreeNode), treeNode.GetType());
        }

        [Test]
        public void AddNode()
        {
            TreeNodeFactory.Register(typeof(MyConfigurationNode), typeof(MyConfigurationTreeNode));
            MockConfigurationTreeNode treeNode = new MockConfigurationTreeNode("test");
            MyConfigurationNode childNode = new MyConfigurationNode("test");
            treeNode.ConfigurationNode.Nodes.Add(childNode);
            Assert.AreEqual(1, treeNode.Nodes.Count);
            ConfigurationTreeNode childTreeNode = treeNode.Nodes[0] as ConfigurationTreeNode;
            Assert.AreSame(childTreeNode.ConfigurationNode, childNode);
            Assert.AreEqual(typeof(MyConfigurationTreeNode), childTreeNode.GetType());
        }

    }
}

#endif