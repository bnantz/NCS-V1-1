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
    public class MoveNodeBeforeCommandFixture : ConfigurationDesignHostTestBase
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
        public void MoveNodeUpFromMiddleMovesNodeToTopAndTopToMiddle()
        {
            MoveNodeBeforeCommand cmd = new MoveNodeBeforeCommand(Host, nodeB, nodeA);
            cmd.Execute(GeneratedApplicationNode);
            Assert.AreSame(GeneratedApplicationNode.FirstNode, nodeB);
        }

        [Test]
        public void MoveNodeUpFromTopDoesNotMoveNode()
        {
            MoveNodeBeforeCommand cmd = new MoveNodeBeforeCommand(Host, nodeA, nodeA);
            cmd.Execute(GeneratedApplicationNode);
            Assert.AreSame(GeneratedApplicationNode.FirstNode, nodeA);
        }

        [Test]
        public void MoveNodeUpFromBottomPutsNodeInMiddleAndShitsMiddleNodeToBottom()
        {
            MoveNodeBeforeCommand cmd = new MoveNodeBeforeCommand(Host, nodeC, nodeB);
            cmd.Execute(GeneratedApplicationNode);
            Assert.AreSame(GeneratedApplicationNode.LastNode, nodeB);
        }

        private class MyTestNode : ConfigurationNode
        {
        }
    }
}
#endif