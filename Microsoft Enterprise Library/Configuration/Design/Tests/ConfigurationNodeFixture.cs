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
using System.IO;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationNodeFixture : ConfigurationDesignHostTestBase
    {
        private static readonly string labelName = "Test";
        private TestConfigurationNode node;
        private TestConfigurationNode workingNode;
        private int addChildEventCount;
        private int removeChildEventCount;
        private int renamedEventCount;
        private int removeEventCount;
        private int changedEventCount;

        public override void SetUp()
        {
            base.SetUp();
            node = new TestConfigurationNode(labelName);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            addChildEventCount = 0;
            removeChildEventCount = 0;
            renamedEventCount = 0;
            removeEventCount = 0;
            changedEventCount = 0;
        }

        public override void TearDown()
        {
            node.Nodes.Clear();
            workingNode = null;
            base.TearDown();
        }

        [Test]
        public void DisplayNameTest()
        {
            Assert.AreEqual(labelName, node.Name);
        }

        [Test]
        public void GetUniqueDisplayNameTest()
        {
            node.Nodes.Add(new TestConfigurationNode("Test"));
            int index = node.AddNode(new TestConfigurationNode("Test"));
            TestConfigurationNode newNode = node.Nodes[index] as TestConfigurationNode;
            Assert.AreEqual("Test1", newNode.Name);
            node.Nodes.Clear();
        }

        [Test]
        public void RenameSameNameTest()
        {
            node.Nodes.Add(new TestConfigurationNode("Test"));
            int index = node.AddNode(new TestConfigurationNode("Test"));
            workingNode = node.Nodes[index] as TestConfigurationNode;
            workingNode.Name = "Test3";
            Assert.AreEqual("Test3", workingNode.Name);
        }

        [Test]
        public void RenameTest()
        {
            node.Nodes.Add(new TestConfigurationNode("Test"));
            int index = node.AddNode(new TestConfigurationNode("Test"));
            workingNode = node.Nodes[index] as TestConfigurationNode;
            workingNode.Renamed += new ConfigurationNodeChangedEventHandler(NodeRenamed);
            workingNode.Name = "MyTest";
            Assert.AreEqual("MyTest", workingNode.Name);
            Assert.AreEqual(1, renamedEventCount);
            workingNode.Renamed -= new ConfigurationNodeChangedEventHandler(NodeRenamed);
        }

        [Test]
        public void ChildAddedTest()
        {
            node.ChildAdded += new ConfigurationNodeChangedEventHandler(NodeChildAdded);
            workingNode = new TestConfigurationNode("New Node");
            node.AddNode(workingNode);
            Assert.AreEqual(1, addChildEventCount);
            node.ChildAdded -= new ConfigurationNodeChangedEventHandler(NodeChildAdded);
        }

        [Test]
        public void ChildRemovedTest()
        {
            node.ChildRemoved += new ConfigurationNodeChangedEventHandler(NodeChildRemoved);
            workingNode = new TestConfigurationNode("New Node");
            node.AddNode(workingNode);
            workingNode.Remove();
            Assert.AreEqual(1, removeChildEventCount);
            node.ChildRemoved -= new ConfigurationNodeChangedEventHandler(NodeChildRemoved);
        }

        public void NodeChildRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            Assert.AreEqual(ConfigurationNodeChangedAction.Remove, e.Action);
            Assert.AreSame(null, e.ParentNode);
            Assert.AreSame(workingNode, e.Node);
            removeChildEventCount++;
        }

        [Test]
        public void RemoveNodeLeavesOnlyOneNodeInChildNodeArray()
        {
            TestNode node = new TestNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            TestNode nodeA = new TestNode();
            TestNode nodeB = new TestNode();
            node.Nodes.Add(nodeA);
            node.Nodes.Add(nodeB);
            nodeA.Remove();
            
            Assert.AreEqual(1, node.ChildCount);
        }

        [Test]
        public void RemovedTest()
        {
            workingNode = new TestConfigurationNode("New Node");
            node.AddNode(workingNode);
            workingNode.Removed += new ConfigurationNodeChangedEventHandler(WorkingNodeRemoved);
            workingNode.Remove();
            Assert.IsFalse(node.Nodes.Contains(workingNode));
            ConfigurationNode match = null;
            foreach (ConfigurationNode childNode in node.Nodes)
            {
                if (childNode == workingNode)
                {
                    match = childNode;
                }
            }
            Assert.IsNull(match);
            Assert.AreEqual(1, removeEventCount);
            workingNode.Removed -= new ConfigurationNodeChangedEventHandler(WorkingNodeRemoved);
        }

        public void WorkingNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            Assert.AreEqual(ConfigurationNodeChangedAction.Remove, e.Action);
            Assert.AreSame(null, e.ParentNode);
            Assert.AreSame(workingNode, e.Node);
            removeEventCount++;
        }

        [Test]
        public void PathTest()
        {
            string path = labelName + Path.AltDirectorySeparatorChar + "A" + Path.AltDirectorySeparatorChar + "B" + Path.AltDirectorySeparatorChar + "C";
            int[] indexes = new int[3];
            indexes[0] = node.AddNode(new TestConfigurationNode("A"));
            indexes[1] = node.ChildNodes[indexes[0]].AddNode(new TestConfigurationNode("B"));
            indexes[2] = node.ChildNodes[indexes[0]].ChildNodes[indexes[1]].AddNode(new TestConfigurationNode("C"));
            Assert.AreEqual(path, node.ChildNodes[indexes[0]].ChildNodes[indexes[1]].ChildNodes[indexes[2]].Path);
            node.ChildNodes[indexes[0]].Remove();
        }

        [Test]
        public void ChangedTest()
        {
            workingNode = new TestConfigurationNode("New Node");
            CreateHierarchyAndAddToHierarchyService(workingNode, CreateDefaultConfiguration());
            workingNode.Changed += new ConfigurationNodeChangedEventHandler(WorkingNodeChanged);
            workingNode.Name = "New Name";
            Assert.AreEqual(1, changedEventCount);
            workingNode.Changed -= new ConfigurationNodeChangedEventHandler(WorkingNodeChanged);
        }

        private void WorkingNodeChanged(object sender, ConfigurationNodeChangedEventArgs e)
        {
            Assert.AreEqual(ConfigurationNodeChangedAction.Changed, e.Action);
            Assert.AreSame(null, e.ParentNode);
            Assert.AreSame(workingNode, e.Node);
            changedEventCount++;
        }

        [Test]
        public void NextSiblingTest()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            CreateHierarchyAndAddToHierarchyService(testNode, CreateDefaultConfiguration());
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            ConfigurationNode firstNode = testNode.ChildNodes[0];
            Assert.AreEqual("ATest", firstNode.NextSibling.Name);
        }

        [Test]
        public void PreviousSiblingTest()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            CreateHierarchyAndAddToHierarchyService(testNode, CreateDefaultConfiguration());
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            ConfigurationNode lastNode = testNode.ChildNodes[2];
            Assert.AreEqual("ATest", lastNode.PreviousSibling.Name);
        }

        [Test]
        public void MoveAfterTest()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            CreateHierarchyAndAddToHierarchyService(testNode, CreateDefaultConfiguration());
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            ConfigurationNode nodeA = testNode.ChildNodes[1];
            testNode.MoveAfter(testNode.ChildNodes[0], nodeA);
            Assert.AreEqual("BTest", nodeA.NextSibling.Name);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveNodeAfterWithNullSiblingNode()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            CreateHierarchyAndAddToHierarchyService(testNode, CreateDefaultConfiguration());
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            testNode.MoveAfter(testNode.ChildNodes[0], null);
        }

        [Test]
        public void MoveBeforeTest()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            CreateHierarchyAndAddToHierarchyService(testNode, CreateDefaultConfiguration());
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            ConfigurationNode nodeA = testNode.ChildNodes[1];
            testNode.MoveBefore(testNode.ChildNodes[2], nodeA);
            Assert.AreEqual("CTest", nodeA.PreviousSibling.Name);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveBeforeTestWithNullSiblingNode()
        {
            TestConfigurationNode testNode = new TestConfigurationNode("Test");
            CreateHierarchyAndAddToHierarchyService(testNode, CreateDefaultConfiguration());
            testNode.AddNode(new TestConfigurationNode("BTest"));
            testNode.AddNode(new TestConfigurationNode("ATest"));
            testNode.AddNode(new TestConfigurationNode("CTest"));
            testNode.MoveBefore(testNode.ChildNodes[2], null);
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(node.Name, node.ToString());
        }

        [Test]
        public void FirstNodeTest()
        {
            TestConfigurationNode node = new TestConfigurationNode("test");
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            TestConfigurationNode firstChild = new TestConfigurationNode("firstchild");
            node.Nodes.Add(firstChild);
            Assert.AreSame(firstChild, node.FirstNode);
        }

        [Test]
        public void LastNodeTest()
        {
            TestConfigurationNode node = new TestConfigurationNode("test");
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            TestConfigurationNode firstChild = new TestConfigurationNode("firstchild");
            TestConfigurationNode lastChild = new TestConfigurationNode("lastchild");
            node.Nodes.Add(firstChild);
            node.Nodes.Add(lastChild);
            Assert.AreSame(lastChild, node.LastNode);
        }

        public void NodeChildAdded(object sender, ConfigurationNodeChangedEventArgs e)
        {
            Assert.AreEqual(ConfigurationNodeChangedAction.Insert, e.Action);
            Assert.AreSame(node, e.ParentNode);
            Assert.AreSame(workingNode, e.Node);
            addChildEventCount++;
        }

        private void NodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            Assert.AreEqual(ConfigurationNodeChangedAction.Rename, e.Action);
            Assert.AreSame(node, e.ParentNode);
            Assert.AreSame(workingNode, e.Node);
            renamedEventCount++;
        }

        private class TestConfigurationNode : ConfigurationNode
        {
            private string name;

            public TestConfigurationNode(string name) : base()
            {
                this.name = name;
            }

            public override bool SortChildren
            {
                get
                {
                    return false;
                }
            }


            protected override void OnSited()
            {
                base.OnSited();
                Site.Name = name;
            }
        }
    }
}

#endif