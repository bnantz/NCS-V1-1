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
    /// <para>Provides hierarchy management for configuration design managers that implement hierarchies.</para>
    /// </summary>
    public interface IUIHierarchy
    {
        /// <summary>
        /// <para>When implemented by a class, occurs after the hierarchy is saved.</para>
        /// </summary>
        event HierarchySavedEventHandler Saved;

        /// <summary>
        /// <para>When implemented by a class, gets a unique id for the hierarchy.</para>
        /// </summary>
        /// <value>
        /// <para>A unique id for the hierarchy.</para>
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// <para>When implemented by a class, gets or sets the root node for the hiearchy.</para>
        /// </summary>
        /// <value>
        /// <para>The root node for the hiearchy.</para>
        /// </value>
        ConfigurationNode RootNode { get; set; }

        /// <summary>
        /// <para>When implemented by a class, gets or sets the currently selected node in the hierarchy.</para>
        /// </summary>
        /// <value>
        /// <para>The currently selected node in the hierarchy</para>
        /// </value>
        ConfigurationNode SelectedNode { get; set; }

        /// <summary>
        /// <para>When implemented by a class, gets the <see cref="ConfigurationContext"/> used for this hierarchy.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ConfigurationContext"/> used for this hierarchy.</para>
        /// </value>
        ConfigurationContext ConfigurationContext { get; set; }

        /// <summary>
        /// <para>When implmented by a class, gets the <see cref="IStorageTable"/> for the current heirarchy.</para>
        /// </summary>
        /// <value>
        /// <para>
        /// Gets the <see cref="IStorageTable"/> for the current heirarchy.
        /// </para>
        /// </value>
        IStorageTable StorageTable { get; }

        /// <summary>
        /// <para>When implemented by a class, finds a node via it's path.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the node.</para>
        /// </param>
        /// <returns>
        /// <para>The node if found or <see langword="null"/> if not found.</para>
        /// </returns>
        /// <remarks>
        /// <para>Use the <seealso cref="ConfigurationNode.Path"/> property get the path to the node.</para>
        /// </remarks>
        ConfigurationNode FindNodeByPath(string path);

        /// <summary>
        /// <para>When implemented by a class, finds nodes by their <see cref="Type"/>.</para>
        /// </summary>
        /// <param name="type">
        /// <para>The <see cref="Type"/> of the node.</para>
        /// </param>
        /// <returns>
        /// <para>The nodes found.</para>
        /// </returns>
        ConfigurationNode[] FindNodesByType(Type type);

        /// <summary>
        /// <para>When implemented by a class, finds nodes by their <see cref="Type"/>.</para>
        /// </summary>
        /// <param name="parent">
        /// <para>The parent to start the search.</para>
        /// </param>
        /// <param name="type">
        /// <para>The <see cref="Type"/> of the node.</para>
        /// </param>
        /// <returns>
        /// <para>The nodes found.</para>
        /// </returns>
        /// <remarks>
        /// If this is more than one type in the hierachy, this function will only find the first one found.
        /// </remarks>
        ConfigurationNode[] FindNodesByType(ConfigurationNode parent, Type type);

        /// <summary>
        /// <para>When implemented by a class, finds a node by it's <see cref="Type"/>.</para>
        /// </summary>
        /// <param name="type">
        /// <para>The <see cref="Type"/> of the node.</para>
        /// </param>
        /// <returns>
        /// <para>The node if found or <see langword="null"/> if not found.</para>
        /// </returns>
        /// <remarks>
        /// <para>If this is more than one type in the hierachy, this function will only find the first one found.</para>
        /// </remarks>
        ConfigurationNode FindNodeByType(Type type);

        /// <summary>
        /// <para>When implemented by a class, finds nodes by their <see cref="Type"/>.</para>
        /// </summary>
        /// <param name="parent">
        /// <para>The parent to start the search.</para>
        /// </param>
        /// <param name="type">
        /// <para>The <see cref="Type"/> of the node.</para>
        /// </param>
        /// <returns>
        /// <para>The node if found or <see langword="null"/> if not found.</para>
        /// </returns>
        /// <remarks>
        /// <para>If this is more than one type in the hierachy, this function will only find the first one found.</para>
        /// </remarks>
        ConfigurationNode FindNodeByType(ConfigurationNode parent, Type type);

        /// <summary>
        /// <para>When implemented by a class, finds nodes by their <seealso cref="ConfigurationNode.Name"/>.</para>
        /// </summary>
        /// <param name="parent">
        /// <para>The parent to start the search.</para>
        /// </param>
        /// <param name="name">
        /// <para>The name of the node.</para>
        /// </param>
        /// <returns>
        /// <para>The node if found or <see langword="null"/> if not found.</para>
        /// </returns>
        ConfigurationNode FindNodeByName(ConfigurationNode parent, string name);

        /// <summary>
        /// <para>When implemented by a class, determines if a node type exists in the hierarchy.</para>
        /// </summary>
        /// <param name="nodeType">
        /// <para>The <see cref="Type"/> of the node to find.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the type is contained in the hierarchy; otherwise <see langword="false"/>.</para>
        /// </returns>
        bool ContainsNodeType(Type nodeType);

        /// <summary>
        /// <para>When implemented by a class, determines if a node type exists in the hierarchy.</para>
        /// </summary>
        /// <param name="parent">
        /// <para>The parent to start the search.</para>
        /// </param>
        /// <param name="nodeType">
        /// <para>The <see cref="Type"/> of the node to find.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the type is contained in the hierarchy; otherwise <see langword="false"/>.</para>
        /// </returns>
        bool ContainsNodeType(ConfigurationNode parent, Type nodeType);

        /// <summary>
        /// <para>When implemented by a class, add a node to the hierarchy.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to add to the hierarchy.</para>
        /// </param>
        void AddNode(ConfigurationNode node);

        /// <summary>
        /// <para>When implemented by a class, remove a node from the hierarchy.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to remove.</para>
        /// </param>
        void RemoveNode(ConfigurationNode node);

        /// <summary>
        /// <para>When implemented by a class, renames a node in the hierarchy.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to be renamed.</para>
        /// </param>
        /// <param name="oldName">
        /// <para>The old name.</para>
        /// </param>
        /// <param name="newName">
        /// <para>The new name.</para>
        /// </param>
        void RenameNode(ConfigurationNode node, string oldName, string newName);

        /// <summary>
        /// <para>When implemented by a class, sets the <see cref="IServiceProvider"/> for the hiearchy.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The <see cref="IServiceProvider"/> for the hiearchy.</para>
        /// </param>
        void SetSite(IServiceProvider serviceProvider);

        /// <summary>
        /// <para>When implemented by a class, save the application and all it's configuration.</para>
        /// </summary>
        void Save();

        /// <summary>
        /// <para>When implemented by a class, opens the application and loads it's configuration.</para>
        /// </summary>
        void Open();

        /// <summary>
        /// <para>When implemented by a class, loads all available configuration available for the application.</para>
        /// </summary>
        void Load();

        /// <summary>
        /// <para>Builds a <see cref="ConfigurationContext"/> from the configuration settings of the current application configuration node.</para>
        /// </summary>
        /// <returns><para>The <see cref="ConfigurationContext"/> based on the current application.</para>.</returns>
        ConfigurationContext BuildConfigurationContext();

        /// <summary>
        /// <para>Gets the requested service.</para>
        /// </summary>
        /// <param name="serviceType"><para>The type of service to retrieve.</para></param>
        /// <returns><para>An instance of the service if it could be found, or a null reference (Nothing in Visual Basic) if it could not be found.</para></returns>
        object GetService(Type serviceType);
    }
}