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
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents a command that adds a <see cref="ConfigurationNode"/> as a child of the  <see cref="ConfigurationNode"/> that this command is executing upon.
    /// </para>
    /// </summary>
    public class AddChildNodeCommand : ConfigurationNodeCommand
    {
        private Type childType;
        private ConfigurationNode childNode;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="AddChildNodeCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="childType">
        /// <para>
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.
        /// </para>
        /// </param>
        public AddChildNodeCommand(IServiceProvider serviceProvider, Type childType) : this(serviceProvider, true, childType)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="AddChildNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorService">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        /// <param name="childType">
        /// <para>
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.
        /// </para>
        /// </param>
        public AddChildNodeCommand(IServiceProvider serviceProvider, bool clearErrorService, Type childType) : base(serviceProvider, clearErrorService)
        {
            if (childType == null)
            {
                throw new ArgumentNullException("childType");
            }
            this.childType = childType;
        }

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationNode"/> that was added as a result of the command being executed.</para>
        /// </summary>
        /// <value>
        /// <para>
        /// The <see cref="ConfigurationNode"/> that was added as a result of the command being executed. 
        /// The default is a <see langword="null"/> reference(Nothing in Visual Basic).
        /// </para>
        /// </value>
        public ConfigurationNode ChildNode
        {
            get { return childNode; }
        }

        /// <summary>
        /// <para>Creates an instance of the child node class and adds it as a child of the parent node.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The parent node to add the newly created <see cref="ChildNode"/>.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                childNode = CreateChild();
                node.Nodes.AddWithDefaultChildren(childNode);
                UIService.SetUIDirty(node.Hierarchy);
                UIService.ActivateNode(childNode);
                AddXmlIncludeTypes(node);
            }
            finally
            {
                UIService.EndUpdate();
            }
        }

        private void AddXmlIncludeTypes(ConfigurationNode node)
        {
            using(AddXmlIncludeTypesCommand includeTypesCommand = new AddXmlIncludeTypesCommand(ServiceProvider, false, childNode.GetType()))
            {
                includeTypesCommand.Execute(node);
            }
        }

        private ConfigurationNode CreateChild()
        {
            return (ConfigurationNode)Activator.CreateInstance(childType,
                                                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.CreateInstance,
                                                               null,
                                                               null,
                                                               null);
        }
    }
}