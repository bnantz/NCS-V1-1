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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationNodeChangedEventArgsFixture : ConfigurationDesignHostTestBase
    {
        private const ConfigurationNodeChangedAction Action = ConfigurationNodeChangedAction.Custom;
        private ConfigurationNodeChangedEventArgs e;

        private MyNode parent;
        private MyNode child;

        public override void SetUp()
        {
            base.SetUp();
            parent = new MyNode();
            GeneratedApplicationNode.Nodes.Add(parent);
            parent.Name = "node";
            child = new MyNode();
            parent.Nodes.Add(child);
            parent.Name = "child";
            e = new ConfigurationNodeChangedEventArgs(Action, child, child.Parent);
        }

        [Test]
        public void NodeTest()
        {
            Assert.AreSame(child, e.Node);
        }

        [Test]
        public void ParentTest()
        {
            Assert.AreSame(parent, e.ParentNode);
            Assert.AreSame(child.Parent, e.ParentNode);
        }

        [Test]
        public void ActionTest()
        {
            Assert.AreEqual(Action, e.Action);
        }

        private class MyNode : ConfigurationNode
        {
            public MyNode() : base()
            {
            }
        }

    }
}

#endif