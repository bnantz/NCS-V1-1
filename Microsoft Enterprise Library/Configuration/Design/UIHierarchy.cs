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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Provides hierarchy management for configuration design managers that implement hierarchies.
    /// </devdoc>
    internal class UIHierarchy : IUIHierarchy, IDisposable
    {
        private static object hierarchySaved = new object();

        private ConfigurationNode rootNode;
        private ConfigurationNode selectedNode;
        internal Hashtable nodesByType;
        private Hashtable nodesById;
        private Hashtable nodesByName;
        private bool loaded;
        private IServiceProvider serviceProvider;
        private ConfigurationContext configurationContext;
        private ConfigurationDesignManagerDomain configDomain;
        private StorageTable storageTable;
        private EventHandlerList handlerList;

        public event HierarchySavedEventHandler Saved
        {
            add { handlerList.AddHandler(hierarchySaved, value); }
            remove { handlerList.RemoveHandler(hierarchySaved, value); }
        }

        public UIHierarchy(IServiceProvider serviceProvider, ConfigurationContext configurationContext)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this.serviceProvider = serviceProvider;
            this.configurationContext = configurationContext;
            this.configDomain = new ConfigurationDesignManagerDomain(serviceProvider);
            nodesByType = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            nodesById = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            nodesByName = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            storageTable = new StorageTable();
            handlerList = new EventHandlerList();
        }

        public UIHierarchy(ConfigurationNode node, IServiceProvider serviceProvider, ConfigurationContext configurationContext) : this(serviceProvider, configurationContext)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            rootNode = node;
            selectedNode = node;
            AddNode(node);
        }
        
        public UIHierarchy(ConfigurationNode node, IServiceProvider serviceProvider) : this(node, serviceProvider, null)
        {
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (configurationContext != null) configurationContext.Dispose();
                handlerList.Dispose();
            }
        }

        /// <devdoc>
        /// Gets a unique id for the hierarchy.
        /// </devdoc>
        public Guid Id
        {
            get
            {
                if (rootNode != null)
                {
                    return rootNode.Id;
                }
                return Guid.Empty;
            }
        }

        /// <devdoc>
        /// Gets or sets the root node for the hiearchy.
        /// </devdoc>
        public ConfigurationNode RootNode
        {
            get { return rootNode; }
            set
            {
                if (rootNode != null)
                {
                    RemoveNode(rootNode);
                }
                rootNode = value;
                selectedNode = rootNode;
                AddNode(rootNode);
                InitializeFromRoot();
            }
        }

        /// <devdoc>
        /// Gets or sets the currently selected node in the hierarchy.
        /// </devdoc>
        public ConfigurationNode SelectedNode
        {
            get { return selectedNode; }
            set { selectedNode = value; }
        }

        /// <devdoc>
        /// Gets the ConfigurationContext used for this hierarchy.
        /// </devdoc>
        public ConfigurationContext ConfigurationContext
        {
            get { return configurationContext; }
            set { configurationContext = value; }
        }

        /// <devdoc>
        /// Gets the IStorageTable for the current heirarchy.
        /// </devdoc>
        public IStorageTable StorageTable
        {
            get { return storageTable; }
        }

        /// <devdoc>
        /// Finds a node via it's path. Use the ConfigurationNode.Path property get the path to the node.
        /// </devdoc>
        public ConfigurationNode FindNodeByPath(string path)
        {
            ConfigurationNode search = rootNode;
            while (search != null)
            {
                foreach (ConfigurationNode node in search.Nodes)
                {
                    if (node.Path.Equals(path))
                    {
                        return node;
                    }
                }
                search = search.FirstNode;
            }
            return null;
        }

        /// <devdoc>
        /// Finds a node by it's type.
        /// </devdoc>
        public ConfigurationNode[] FindNodesByType(Type type)
        {
            ArgumentValidation.CheckForNullReference(type, "type");
            
            return FindNodesByType(rootNode, type);
        }

        /// <devdoc>
        /// Finds nodes by it's type for a parent
        /// </devdoc>
        public ConfigurationNode[] FindNodesByType(ConfigurationNode parent, Type type)
        {
            ArgumentValidation.CheckForNullReference(type, "type");
            ArgumentValidation.CheckForNullReference(parent, "parent");

            ArrayList nodes = new ArrayList();
            if (type == parent.GetType())
            {
                nodes.Add(parent);
            }
            DoSearch(parent, type, nodes);
            return (ConfigurationNode[])nodes.ToArray(typeof(ConfigurationNode));
        }

        private void DoSearch(ConfigurationNode parent, Type type, ArrayList nodes)
        {
            if (parent == null) return;
            ConfigurationNode searchNode = parent;
            SearchNodeForType(searchNode, type, nodes);
            foreach (ConfigurationNode childNode in searchNode.Nodes)
            {
                DoSearch(childNode, type, nodes);
            }                
            searchNode = searchNode.NextSibling;
        }

        private void SearchNodeForType(ConfigurationNode searchNode, Type type, ArrayList nodes)
        {
            ConfigurationNode[] childNodes;
            NodeTypeEntryArrayList childNodesByType = (NodeTypeEntryArrayList)nodesByType[searchNode.Id];
            if (childNodesByType == null) return;
            childNodes = childNodesByType.FindAllEntryTypeMatchs(type.FullName);
            foreach (ConfigurationNode node in childNodes)
            {
                // since the same instance can be in multiple places... we have to make sure
                // we don't have it already
                if (!nodes.Contains(node))
                {
                    nodes.Add(node);
                }
                
            }
        }

        /// <summary>
        /// <para>Finds a node by it's <see cref="Type"/>.</para>
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
        public ConfigurationNode FindNodeByType(Type type)
        {
            return FindNodeByType(rootNode, type);
        }

        /// <summary>
        /// <para>Finds nodes by their <see cref="Type"/>.</para>
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
        public ConfigurationNode FindNodeByType(ConfigurationNode parent, Type type)
        {
            ConfigurationNode[] nodes = FindNodesByType(parent, type);
            if (nodes.Length > 0)
            {
                return nodes[0];
            }
            return null;
        }

        /// <summary>
        /// <para>Finds nodes by their <seealso cref="ConfigurationNode.Name"/>.</para>
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
        public ConfigurationNode FindNodeByName(ConfigurationNode parent, string name)
        {
            if (!nodesByName.Contains(parent.Id))
            {
                return null;
            }
            Hashtable childs = (Hashtable)nodesByName[parent.Id];
            return (ConfigurationNode)childs[name];
        }

        /// <summary>
        /// <para>Determines if a node type exists in the hierarchy.</para>
        /// </summary>
        /// <param name="nodeType">
        /// <para>The <see cref="Type"/> of the node to find.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the type is contained in the hierarchy; otherwise <see langword="false"/>.</para>
        /// </returns>f
        public bool ContainsNodeType(Type nodeType)
        {
            ConfigurationNode node = FindNodeByType(nodeType);
            return node != null;
        }

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
        public bool ContainsNodeType(ConfigurationNode parent, Type nodeType)
        {
            ConfigurationNode node = FindNodeByType(parent, nodeType);
            return node != null;
        }

        /// <summary>
        /// <para>Add a node to the hierarchy.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to add to the hierarchy.</para>
        /// </param>
        public void AddNode(ConfigurationNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            node.Hierarchy = this;
            string nodeTypeName = node.GetType().FullName;
            if (!nodesByType.Contains(node.Id))
            {
                nodesByType[node.Id] = new NodeTypeEntryArrayList();
            }
            if (node.Parent != null)
            {
                NodeTypeEntryArrayList childTypes = (NodeTypeEntryArrayList)nodesByType[node.Parent.Id];
                NodeTypeEntry entry = new NodeTypeEntry(node, nodeTypeName);
                if (!childTypes.Contains(entry))
                {
                    childTypes.Add(entry);
                    AddBaseTypes(node, childTypes);
                }
            }
            nodesById[node.Id] = node;
            AddToContainer(node);
            SetSiteName(node.Name, node);
            // make sure to add after contanier so the name is set
            AddNodeByName(node);
        }

        private void SetSiteName(string originalDisplayName, ConfigurationNode configurationNode)
        {
            INodeNameCreationService nodeNameCreationService = GetService(typeof(INodeNameCreationService)) as INodeNameCreationService;
            Debug.Assert(nodeNameCreationService != null, "The INodeNameCreationService needs to be implemented.");
            Debug.Assert(configurationNode.Site != null, "The Site was expeted not to be null");
            //configurationNode.Site.Name = nodeNameCreationService.GetUniqueDisplayName(configurationNode.Parent, originalDisplayName);
            string name = nodeNameCreationService.GetUniqueDisplayName(configurationNode.Parent, originalDisplayName);
            configurationNode.Rename(name, DefaultNodeRename.ForceRename);
        }

        /// <summary>
        /// <para>Remove a node from the hierarchy.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to remove.</para>
        /// </param>
        public void RemoveNode(ConfigurationNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            string nodeTypeName = node.GetType().FullName;
            nodesByType.Remove(node.Id);
            if (node.Parent != null)
            {
                NodeTypeEntryArrayList childTypes = (NodeTypeEntryArrayList)nodesByType[node.Parent.Id];
                NodeTypeEntry entryToRemove = new NodeTypeEntry(node, nodeTypeName);
                if (childTypes.Contains(entryToRemove))
                {
                    childTypes.Remove(entryToRemove);
                    RemoveBaseTypes(node, childTypes);
                }
            }
            node.Hierarchy = null;
            nodesById.Remove(node.Id);
            RemoveNodeByName(node);
            RemoveFromContainer(node);
        }

        /// <summary>
        /// <para>Renames a node in the hierarchy.</para>
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
        public void RenameNode(ConfigurationNode node, string oldName, string newName)
        {
            if (node.Parent == null)
            {
                return;
            }

            if (!nodesByName.Contains(node.Parent.Id))
            {
                return;

            }
            Hashtable childNodes = (Hashtable)nodesByName[node.Parent.Id];
            childNodes.Remove(oldName);
            childNodes.Add(newName, node);
        }

        /// <summary>
        /// <para>Sets the <see cref="IServiceProvider"/> for the hiearchy.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The <see cref="IServiceProvider"/> for the hiearchy.</para>
        /// </param>
        public void SetSite(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// <para>Save the application and all it's configuration.</para>
        /// </summary>
        public void Save()
        {
            this.configDomain.Save();
            IConfigurationErrorLogService errorLogService = serviceProvider.GetService(typeof(IConfigurationErrorLogService)) as IConfigurationErrorLogService;
            Debug.Assert(errorLogService != null);
            if (errorLogService.ConfigurationErrors.Count > 0) return;
            OnSaved(new HierarchySavedEventArgs(this));
        }

        /// <summary>
        /// <para>Opens the application and loads it's configuration.</para>
        /// </summary>
        public void Open()
        {
            if (!loaded)
            {
                Load();
            }
            this.configDomain.Open();
//            this.configDomain.ResolveDependencies();
            Debug.Assert(rootNode != null, "The root node should not be null");
            this.rootNode.ResolveNodeReferences();

        }

        /// <summary>
        /// <para>Loads all available configuration available for the application.</para>
        /// </summary>
        public void Load()
        {
            if (!loaded)
            {
                this.configDomain.Load();
            }
            this.configDomain.Register();
            loaded = true;
        }

        /// <summary>
        /// <para>Builds a <see cref="ConfigurationContext"/> from the configuration settings of the current application configuration node.</para>
        /// </summary>
        /// <returns><para>The <see cref="ConfigurationContext"/> based on the current application.</para>.</returns>
        public ConfigurationContext BuildConfigurationContext()
        {
            return this.configDomain.BuildContext();
        }

        /// <summary>
        /// <para>Gets the requested service.</para>
        /// </summary>
        /// <param name="serviceType"><para>The type of service to retrieve.</para></param>
        /// <returns><para>An instance of the service if it could be found, or a null reference (Nothing in Visual Basic) if it could not be found.</para></returns>
        public object GetService(Type serviceType)
        {
            return serviceProvider.GetService(serviceType);
        }

        private void AddNodeByName(ConfigurationNode node)
        {
            if (node.Parent == null)
            {
                return;
            }
            Hashtable childNodes = null;
            if (!nodesByName.Contains(node.Parent.Id))
            {
                childNodes = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
                nodesByName[node.Parent.Id] = childNodes;
            }
            else
            {
                childNodes = (Hashtable)nodesByName[node.Parent.Id];
            }
            Debug.Assert(node.Name != null, "The name should be set before adding to hierarchy.");
            childNodes.Add(node.Name, node);
        }

        private void RemoveNodeByName(ConfigurationNode node)
        {
            if (node.Parent == null)
            {
                return;
            }

            if (!nodesByName.Contains(node.Parent.Id))
            {
                return;

            }
            Hashtable childNodes = (Hashtable)nodesByName[node.Parent.Id];
            childNodes.Remove(node.Name);
        }

        private void AddBaseTypes(ConfigurationNode node, ArrayList tableToAddTypes)
        {
            Type searchType = node.GetType().BaseType;
            while (searchType != typeof(ConfigurationNode))
            {
                tableToAddTypes.Add(new NodeTypeEntry(node, searchType.FullName));
                searchType = searchType.BaseType;
            }
        }

        private void RemoveBaseTypes(ConfigurationNode node, NodeTypeEntryArrayList tableToAddTypes)
        {
            Type searchType = node.GetType().BaseType;
            while (searchType != typeof(ConfigurationNode))
            {
                NodeTypeEntry entry = new NodeTypeEntry(node, searchType.FullName);
                tableToAddTypes.Remove(entry);
                searchType = searchType.BaseType;
            }
        }

        private void AddToContainer(ConfigurationNode node)
        {
            IContainer container = GetContainer();
            Debug.Assert(container != null, "Could not get the IContainer.");
            container.Add(node);
        }

        private IContainer GetContainer()
        {
            return serviceProvider.GetService(typeof(IContainer)) as IContainer;
        }

        private void RemoveFromContainer(ConfigurationNode node)
        {
            IContainer container = GetContainer();
            Debug.Assert(container != null, "Could not get the IContainer.");
            container.Remove(node);
        }

        private void InitializeFromRoot()
        {
            ConfigurationNode search = rootNode;
            while (search != null)
            {
                foreach (ConfigurationNode node in search.Nodes)
                {
                    AddNode(node);
                }
                search = search.FirstNode;
            }
        }

        private void OnSaved(HierarchySavedEventArgs e)
        {
            HierarchySavedEventHandler handler = (HierarchySavedEventHandler)handlerList[hierarchySaved];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private class NodeTypeEntryArrayList : ArrayList
        {
            public override bool Contains(object item)
            {
                NodeTypeEntry nodeTypeEntry = (NodeTypeEntry)item;
                foreach (NodeTypeEntry entry in this)
                {
                    if (nodeTypeEntry == entry)
                    {
                        return true;
                    }
                }
                return false;
            }

            public ConfigurationNode[] FindAllEntryTypeMatchs(string typeName)
            {
                ArrayList list = new ArrayList(this.Count);
                foreach (NodeTypeEntry entry in this)
                {
                    if (entry.TypeName == typeName)
                    {
                        list.Add(entry.Node);
                    }
                }
                return (ConfigurationNode[])list.ToArray(typeof(ConfigurationNode));
            }
        }

        private class NodeTypeEntry
        {
            private readonly ConfigurationNode node;
            private readonly string typeName;

            public NodeTypeEntry(ConfigurationNode node, string typeName)
            {
                this.node = node;
                this.typeName = typeName;
            }

            public static bool operator ==(NodeTypeEntry lhs, NodeTypeEntry rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(NodeTypeEntry lhs, NodeTypeEntry rhs)
            {
                return !lhs.Equals(rhs);
            }

            public ConfigurationNode Node
            {
                get { return node; }
            }

            public string TypeName
            {
                get { return typeName; }
            }

            public override bool Equals(object obj)
            {
                NodeTypeEntry entry = (NodeTypeEntry)obj;
                return node.Id == entry.node.Id && typeName == entry.typeName;
            }

            public override int GetHashCode()
            {
                return node.Id.GetHashCode() ^ typeName.GetHashCode();
            }
        }
    }
}