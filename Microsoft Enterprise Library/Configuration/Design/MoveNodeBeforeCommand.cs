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

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a command that will move a <see cref="ConfigurationNode"/> before a given <see cref="ConfigurationNode"/>.</para>
    /// </summary>
    public class MoveNodeBeforeCommand : ConfigurationNodeCommand
    {
        private readonly ConfigurationNode nodeToMove;
        private readonly ConfigurationNode siblingNode;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="MoveNodeBeforeCommand"/> class with an <see cref="IServiceProvider"/>, the node to move, and the sibling node to move it before.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="nodeToMove">
        /// <para>The node to move.</para>
        /// </param>
        /// <param name="siblingNode">
        /// <para>The sibling node to move <paramref name="nodeToMove"/> before.</para>
        /// </param>
        public MoveNodeBeforeCommand(IServiceProvider serviceProvider, ConfigurationNode nodeToMove, ConfigurationNode siblingNode) : this(serviceProvider, false, nodeToMove, siblingNode)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="MoveNodeBeforeCommand"/> class with an <see cref="IServiceProvider"/>, the node to move, and the sibling node to move it before.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorLog">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        /// <param name="nodeToMove">
        /// <para>The node to move.</para>
        /// </param>
        /// <param name="siblingNode">
        /// <para>The sibling node to move <paramref name="nodeToMove"/> before.</para>
        /// </param>
        public MoveNodeBeforeCommand(IServiceProvider serviceProvider, bool clearErrorLog, ConfigurationNode nodeToMove, ConfigurationNode siblingNode) : base(serviceProvider, clearErrorLog)
        {
            if (nodeToMove == null) throw new ArgumentNullException("nodeToMove");
            if (siblingNode == null) throw new ArgumentNullException("siblingNode");

            this.nodeToMove = nodeToMove;
            this.siblingNode = siblingNode;
        }

        /// <summary>
        /// <para>Executes the moving the node before it's sibling.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to execute the command upon.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            node.MoveBefore(nodeToMove, siblingNode);
        }
    }
}
