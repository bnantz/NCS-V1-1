//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// The root node for caching configuration.
    /// </summary>
    [Image(typeof(CacheManagerSettingsNode))]
    [ServiceDependency(typeof(ILinkNodeService))]
    public class CacheManagerSettingsNode : ConfigurationNode
    {
        private CacheManagerSettings cacheManagerSettings;
        private CacheManagerNode cacheManagerNode;
        private ConfigurationNodeChangedEventHandler cacheManagerNodeRemovedHandler;
        private ConfigurationNodeChangedEventHandler cacheManagerNodeRenamedHandler;

        /// <summary>
        /// Creates node with initial data.
        /// </summary>
        public CacheManagerSettingsNode() : this(new CacheManagerSettings())
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheManagerSettingsNode"/> class.
		/// </summary>
		/// <param name="cacheManagerSettings">The settings to use for initialization.</param>
        public CacheManagerSettingsNode(CacheManagerSettings cacheManagerSettings) : base()
        {
            this.cacheManagerNodeRemovedHandler = new ConfigurationNodeChangedEventHandler(OnCacheManagerNodeRemoved);
            this.cacheManagerNodeRenamedHandler = new ConfigurationNodeChangedEventHandler(OnCacheManagerNodeRenamed);
            this.cacheManagerSettings = cacheManagerSettings;
        }

		/// <summary>
		/// <para>Gets the name for the node.</para>
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
		[ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// The default cache manager
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DefaultCacheManagerDescription)]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CacheManagerNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public CacheManagerNode DefaultCacheManager
        {
            get { return cacheManagerNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                cacheManagerNode = (CacheManagerNode)service.CreateReference(cacheManagerNode, value, cacheManagerNodeRemovedHandler, cacheManagerNodeRenamedHandler);
                this.cacheManagerSettings.DefaultCacheManager = cacheManagerNode == null ? String.Empty : cacheManagerNode.Name;
            }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual CacheManagerSettings CacheManagerSettings
        {
            get
            {
                CacheManagerCollectionNode cacheManagerCollectionNode = Hierarchy.FindNodeByType(this, typeof(CacheManagerCollectionNode)) as CacheManagerCollectionNode;
                Debug.Assert(cacheManagerCollectionNode != null, "We should have a cache manager collection.");
                CacheManagerDataCollection cacheManagerDataCollection = cacheManagerCollectionNode.CacheManagerDataCollection;
                if (Object.ReferenceEquals(cacheManagerDataCollection, cacheManagerSettings.CacheManagers))
                {
                    return cacheManagerSettings;
                }

                cacheManagerSettings.CacheManagers.Clear();
                foreach (CacheManagerData managerData in cacheManagerDataCollection)
                {
                    cacheManagerSettings.CacheManagers[managerData.Name] = managerData;
                }
                return cacheManagerSettings;
            }

        }

        /// <summary>
        /// See <see cref="ConfigurationNode.ResolveNodeReferences"/>.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            CacheManagerCollectionNode nodes = Hierarchy.FindNodeByType(this, typeof(CacheManagerCollectionNode)) as CacheManagerCollectionNode;
            DefaultCacheManager = Hierarchy.FindNodeByName(nodes, this.cacheManagerSettings.DefaultCacheManager) as CacheManagerNode;
        }

        /// <summary>
        /// Adds default nodes for managers and storages.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            CacheManagerCollectionNode node = new CacheManagerCollectionNode(this.cacheManagerSettings.CacheManagers);
            Nodes.AddWithDefaultChildren(node);
        }

		/// <summary>
		/// Creates a default <see cref="CacheManagerCollectionNode"/> when this node is sited.
		/// </summary>
		protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DefaultCacheManagerSettingsNodeName;
            if ((cacheManagerSettings.CacheManagers != null) && (cacheManagerSettings.CacheManagers.Count > 0))
            {
                CacheManagerCollectionNode node = new CacheManagerCollectionNode(cacheManagerSettings.CacheManagers);
                Nodes.Add(node);
            }
        }

		/// <summary>
		/// <para>Adds a menu item for the CacheManagerCollectionNode.</para>
		/// </summary>
		protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            ConfigurationMenuItem item = ConfigurationMenuItem.CreateAddChildNodeMenuItem(Site, this, typeof(CacheManagerCollectionNode), SR.CacheManagerMenuText, false);
            AddMenuItem(item);
        }

		/// <summary>
		/// Raises the ChildAdded event and confirms that only one <see cref="CacheManagerCollectionNode"/> has been added.
		/// </summary>
		/// <param name="e">A 
		/// <see cref="ConfigurationNodeChangedEventArgs"/> that contains
		/// the event data.</param>
		/// <exception cref="InvalidOperationException" />
		protected override void OnChildAdded(ConfigurationNodeChangedEventArgs e)
        {
            base.OnChildAdded (e);
            if (Nodes.Count > 1 && e.Node.GetType() == typeof(CacheManagerCollectionNode))
            {
                throw new InvalidOperationException(SR.ExceptionOnlyOneCacheManagerCollectionNode);
            }
        }


        private void OnCacheManagerNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.cacheManagerNode = null;
        }

        private void OnCacheManagerNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.cacheManagerSettings.DefaultCacheManager = e.Node.Name;
        }
    }
}