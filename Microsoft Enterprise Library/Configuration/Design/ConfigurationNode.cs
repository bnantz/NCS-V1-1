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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a node from runtime configuration. It allows a visual hierarchieal representation of configuration data.</para>
    /// </summary>
    [Image(typeof(ConfigurationNode))]
    [DefaultProperty("Name")]
    public abstract class ConfigurationNode : SitedComponent, IComparable
    {
        private static readonly object changedEvent = new object();
        private static readonly object childInsertedEvent = new object();
        private static readonly object renamingEvent = new object();
        private static readonly object renamedEvent = new object();
        private static readonly object childRemovedEvent = new object();
        private static readonly object childAddedEvent = new object();
        private static readonly object childAddingEvent = new object();
        private static readonly object removingEvent = new object();
        private static readonly object removedEvent = new object();

        private Guid id;
        private ConfigurationNode parent;
        private int index;
        private int childCount;
        private ConfigurationNode[] childNodes;
        private ConfigurationNodeCollection nodes;
        private IUIHierarchy hierarchy;
        internal Hashtable childNodeLookup;
        private IMenuContainerService currentContainerService;
        private bool sortChildren;

        /// <summary>
        /// <para>Occurs when the internal state of the current node has changed.</para>
        /// </summary>
        public event ConfigurationNodeChangedEventHandler Changed
        {
            add { Events.AddHandler(changedEvent, value); }
            remove { Events.RemoveHandler(changedEvent, value); }
        }

        /// <summary>
        /// <para>Occurs after a node is added as a child node to the current object.</para>
        /// </summary>
        public event ConfigurationNodeChangedEventHandler ChildInserted
        {
            add { Events.AddHandler(childInsertedEvent, value); }
            remove { Events.RemoveHandler(childInsertedEvent, value); }
        }

        /// <summary>
        /// <para>Occurs after the name of the current node changes.</para>
        /// </summary>
        public event ConfigurationNodeChangedEventHandler Renaming
        {
            add { Events.AddHandler(renamingEvent, value); }
            remove { Events.RemoveHandler(renamingEvent, value); }
        }

        /// <summary>
        /// <para>Occurs after the name of the current node changes.</para>
        /// </summary>
        public event ConfigurationNodeChangedEventHandler Renamed
        {
            add { Events.AddHandler(renamedEvent, value); }
            remove { Events.RemoveHandler(renamedEvent, value); }
        }

        /// <summary>
        /// <para>Occurs after a child node of the current node is removed.</para>
        /// </summary>
        public event ConfigurationNodeChangedEventHandler ChildRemoved
        {
            add { Events.AddHandler(childRemovedEvent, value); }
            remove { Events.RemoveHandler(childRemovedEvent, value); }
        }

        /// <summary>
        /// <para>Occurs after a child node is added to the current node.</para>
        /// </summary>
        public event ConfigurationNodeChangedEventHandler ChildAdding
        {
            add { Events.AddHandler(childAddingEvent, value); }
            remove { Events.RemoveHandler(childAddingEvent, value); }
        }

        /// <summary>
        /// <para>Occurs after a child node is added to the current node.</para>
        /// </summary>
        public event ConfigurationNodeChangedEventHandler ChildAdded
        {
            add { Events.AddHandler(childAddedEvent, value); }
            remove { Events.RemoveHandler(childAddedEvent, value); }
        }

        /// <summary>
        /// <para>Occurs after the current node is removed from its parent's node collection.</para>
        /// </summary>
        public event ConfigurationNodeChangedEventHandler Removed
        {
            add { Events.AddHandler(removedEvent, value); }
            remove { Events.RemoveHandler(removedEvent, value); }
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationNode"/> class with a display name.</para>
        /// </summary>       
        protected ConfigurationNode() : base()
        {
            id = Guid.NewGuid();
            sortChildren = true;
            childNodeLookup = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
        }

        /// <summary>
        /// <para>Gets the position of this node in the <seealso cref="Nodes"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The position of this node in the <seealso cref="Nodes"/>.</para>
        /// </value>
        [Browsable(false)]
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// <para>Gets the collection of <see cref="ConfigurationNode"/> objects assigned to the current configuration node.</para>
        /// </summary>
        /// <value>
        /// <para>The collection of <see cref="ConfigurationNode"/> objects assigned to the current configuration node.</para>
        /// </value>
        [Browsable(false)]
        public ConfigurationNodeCollection Nodes
        {
            get
            {
                if (nodes == null)
                {
                    nodes = new ConfigurationNodeCollection(this);
                }
                return nodes;
            }
        }

        /// <summary>
        /// <para>Gets or sets the name for the node.</para>
        /// </summary>
        /// <value>
        /// <para>The display name for the node.</para>
        /// </value>
        /// <remarks>
        /// <para>The name should be the <seealso cref="ISite.Name"/>.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// <para>The name already exists in the parent's node collection.</para>
        /// </exception>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.NodeNameDescription)]
        [Required]
        public virtual string Name
        {
            get
            {
                if (Site == null)
                {
                    return string.Empty;
                }
                return Site.Name;
            }
            set { Rename(value, DefaultNodeRename.DoNotForceRename); }
        }

        /// <summary>
        /// <para>Gets the node immediately preceding this node.</para>
        /// </summary>
        /// <value>
        /// <para>The preceding <see cref="ConfigurationNode"/>. If there is no preceding node, a <see langword="null"/> reference (Nothing in Visual Basic) is returned.</para>
        /// </value>
        [Browsable(false)]
        public ConfigurationNode PreviousSibling
        {
            get
            {
                if (Index > 0)
                {
                    int previousIndex = Index - 1;
                    return parent.childNodes[previousIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// <para>Gets the node immediately following this node.</para>
        /// </summary>
        /// <value>
        /// <para>The next <see cref="ConfigurationNode"/>. If there is no next node, a <see langword="null"/> reference (Nothing in Visual Basic) is returned.</para>
        /// </value>
        [Browsable(false)]
        public ConfigurationNode NextSibling
        {
            get
            {
                int nextIndex = Index + 1;
                // check if this is the last child
                if (parent != null && nextIndex != parent.childCount)
                {
                    return parent.childNodes[nextIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// <para>Gets the first child tree node in the node collection.</para>
        /// </summary>
        /// <value>
        /// <para>The first child <see cref="ConfigurationNode"/> in the <seealso cref="Nodes"/> collection.</para>
        /// </value>
        /// <remarks>
        /// The <seealso cref="FirstNode"/> is the first child <see cref="ConfigurationNode"/> in the <see cref="ConfigurationNodeCollection"/> stored in the <seealso cref="Nodes"/> property of the current node. If the <see cref="ConfigurationNode"/> has no child node, the <seealso cref="FirstNode"/> property returns a <see langword ="null"/> reference (Nothing in Visual Basic).
        /// </remarks>
        [Browsable(false)]
        public ConfigurationNode FirstNode
        {
            get
            {
                if (childCount > 0)
                {
                    return childNodes[0];
                }
                return null;
            }
        }

        /// <summary>
        /// <para>Gets the first child tree node in the node collection.</para>
        /// </summary>
        /// <value>
        /// <para>The first child <see cref="ConfigurationNode"/> in the <seealso cref="Nodes"/> collection.</para>
        /// </value>
        /// <remarks>
        /// The <seealso cref="LastNode"/> is the last child <see cref="ConfigurationNode"/> in the <see cref="ConfigurationNodeCollection"/> stored in the <seealso cref="Nodes"/> property of the current node. If the <see cref="ConfigurationNode"/> has no child node, the <seealso cref="FirstNode"/> property returns a <see langword ="null"/> reference (Nothing in Visual Basic).
        /// </remarks>
        [Browsable(false)]
        public ConfigurationNode LastNode
        {
            get
            {
                if (childCount > 0)
                {
                    return childNodes[childCount - 1];
                }
                return null;
            }
        }

        /// <summary>
        /// <para>Gets the parent configuration node of the the current configuration node.</para>
        /// </summary>
        /// <value>
        /// <para>The parent configuration node of the the current configuration node.</para>
        /// </value>
        [Browsable(false)]
        public ConfigurationNode Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// <para>Gets the path from the root node to the current node.</para>
        /// </summary>
        /// <value>
        /// <para>The path from the root node to the current node.</para>
        /// </value>
        [Browsable(false)]
        public string Path
        {
            get
            {
                StringBuilder path = new StringBuilder();
                GetFullPath(path, System.IO.Path.AltDirectorySeparatorChar);
                return path.ToString();
            }
        }

        /// <summary>
        /// <para>Gets a runtime-generated, non-persisted unique identifier for this node.</para>
        /// </summary>
        /// <value>
        /// <para>A runtime-generated, non-persisted unique identifier for this node.</para>
        /// </value>
        [Browsable(false)]
        public Guid Id
        {
            get { return id; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="IUIHierarchy"/> containing this node.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IUIHierarchy"/> containing this node. The default is <see langword="null"/></para>
        /// </value>
        [Browsable(false)]
        public IUIHierarchy Hierarchy
        {
            get { return hierarchy; }
            set { hierarchy = value; }
        }

        /// <summary>
        /// <para>Gets if children added to the node are sorted.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if nodes add are sorted; otherwise <see langword="false"/>. The default is <see langword="true"/></para>
        /// </value>
        [Browsable(false)]
        public virtual bool SortChildren
        {
            get { return sortChildren; }
        }

        internal int ChildCount
        {
            get { return childCount; }
        }

        internal ConfigurationNode[] ChildNodes
        {
            get { return childNodes; }
        }

        /// <summary>
        /// <para>Moves the specified child node immediately after the specified reference node.</para>
        /// </summary>
        /// <param name="childNode"><para>The existing child node to move.</para></param>
        /// <param name="siblingNode"><para>The existing child node after which the <i>childNode</i> will be placed.</para></param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="childNode"/> can not be <see langword="null"/>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="siblingNode"/> can not be <see langword="null"/>.</para>
        /// </exception>
        public void MoveAfter(ConfigurationNode childNode, ConfigurationNode siblingNode)
        {
            ArgumentValidation.CheckForNullReference(childNode, "childNode");
            ArgumentValidation.CheckForNullReference(siblingNode, "siblingNode");
            
            if ((childNode.Parent != this) || (siblingNode.Parent != this))
            {
                throw new InvalidOperationException(SR.ExceptionOnlyReorderSiblings);
            }
            if (siblingNode.NextSibling != childNode)
            {
                Nodes.Remove(childNode);
                Nodes.Insert(siblingNode.Index + 1, childNode);
                OnChildInserted(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Insert, childNode, this));
            }
        }

        /// <summary>
        /// <para>Moves the specified node immediately before the specified reference node.</para>
        /// </summary>
        /// <param name="childNode"><para>The existing child node to move.</para></param>
        /// <param name="siblingNode"><para>The existing child node before which the <i>childNode</i> will be placed.</para></param>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="childNode"/> can not be <see langword="null"/>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="siblingNode"/> can not be <see langword="null"/>.</para>
        /// </exception>
        public void MoveBefore(ConfigurationNode childNode, ConfigurationNode siblingNode)
        {
            ArgumentValidation.CheckForNullReference(childNode, "childNode");
            ArgumentValidation.CheckForNullReference(siblingNode, "siblingNode");

            if (childNode.Parent != this || siblingNode.Parent != this)
            {
                throw new InvalidOperationException(SR.ExceptionOnlyReorderSiblings);
            }
            // check if the node's are already ordered in this way
            if (siblingNode.PreviousSibling != childNode)
            {
                Nodes.Remove(childNode);
                Nodes.Insert(siblingNode.Index, childNode);
                OnChildInserted(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Insert, childNode, this));
            }
        }

        /// <summary>
        /// <para>Creates and returns a string representation of the current node.</para>
        /// </summary>
        /// <returns><para>A string representation of the current node.</para></returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// <para>Removes the current node from its parent node.</para>
        /// </summary>
        public void Remove()
        {
            RemoveNode(true);
        }

        /// <summary>
        /// <para>Allows a node to add default nodes after being created.</para>
        /// </summary>
        /// <remarks>
        /// <para>This is called by the framework the each time an instance of this node is created with an <see cref="AddChildNodeCommand"/>.</para>
        /// </remarks>
        protected virtual void AddDefaultChildNodes()
        {
        }

        /// <summary>
        /// <para>Allows child nodes to resolve references to sibling nodes in configuration.</para>
        /// </summary>
        /// <remarks>
        /// <para>This is called by the framework the each time an instance of this node is created from a previous saved state.</para>
        /// </remarks>
        public virtual void ResolveNodeReferences()
        {
            foreach (ConfigurationNode child in this.Nodes)
            {
                child.ResolveNodeReferences();
            }
        }

        /// <summary>
        /// <para>Add menu items to the user interface.</para>
        /// </summary>
        /// <param name="menuContainerService">
        /// <para>The container to add the menu items.</para>
        /// </param>
        /// <remarks>
        /// <para>This is called by the framework when the context switches to this node.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="menuContainerService"/> can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void AddMenuItems(IMenuContainerService menuContainerService)
        {
            ArgumentValidation.CheckForNullReference(menuContainerService, "containerService");
            
            currentContainerService = menuContainerService;
            OnAddMenuItems();
        }

        /// <summary>
        /// <para>Add a menu item to the user interface.</para>
        /// </summary>
        /// <param name="configurationMenuItem">
        /// <para>The <see cref="ConfigurationMenuItem"/> to add.</para>
        /// </param>
        protected void AddMenuItem(ConfigurationMenuItem configurationMenuItem)
        {
            ArgumentValidation.CheckForNullReference(configurationMenuItem, "configurationMenuItem");
            Debug.Assert(currentContainerService != null, "The currentContainerService should not be null");

            currentContainerService.MenuItems.Add(configurationMenuItem);
        }

        /// <summary>
        /// <para>Allows nodes that override to add menu items for the user interface.</para>
        /// </summary>
        /// <remarks>
        /// <para>Call <seealso cref="AddMenuItem"/> for every menu item that needs to be added to the user interface.</para>
        /// <para>By default, the base class adds the <see cref="ValidateNodeCommand"/> and <see cref="RemoveNodeCommand"/></para>
        /// </remarks>
        protected virtual void OnAddMenuItems()
        {
            currentContainerService.MenuItems.Add(ConfigurationMenuItem.CreateValidateNodeCommand(Site, this));
            currentContainerService.MenuItems.Add(ConfigurationMenuItem.CreateRemoveNodeCommand(Site, this));
        }

        /// <summary>
        /// <para>Compares the specified node to the current node based on the value of the <seealso cref="Name"/> property.</para>
        /// </summary>
        /// <param name="node"><para>An object to compare.</para></param>
        /// <returns>
        /// <para>A signed integer that indicates the relative order of this node and the node being compared.</para>
        /// </returns>
        int IComparable.CompareTo(object node)
        {
            ConfigurationNode compareNode = node as ConfigurationNode;
            if (compareNode == null)
            {
                return -1;
            }
            return Id.CompareTo(compareNode.Id);
        }

        /// <summary>
        /// <para>Compares the specified node to the current node based on the value of the <seealso cref="Name"/> property.</para>
        /// </summary>
        /// <param name="node"><para>A <see cref="ConfigurationNode"/> to compare.</para></param>
        /// <returns>
        /// <para>A signed integer that indicates the relative order of this node and the node being compared.</para>
        /// </returns>
        public int CompareTo(ConfigurationNode node)
        {
            return ((IComparable)this).CompareTo(node);
        }

        /// <summary>
        /// <para>Compares two <see cref="ConfigurationNode"></see> instances for equality.</para>
        /// </summary>
        /// <param name="obj"><para>The object to compare.</para></param>
        /// <returns><para><see langword="true"/> if objects are equal; otherwise <see langword="false"/>.</para></returns>
        public override bool Equals(object obj)
        {
            return ((IComparable)this).CompareTo(obj) == 0;
        }

        /// <summary>
        /// <para>Compares two <see cref="ConfigurationNode"></see> instances for equality.</para>
        /// </summary>
        /// <param name="node"><para>The <see cref="ConfigurationNode"/> to compare.</para></param>
        /// <returns><para><see langword="true"/> if objects are equal; otherwise <see langword="false"/>.</para></returns>
        public bool Equals(ConfigurationNode node)
        {
            return Equals((object)node);
        }

        /// <summary>
        /// <para>Determines whether two specified <see cref="ConfigurationNode"/> objects have the same value.</para>
        /// </summary>
        /// <param name="a"><para>A <see cref="ConfigurationNode"/> or a <see langword="null"/> reference (Nothing in Visual Basic).</para></param>
        /// <param name="b"><para>A <see cref="ConfigurationNode"/> or a <see langword="null"/> reference (Nothing in Visual Basic).</para></param>
        /// <returns><para><see langword="true"/> if the value of a is the same as the value of b; otherwise, <see langword="false"/>.</para></returns>
        public static bool operator == (ConfigurationNode a, ConfigurationNode b) 
        {
            if ((Object)a==(Object)b) 
            {
                return true;
            }
    
            if ((Object)a==null || (Object)b==null) 
            {
                return false;
            }
            return a.Equals(b);
        }

        /// <summary>
        /// <para>Determines whether two specified <see cref="ConfigurationNode"/> objects have the different value.</para>
        /// </summary>
        /// <param name="a"><para>A <see cref="ConfigurationNode"/> or a <see langword="null"/> reference (Nothing in Visual Basic).</para></param>
        /// <param name="b"><para>A <see cref="ConfigurationNode"/> or a <see langword="null"/> reference (Nothing in Visual Basic).</para></param>
        /// <returns><para><see langword="true"/> if the value of <paramref name="a"/> is different from the value of <paramref name="b"/>; otherwise, <see langword="false"/>.</para></returns>
        public static bool operator != (ConfigurationNode a, ConfigurationNode b) 
        {
            return !(a == b);
        }

        /// <summary>
        /// <para>Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.</para>
        /// </summary>
        /// <returns><para>A hash code for the current <see cref="ConfigurationNode"/>.</para></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        internal void InsertNodeAt(int index, ConfigurationNode node, bool updateSite)
        {
            EnsureNodeCapacity();
            node.parent = this;
            node.index = index;
            for (int count = childCount; count > index; --count)
            {
                (childNodes[count] = childNodes[count - 1]).index = count;
            }
            childNodes[index] = node;
            childCount++;
            if (updateSite) UpdateSite(node);
        }

        internal void Clear()
        {
            while (childCount > 0)
            {
                childNodes[childCount - 1].RemoveNode(true);
            }
            childNodes = null;
        }

        internal void EnsureNodeCapacity()
        {
            if (childNodes == null)
            {
                childNodes = new ConfigurationNode[4];
            }
            else if (childCount == childNodes.Length)
            {
                ConfigurationNode[] resizeArray = new ConfigurationNode[childCount * 2];
                Array.Copy(childNodes, 0, resizeArray, 0, childCount);
                childNodes = resizeArray;
            }
        }

        internal void RemoveNode(bool notifyParent)
        {
            OnRemoving(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Remove, this, parent));
            
            for (int i = 0; i < childCount; i++)
            {
                childNodes[i].RemoveNode(false);
            }
            Debug.Assert(Site != null, "The site should be set by now.");
            // since the name is based on site have to store to really remove it
            string nameToRemove = this.Name;
            RemoveNodeFromChildLookup(notifyParent, nameToRemove);
            hierarchy.RemoveNode(this);
            if (notifyParent && (parent != null))
            {
                parent.childNodes[index] = null;
                for (int i = index; i < parent.childCount - 1; ++i)
                {
                    parent.childNodes[i] = parent.childNodes[i + 1];
                    parent.childNodes[i].index = i;
                }
                parent.childCount--;
                parent.OnChildRemoved(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Remove, this, null));
            }
            parent = null;
            OnRemoved(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Remove, this, parent));
        }

        private void RemoveNodeFromChildLookup(bool notifyParent, string nameToRemove)
        {
            if (notifyParent && (parent != null)) parent.childNodeLookup.Remove(nameToRemove);
        }

        internal int AddNodeWithDefaultChildren(ConfigurationNode configurationNode)
        {
            int index = AddNode(configurationNode);
            configurationNode.AddDefaultChildNodes();
            return index;
        }

        internal int AddNode(ConfigurationNode configurationNode)
        {
            EnsureNodeCapacity();
            int index = -1;
            configurationNode.parent = this;
            OnChildAdding(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Insert, configurationNode, this));
            UpdateSite(configurationNode);
            if (SortChildren) 
            {
                index = AddSorted(configurationNode);
            } 
            else 
            {
                index = AddUnsorted(configurationNode);
            }
            OnChildAdded(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Insert, configurationNode, this));
            return index;
        }

        private int AddUnsorted(ConfigurationNode configurationNode) 
        {   
            configurationNode.index = childCount++;
            childNodes[configurationNode.index] = configurationNode;  
            return configurationNode.index;
        }

        private int AddSorted(ConfigurationNode node) 
        {
            int index = 0;
            string nodeDisplayName = node.Name;
            if (childCount > 0) 
            {
                CompareInfo compare = CultureInfo.CurrentCulture.CompareInfo;                
                if (compare.Compare(childNodes[childCount-1].Name, nodeDisplayName) <= 0) 
                {
                    index = childCount;
                } 
                else 
                {                    
                    int firstNode = 0;
                    int lastNode = 0;
                    int compareNode = 0;                   
                    for (firstNode = 0, lastNode = childCount; firstNode < lastNode;) 
                    {
                        compareNode = (firstNode + lastNode) / 2;
                        if (compare.Compare(childNodes[compareNode].Name, nodeDisplayName) <= 0) 
                        {
                            firstNode = compareNode + 1;
                        } 
                        else 
                        {
                            lastNode = compareNode;
                        }                        
                    }
                    index = firstNode;
                }
            }
            InsertNodeAt(index, node, false);            
            return index;
        }
        

        /// <summary>
        /// <para>Create menu items based on the nodes registered with the <see cref="INodeCreationService"/> for the spefied type.</para>
        /// </summary>
        /// <param name="nodeType">
        /// <para>The type to create menu items.</para>
        /// </param>
        protected void CreateDynamicMenuItems(Type nodeType)
        {
            INodeCreationService service = ServiceHelper.GetNodeCreationService(Site);
            StringCollection names = service.GetDisplayNames(nodeType);
            ConfigurationMenuItem item = null;
            foreach (string name in names)
            {
                NodeCreationEntry entry = service.GetNodeCreationEntry(name);
                item = new ConfigurationMenuItem(name, entry.ConfigurationNodeCommand, this, Shortcut.None, SR.GenericCreateStatusText(name), InsertionPoint.New);
                if (!entry.AllowMultiple)
                {
                    if (Hierarchy.ContainsNodeType(this, nodeType/*entry.BaseTypeToCompare*/))
                    {
                        item.Enabled = false;
                    }
                }
                AddMenuItem(item);
            }
        }
        

        /// <summary>
        /// <para>Create child nodes for a collection of configuration data.</para>
        /// </summary>
        /// <param name="collection">
        /// <para>A collection of runtime data.</para>
        /// </param>
        protected void CreateDynamicNodes(ICollection collection)
        {
            INodeCreationService service = ServiceHelper.GetNodeCreationService(Site);
            ConfigurationNode configurationNode = null;
            foreach (object data in collection)
            {
                configurationNode = service.CreateNode(data.GetType(), new object[] {data});
                Debug.Assert(configurationNode != null, String.Format("The configuration for {0} type was not registered succesfully.", data.GetType().Name));
                this.Nodes.Add(configurationNode);
            }
        }


        /// <summary>
        /// <para>Raises the <seealso cref="ChildRemoved"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected virtual void OnChildRemoved(ConfigurationNodeChangedEventArgs e)
        {
            if (Events != null)
            {
                ConfigurationNodeChangedEventHandler handler = (ConfigurationNodeChangedEventHandler)Events[childRemovedEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
            OnChanged(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Changed, this, Parent));
        }

        /// <summary>
        /// <para>Raises the <seealso cref="Removed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected virtual void OnRemoved(ConfigurationNodeChangedEventArgs e)
        {
            foreach (ConfigurationNode child in e.Node.Nodes)
            {
                child.OnRemoved(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Remove, child, e.Node));
            }
            if (Events != null)
            {
                ConfigurationNodeChangedEventHandler handler = (ConfigurationNodeChangedEventHandler)Events[removedEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
            OnChanged(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Changed, this, Parent));
        }

        /// <summary>
        /// <para>Raises the <seealso cref="Removed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected virtual void OnRemoving(ConfigurationNodeChangedEventArgs e)
        {
            foreach (ConfigurationNode child in e.Node.Nodes)
            {
                child.OnRemoving(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Remove, child, e.Node));
            }
            if (Events != null)
            {
                ConfigurationNodeChangedEventHandler handler = (ConfigurationNodeChangedEventHandler)Events[removingEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
            OnChanged(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Changed, this, Parent));
        }

        /// <summary>
        /// <para>Raises the <seealso cref="ChildInserted"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected virtual void OnChildInserted(ConfigurationNodeChangedEventArgs e)
        {
            if (Events != null)
            {
                ConfigurationNodeChangedEventHandler handler = (ConfigurationNodeChangedEventHandler)Events[childInsertedEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
            OnChanged(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Changed, this, Parent));
        }

        /// <summary>
        /// <para>Raises the <see cref="Renamed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected virtual void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            if (Events != null)
            {
                ConfigurationNodeChangedEventHandler handler = (ConfigurationNodeChangedEventHandler)Events[renamedEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
            OnChanged(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Changed, this, Parent));
        }

        /// <summary>
        /// Raises the <see cref="ChildAdded"/> event.
        /// </summary>
        /// <param name="e">A 
        /// <see cref="ConfigurationNodeChangedEventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnChildAdding(ConfigurationNodeChangedEventArgs e)
        {
            if (Events != null)
            {
                ConfigurationNodeChangedEventHandler handler = (ConfigurationNodeChangedEventHandler)Events[childAddingEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
            OnChanged(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Changed, this, Parent));
        }

        /// <summary>
        /// Raises the <see cref="ChildAdded"/> event.
        /// </summary>
        /// <param name="e">A 
        /// <see cref="ConfigurationNodeChangedEventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnChildAdded(ConfigurationNodeChangedEventArgs e)
        {
            if (Events != null)
            {
                ConfigurationNodeChangedEventHandler handler = (ConfigurationNodeChangedEventHandler)Events[childAddedEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
            OnChanged(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Changed, this, Parent));
        }

        /// <summary>
        /// <para>Raises the <seealso cref="Changed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> object containing the event data.</para></param>
        protected virtual void OnChanged(ConfigurationNodeChangedEventArgs e)
        {
            if (Events != null)
            {
                ConfigurationNodeChangedEventHandler handler = (ConfigurationNodeChangedEventHandler)Events[changedEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }


        private void UpdateSite(ConfigurationNode configurationNode)
        {
            hierarchy.AddNode(configurationNode);
            Debug.Assert(Site != null, "The site should have been set by now.");
            childNodeLookup.Add(configurationNode.Name, configurationNode);
        }

        internal void Rename(string newName, DefaultNodeRename defaultNodeRename)
        {
            if (newName == null)
            {
                throw new ArgumentNullException("newName");
            }
            if (newName == Name)
            {
                return;
            }
            string oldName = Name;
            if (parent == null)
            {
                Site.Name = newName;
            }
            else
            {
                if (parent.childNodeLookup.Contains(newName))
                {
                    throw new InvalidOperationException(SR.ExceptionNodeNameAlreadyExists(newName));
                }
                if (defaultNodeRename == DefaultNodeRename.DoNotForceRename)
                {
                    parent.childNodeLookup.Remove(oldName);
                
                    try
                    {
                        parent.childNodeLookup.Add(newName, this);
                    }
                    catch
                    {
                        parent.childNodeLookup.Add(oldName, this);
                        throw;
                    }
                }
                Site.Name = newName;
            }
            if (defaultNodeRename == DefaultNodeRename.DoNotForceRename)
            {
                Hierarchy.RenameNode(this, oldName, newName);
            }
            OnRenamed(new ConfigurationNodeChangedEventArgs(ConfigurationNodeChangedAction.Rename, this, parent));
        }

        private void GetFullPath(StringBuilder path, char pathSeparator)
        {
            if (parent != null)
            {
                parent.GetFullPath(path, pathSeparator);
                path.Append(pathSeparator);
            }
            path.Append(this.Name);
        }
    }
}