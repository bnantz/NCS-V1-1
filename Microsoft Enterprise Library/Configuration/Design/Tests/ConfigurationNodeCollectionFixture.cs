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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationNodeCollectionFixture : ConfigurationDesignHostTestBase
    {
        private RootNode rootNode;

        public override void SetUp()
        {
            base.SetUp();
            rootNode = new RootNode();
            GeneratedApplicationNode.Nodes.Add(rootNode);
        }

        [Test]
        public void AddTest()
        {
            MyNode node = new MyNode();
            rootNode.Nodes.Add(node);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            Assert.AreSame(node, rootNode.Nodes[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullTest()
        {
            rootNode.Nodes.Add(null);
        }

        [Test]
        public void AddRangeTest()
        {
            MyNode[] localNodes = new MyNode[] {new MyNode(), new MyNode()};
            rootNode.Nodes.AddRange(localNodes);
            Assert.AreEqual(2, rootNode.Nodes.Count);
        }

        [Test]
        public void ContainsTest()
        {
            MyNode[] localNodes = new MyNode[] {new MyNode(), new MyNode()};
            rootNode.Nodes.AddRange(localNodes);
            Assert.AreEqual(2, rootNode.Nodes.Count);
            Assert.AreEqual(true, rootNode.Nodes.Contains(localNodes[0]));
            Assert.AreEqual(true, rootNode.Nodes.Contains(localNodes[1]));
            Assert.AreEqual(false, rootNode.Nodes.Contains(new MyNode()));
        }

        [Test]
        public void IndexOfTest()
        {
            MyNode[] localNodes = new MyNode[] {new MyNode(), new MyNode()};
            rootNode.Nodes.AddRange(localNodes);
            Assert.AreEqual(2, rootNode.Nodes.Count);
            int index = rootNode.Nodes.IndexOf(localNodes[1]);
            Assert.AreEqual(localNodes[1].Index, index);
        }

        [Test]
        public void InsertTest()
        {
            MyNode[] localNodes = new MyNode[] {new MyNode(), new MyNode()};
            rootNode.Nodes.AddRange(localNodes);
            Assert.AreEqual(2, rootNode.Nodes.Count);
            rootNode.Nodes.Insert(1, new MyNode());
            Assert.AreEqual(3, rootNode.Nodes.Count);
            int index = rootNode.Nodes.IndexOf(localNodes[1]);
            Assert.AreEqual(localNodes[1].Index, index);
        }

        [Test]
        public void ClearTest()
        {
            MyNode[] localNodes = new MyNode[] {new MyNode(), new MyNode()};
            rootNode.Nodes.AddRange(localNodes);
            Assert.AreEqual(2, rootNode.Nodes.Count);
            rootNode.Nodes.Clear();
            Assert.AreEqual(0, rootNode.Nodes.Count);
        }

        [Test]
        public void RemoveTest()
        {
            MyNode[] localNodes = new MyNode[] {new MyNode(), new MyNode()};
            rootNode.Nodes.AddRange(localNodes);
            Assert.AreEqual(2, rootNode.Nodes.Count);
            rootNode.Nodes.Remove(localNodes[0]);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            Assert.IsFalse(rootNode.Nodes.Contains(localNodes[0]));
        }

        [Test]
        public void RemoveAtTest()
        {
            MyNode[] localNodes = new MyNode[] {new MyNode(), new MyNode()};
            rootNode.Nodes.AddRange(localNodes);
            Assert.AreEqual(2, rootNode.Nodes.Count);
            rootNode.Nodes.RemoveAt(localNodes[0].Index);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            Assert.IsFalse(rootNode.Nodes.Contains(localNodes[0]));
        }

        private class RootNode : ConfigurationNode
        {
            public RootNode() : base()
            {
            }
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