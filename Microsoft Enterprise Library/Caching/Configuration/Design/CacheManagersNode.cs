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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Node that represents a CacheManagerDataCollection
    /// </summary>
    [Image(typeof(CacheManagerCollectionNode))]
    public class CacheManagerCollectionNode : ConfigurationNode
    {
        private CacheManagerDataCollection cacheManagerDataCollection;

        /// <summary>
        /// Creates node with initial data.
        /// </summary>
        public CacheManagerCollectionNode() : this(new CacheManagerDataCollection())
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheManagerDataCollection"/> class with the given settings.
		/// </summary>
		/// <param name="cacheManagerDataCollection">A settings class to use for initialization.</param>
        public CacheManagerCollectionNode(CacheManagerDataCollection cacheManagerDataCollection) : base()
        {
            if (cacheManagerDataCollection == null)
            {
                throw new ArgumentNullException("cacheManagerDataCollection");
            }
            this.cacheManagerDataCollection = cacheManagerDataCollection;
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
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual CacheManagerDataCollection CacheManagerDataCollection
        {
            get
            {
                cacheManagerDataCollection.Clear();
                foreach (CacheManagerNode node in Nodes)
                {
                    cacheManagerDataCollection[node.CacheManagerData.Name] = node.CacheManagerData;
                }
                return cacheManagerDataCollection;
            }
        }

        /// <summary>
        /// Adds a default cache manager.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            CacheManagerData defaultData = new CacheManagerData(SR.DefaultCacheManagerName, CacheManagerNode.expirationPollFreq, CacheManagerNode.maxElementsInCache, CacheManagerNode.numberToRemoveWhenScavenging);
            int defaultNodeIdx = Nodes.Add(new CacheManagerNode(defaultData));
            CacheManagerSettingsNode settingsNode = Parent as CacheManagerSettingsNode;
            if (settingsNode != null)
            {
                settingsNode.DefaultCacheManager = (CacheManagerNode)Nodes[defaultNodeIdx];
            }
        }

		/// <summary>
		/// Adds the <see cref="CacheManagerNode"/>'s menu items and the and Validate node menu item.
		/// </summary>
        protected override void OnAddMenuItems() 
        {
            AddMenuItem(ConfigurationMenuItem.CreateValidateNodeCommand(Site, this));
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.CacheManagerMenuText, new AddChildNodeCommand(Site, typeof(CacheManagerNode)), this, Shortcut.None, SR.CacheManagerStatusText, InsertionPoint.New);
            AddMenuItem(item);
        }

		/// <summary>
		/// Sets the site's name and builds the cache manager nodes.
		/// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DefaultCacheManagerCollectionNodeName;
            BuildCacheManagerNodes();
        }

        private void BuildCacheManagerNodes()
        {
            foreach (CacheManagerData cacheManagerData in cacheManagerDataCollection)
            {
                Nodes.Add(new CacheManagerNode(cacheManagerData));
            }
        }
    }
}