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
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents an individual item that is displayed within the main menu or context menu of the user interface.</para>
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class ConfigurationMenuItem : MenuItem
    {
        private string statusBarText;
        private InsertionPoint insertionPoint;
        private ConfigurationNodeCommand command;
        private ConfigurationNode node;

        private ConfigurationMenuItem()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the class with a specified caption, event handler, associated shortcut key, icon, status bar text, and the insertion point for the menu item.</para>
        /// </summary>
        /// <param name="text">
        /// <para>The caption for the menu item.</para>
        /// </param>
        /// <param name="command">
        /// <para>The <see cref="ConfigurationNodeCommand"/> to execute.</para>
        /// </param>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to execute the command upon.</para>
        /// </param>
        /// <param name="shortcut">
        /// <para>One of the <see cref="Shortcut"/> values.</para>
        /// </param>
        /// <param name="statusBarText">
        /// <para>The text for the status bar.</para>
        /// </param>
        /// <param name="insertionPoint">
        /// <para>One of the <see cref="InsertionPoint"/> values.</para>
        /// </param>
        public ConfigurationMenuItem(string text, ConfigurationNodeCommand command, ConfigurationNode node, Shortcut shortcut, string statusBarText, InsertionPoint insertionPoint) : base(text, null, shortcut)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            this.statusBarText = statusBarText;
            this.insertionPoint = insertionPoint;
            this.command = command;
            this.node = node;
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (command != null)
                {
                    command.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <para>Create a menu item for the <see cref="RemoveNodeCommand"/> for the <paramref name="node"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to associate with the menu.</para>
        /// </param>
        /// <returns>
        /// <para>An instance of the <see cref="RemoveNodeCommand"/> class.</para>
        /// </returns>
        public static ConfigurationMenuItem CreateRemoveNodeCommand(IServiceProvider serviceProvider, ConfigurationNode node)
        {
            return new ConfigurationMenuItem(SR.RemoveNodeMenuItemText, new RemoveNodeCommand(serviceProvider), node, Shortcut.None, SR.RemoveNodeMenuItemStatusText, InsertionPoint.Action);
        }

        /// <summary>
        /// <para>Create a menu item for the <see cref="ValidateNodeCommand"/> for the <paramref name="node"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to associate with the menu.</para>
        /// </param>
        /// <returns>
        /// <para>An instance of the <see cref="ValidateNodeCommand"/> class.</para>
        /// </returns>
        public static ConfigurationMenuItem CreateValidateNodeCommand(IServiceProvider serviceProvider, ConfigurationNode node)
        {
            return new ConfigurationMenuItem(SR.ValidateNodeMenuItemText, new ValidateNodeCommand(serviceProvider), node, Shortcut.None, SR.ValidateNodeMenuItemStatusText, InsertionPoint.Action);
        }

        /// <summary>
        /// <para>Create a menu item for the <see cref="AddChildNodeCommand"/> for the <paramref name="node"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to associate with the menu.</para>
        /// </param>
        /// <param name="t">
        /// <para>The type of node to create.</para>
        /// </param>
        /// <param name="menuText">
        /// <para>The text to display in the menu item.</para>
        /// </param>
        /// <param name="allowMultiple">
        /// <para>Allow multiple of the same node typ for the parent <paramref name="node"/>.</para>
        /// </param>
        /// <returns>
        /// <para>An instance of the <see cref="AddChildNodeCommand"/> class.</para>
        /// </returns>
        public static ConfigurationMenuItem CreateAddChildNodeMenuItem(IServiceProvider serviceProvider, ConfigurationNode node, Type t, string menuText, bool allowMultiple)
        {
            ConfigurationMenuItem item = new ConfigurationMenuItem(menuText, new AddChildNodeCommand(serviceProvider, t), node, Shortcut.None, SR.GenericCreateStatusText(menuText), InsertionPoint.New);
            if (!allowMultiple)
            {
                if (node.Hierarchy.ContainsNodeType(node, t))
                {
                    item.Enabled = false;
                }
            }
            return item;

        }

        /// <summary>
        /// <para>Gets or sets the status bar text associated with this menu item.</para>
        /// </summary>
        /// <value>
        /// <para>The status bar text associated with this menu item.</para>
        /// </value>
        public string StatusBarText
        {
            get { return statusBarText; }
            set { statusBarText = value; }
        }

        /// <summary>
        /// <para>Gets or sets the insertion point for the menu item.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="InsertionPoint"/> values.</para>
        /// </value>
        public InsertionPoint InsertionPoint
        {
            get { return insertionPoint; }
            set { insertionPoint = value; }
        }

        /// <summary>
        /// <para>Gets or sets the commad to run for this menu item.</para>
        /// </summary>
        /// <value>
        /// <para>The commad to run for this menu item.</para>
        /// </value>
        [Browsable(false)]
        public ConfigurationNodeCommand Command
        {
            get { return command; }
            set { command = value; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="ConfigurationNode"/> for the command.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ConfigurationNode"/> for the command </para>
        /// </value>
        [Browsable(false)]
        public ConfigurationNode Node
        {
            get { return node; }
            set { node = value; }
        }

        /// <summary>
        /// <para>Creates a copy of the current <see cref="ConfigurationMenuItem"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>A <see cref="ConfigurationMenuItem"/> that represents the duplicated menu item.</para>
        /// </returns>
        public override MenuItem CloneMenu()
        {
            ConfigurationMenuItem item = new ConfigurationMenuItem();
            item.CloneMenu(this);
            item.StatusBarText = this.statusBarText;
            item.InsertionPoint = this.insertionPoint;
            item.Node = this.node;
            item.Command = this.command;
            return item;
        }

        /// <summary>
        /// <para>Raises the <see cref="MenuItem.Click"/> event and executes the <see cref="ConfigurationNodeCommand"/>.</para>
        /// </summary>
        /// <param name="e">
        /// <para>An <see cref="EventArgs"/> that contains the event data.</para>
        /// </param>
        protected override void OnClick(EventArgs e)
        {
            using (new WaitCursor())
            {
                base.OnClick(e);
                command.Execute(node);
            }
        }

    }
}