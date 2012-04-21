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
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a service for creating nodes based on data types.</para>
    /// </summary>
    public interface INodeCreationService
    {
        /// <summary>
        /// <para>When implemented by a class, gets the names for the base node type that previous nodes were registered.</para>
        /// </summary>
        /// <param name="nodeType">
        /// <para>The type of node to get the names.</para>
        /// </param>
        /// <returns>
        /// <para>A collection of names for the nod type.</para>
        /// </returns>
        StringCollection GetDisplayNames(Type nodeType);

        /// <summary>
        /// <para>When implemented by a class, gets the <see cref="NodeCreationEntry"/> for the <paramref name="displayName"/>.</para>
        /// </summary>
        /// <param name="displayName">
        /// <para>The name used when displaying a command in the UI and used when adding the <see cref="NodeCreationEntry"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="NodeCreationEntry"/> of node to create for the <parmref name="displayName"/>.</para>
        /// </returns>
        NodeCreationEntry GetNodeCreationEntry(string displayName);

        /// <summary>
        /// <para>When implemented by a class, creates a <see cref="ConfigurationNode"/> based on the data type.</para>
        /// </summary>
        /// <param name="dataType">
        /// <para>The data type to base the creation upon.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="ConfigurationNode"/> based on the data type or <see langword="null"/> if one does not exists.</para>
        /// </returns>
        ConfigurationNode CreateNode(Type dataType);

        /// <summary>
        /// <para>When implemented by a class, creates a <see cref="ConfigurationNode"/> based on the data type.</para>
        /// </summary>
        /// <param name="dataType">
        /// <para>The data type to base the creation upon.</para>
        /// </param>
        /// <param name="constructorArguments">
        /// <para>The constructor arguments for the node.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="ConfigurationNode"/> based on the data type or <see langword="null"/> if one does not exists.</para>
        /// </returns>
        ConfigurationNode CreateNode(Type dataType, object[] constructorArguments);
        
        /// <summary>
        /// <para>When implemented by a class, creates a map between the node and the data that it represents.</para>
        /// </summary>
        /// <param name="nodeCreationEntry">
        /// <para>A <see cref="NodeCreationEntry"/> object.</para>
        /// </param>
        void AddNodeCreationEntry(NodeCreationEntry nodeCreationEntry);
        
        /// <summary>
        /// <para>When implemented by a class, determines if a node type matches it's registered data type.</para>
        /// </summary>
        /// <param name="nodeType"><para>The node type to compare.</para></param>
        /// <param name="dataType"><para>The data type to compare.</para></param>
        /// <returns><para><see langword="true"/> if the node type matches the registered data type; otherwise, <see langword="false"/>.</para></returns>
        bool DoesNodeTypeMatchDataType(Type nodeType, Type dataType);

        /// <summary>
        /// <para>When implemented by a class, gets the node type for the given data type.</para>
        /// </summary>
        /// <param name="dataType"><para>The data type that was registered for the node type.</para></param>
        /// <returns>
        /// <para>The node type for the data type or <see langword="null"/> (Nothing in Visual Basic) if the type does not exist.</para>
        /// </returns>
        Type GetNodeType(Type dataType);
    }
}