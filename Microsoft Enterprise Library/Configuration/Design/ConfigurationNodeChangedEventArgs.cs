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
    /// Represents the method that handles a 
    /// <seealso cref="ConfigurationNode.Changed"/>,
    /// <seealso cref="ConfigurationNode.ChildAdded"/>,
    /// <seealso cref="ConfigurationNode.ChildRemoved"/>,
    /// <seealso cref="ConfigurationNode.ChildInserted"/>,
    /// <seealso cref="ConfigurationNode.Removed"/>, or
    /// <seealso cref="ConfigurationNode.Renamed"/> event.
    /// </summary>
    /// <param name="sender">
    /// <para>The source of the event.</para>
    /// </param>
    /// <param name="e">
    /// <para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para>
    /// </param>
    public delegate void ConfigurationNodeChangedEventHandler(object sender, ConfigurationNodeChangedEventArgs e);

    /// <summary>
    /// <para>
    /// Provides data for the <see cref="ConfigurationNode.Changed"/>,
    /// <seealso cref="ConfigurationNode.ChildAdded"/>,
    /// <seealso cref="ConfigurationNode.ChildRemoved"/>,
    /// <seealso cref="ConfigurationNode.ChildInserted"/>,
    /// <seealso cref="ConfigurationNode.Removed"/>, and
    /// <seealso cref="ConfigurationNode.Renamed"/> events.
    /// </para>
    /// </summary>    
    public class ConfigurationNodeChangedEventArgs : EventArgs
    {
        private ConfigurationNodeChangedAction action;
        private ConfigurationNode node;
        private ConfigurationNode parent;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationNodeChangedEventArgs"/> class with an action, the node it was performed upon, and the parent node.</para>
        /// </summary>
        /// <param name="action">
        /// <para>One of the <see cref="ConfigurationNodeChangedAction"/> values.</para>
        /// </param>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> that the action occured upon.</para>
        /// </param>
        /// <param name="parent"><para>The parent node of the <paramref name="node"/>.</para></param>
        public ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction action,
                                                 ConfigurationNode node,
                                                 ConfigurationNode parent)
        {
            this.action = action;
            this.node = node;
            this.parent = parent;
        }

        /// <summary>
        /// <para>Gets a value indicating the action for the <seealso cref="Node"/>.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="ConfigurationNodeChangedAction"/> values.</para>
        /// </value>
        public ConfigurationNodeChangedAction Action
        {
            get { return this.action; }
        }

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationNode"/> for the action.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ConfigurationNode"/> for the action.</para>
        /// </value>
        public ConfigurationNode Node
        {
            get { return this.node; }
        }

        /// <summary>
        /// <para>Gets the parent node of the <seealso cref="Node"/> object.</para>
        /// </summary>
        /// <value>
        /// <para>The parent node of the <seealso cref="Node"/> object.</para>
        /// </value>
        public ConfigurationNode ParentNode
        {
            get { return this.parent; }
        }
    }
}