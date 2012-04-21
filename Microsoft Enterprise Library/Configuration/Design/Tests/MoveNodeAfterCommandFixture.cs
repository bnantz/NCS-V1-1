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
    public class MoveNodeAfterCommandFixture : ConfigurationDesignHostTestBase
    {
        private MyTestNode nodeA;
        private MyTestNode nodeB;
        private MyTestNode nodeC;

        public override void SetUp()
        {
            base.SetUp();
            nodeA = new MyTestNode();
            nodeB = new MyTestNode();
            nodeC = new MyTestNode();
            GeneratedApplicationNode.AddNode(nodeA);
            GeneratedApplicationNode.AddNode(nodeB);
            GeneratedApplicationNode.AddNode(nodeC);
        }

        [Test]
        public void MoveNodeDownFromTopMovesNodeToNextSibling()
        {
            MoveNodeAfterCommand cmd = new MoveNodeAfterCommand(Host, nodeA, nodeB);
            cmd.Execute(GeneratedApplicationNode);
            Assert.AreSame(GeneratedApplicationNode.FirstNode, nodeB);
            Assert.AreSame(GeneratedApplicationNode.FirstNode.NextSibling, nodeA);
        }

        [Test]
        public void MoveNodeDownFromBottomDoesNotMoveNode()
        {
            MoveNodeAfterCommand cmd = new MoveNodeAfterCommand(Host, nodeC, nodeC);
            cmd.Execute(GeneratedApplicationNode);
            Assert.AreSame(GeneratedApplicationNode.LastNode, nodeC);
        }

        [Test]
        public void MoveNodeDownFomMiddlePutsNodeAtBottomAndShitsBottomNodeUp()
        {
            MoveNodeAfterCommand cmd = new MoveNodeAfterCommand(Host, nodeB, nodeC);
            cmd.Execute(GeneratedApplicationNode);
            Assert.AreSame(GeneratedApplicationNode.LastNode, nodeB);
            Assert.AreSame(GeneratedApplicationNode.LastNode.PreviousSibling, nodeC);
        }

        private class MyTestNode : ConfigurationNode
        {
        }
    }
}
#endif