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
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a entry for the <see cref="INodeCreationService"/> that contains the data to create a node.</para>
    /// </summary>
    public class NodeCreationEntry
    {
        private readonly ConfigurationNodeCommand configurationNodeCommand;
        private readonly bool allowMultiple;
        private readonly Type nodeType;
        private readonly string displayName;
        private readonly Type dataType;
        private Type baseTypeToCompare;
        

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="NodeCreationEntry"/> with a command, the node type to create, the configuration data type associated with the node, name to display for this node, and if there can be more than one node of the type.</para>
        /// </summary>
        /// <param name="configurationNodeCommand"><para>The <see cref="ConfigurationNodeCommand"/> used to create the node.</para></param>
        /// <param name="nodeType"><para>The <see cref="Type"/> of the node to create.</para></param>
        /// <param name="dataType"><para>The <see cref="Type"/> of the configuration data associated with the <paramref name="nodeType"/>.</para></param>
        /// <param name="displayName"><para>The name to display in the user interface to create the node.</para></param>
        /// <param name="allowMultiple">
        /// <para>Determines if more than one of the node type can exist.</para>
        /// </param>
        private NodeCreationEntry(ConfigurationNodeCommand configurationNodeCommand, Type nodeType, Type dataType, string displayName, bool allowMultiple)
        {
            ArgumentValidation.CheckForNullReference(configurationNodeCommand, "configurationNodeCommand");
            ArgumentValidation.CheckForNullReference(nodeType, "nodeType");
            ArgumentValidation.CheckForNullReference(dataType, "dataType");
            ArgumentValidation.CheckForNullReference(displayName, "displayName");

            this.configurationNodeCommand = configurationNodeCommand;
            this.allowMultiple = allowMultiple;
            this.nodeType = nodeType;
            if (!typeof(ConfigurationNode).IsAssignableFrom(nodeType))
            {
                throw new ArgumentException(SR.ExceptionTypeNotConfigurationNode(nodeType.FullName), "nodeType");
            }

            this.displayName = displayName;
            this.dataType = dataType;
        }

        /// <summary>
        /// <para>Create a new instance of the <see cref="NodeCreationEntry"/> that allows multiple nodes in the same parent node with a command, the node type to create, the configuration data type associated with the node and name to display for this node.</para>
        /// </summary>
        /// <param name="configurationNodeCommand"><para>The <see cref="ConfigurationNodeCommand"/> used to create the node.</para></param>
        /// <param name="nodeType"><para>The <see cref="Type"/> of the node to create.</para></param>
        /// <param name="dataType"><para>The <see cref="Type"/> of the configuration data associated with the <paramref name="nodeType"/>.</para></param>
        /// <param name="displayName"><para>The name to display in the user interface to create the node.</para></param>
        public static NodeCreationEntry CreateNodeCreationEntryWithMultiples(ConfigurationNodeCommand configurationNodeCommand, Type nodeType, Type dataType, string displayName)
        {
            return new NodeCreationEntry(configurationNodeCommand, nodeType, dataType, displayName, true);
        }

        /// <summary>
        /// <para>Create a new instance of the <see cref="NodeCreationEntry"/> that does not allows multiple nodes in the same parent node with a command, the node type to create, the configuration data type associated with the node and name to display for this node.</para>
        /// </summary>
        /// <param name="configurationNodeCommand"><para>The <see cref="ConfigurationNodeCommand"/> used to create the node.</para></param>
        /// <param name="nodeType"><para>The <see cref="Type"/> of the node to create.</para></param>
        /// <param name="dataType"><para>The <see cref="Type"/> of the configuration data associated with the <paramref name="nodeType"/>.</para></param>
        /// <param name="displayName"><para>The name to display in the user interface to create the node.</para></param>
        public static NodeCreationEntry CreateNodeCreationEntryNoMultiples(ConfigurationNodeCommand configurationNodeCommand, Type nodeType, Type dataType, string displayName)
        {
            return new NodeCreationEntry(configurationNodeCommand, nodeType, dataType, displayName, false);
        }

        /// <summary>
        /// <para>Gets the configuration data type associated with the node type.</para>
        /// </summary>
        /// <value>
        /// <para>The configuration data type associated with the node type.</para>
        /// </value>
        public Type DataType
        {
            get { return dataType; }
        }

        /// <summary>
        /// <para>Gets the node type to create.</para>
        /// </summary>
        /// <value>
        /// <para>The node type to create.</para>
        /// </value>
        public Type NodeType
        {
            get { return nodeType; }
        }

        /// <summary>
        /// <para>Gets the display name to show in the user interface that creats the node.</para>
        /// </summary>
        /// <value>
        /// <para>The display name to show in the user interface that creats the node.</para>
        /// </value>
        public string DisplayName
        {
            get { return displayName; }
        }

        /// <summary>
        /// <para>Gets the command used to create the node.</para>
        /// </summary>
        /// <value>
        /// <para>The commad used to create the node.</para>
        /// </value>
        public ConfigurationNodeCommand ConfigurationNodeCommand
        {
            get { return configurationNodeCommand; }
        }

        /// <summary>
        /// <para>Determines if more than one of the node type can exist.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if more than one of the node type can exist; otherwise, <see langword="false"/>.</para>
        /// </value>
        public bool AllowMultiple
        {
            get { return allowMultiple; }
        }

        /// <summary>
        /// <para>Gets or sets the base type of the node type that will be compared to determine if the type can allow multiple.</para>
        /// </summary>
        /// <value>
        /// <para>The base type of the node type that will be compared to determine if the type can allow multiple.</para>
        /// </value>
        internal Type BaseTypeToCompare
        {
            get { return baseTypeToCompare; }
            set { baseTypeToCompare = value; }
        }
    }
}
