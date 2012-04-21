//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design
{
    /// <summary>
    /// Node representing configuration from <see cref="CachingStoreProviderData"/>.
    /// </summary>
    public class CachingStoreProviderNode : SecurityCacheProviderNode
    {
        private const int absoluteExpiration = 60;
        private const int slidingExpiration = 10;
        private CachingStoreProviderData cachingStoreProviderData;
        private CacheManagerNode cacheManagerNode;
        private ConfigurationNodeChangedEventHandler onCacheManagerNodeRemoved;
        private ConfigurationNodeChangedEventHandler onCacheManagerNodeRenamed;

        public CachingStoreProviderNode() : this(new CachingStoreProviderData(SR.SecurityInstance, slidingExpiration, absoluteExpiration))
        {
        }

        public CachingStoreProviderNode(CachingStoreProviderData data) : base(data)
        {
            this.cachingStoreProviderData = data;
            this.onCacheManagerNodeRemoved = new ConfigurationNodeChangedEventHandler(OnCacheManagerNodeRemoved);
            this.onCacheManagerNodeRenamed = new ConfigurationNodeChangedEventHandler(OnCacheManagerNodeRenamed);
        }

        /// <summary>
        /// See <see cref="CachingStoreProviderData.AbsoluteExpiration"/>.
        /// </summary>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription(SR.Keys.AbsoluteExpirationDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public int AbsoluteExpiration
        {
            get { return cachingStoreProviderData.AbsoluteExpiration; }
            set { cachingStoreProviderData.AbsoluteExpiration = value; }
        }

        /// <summary>
        /// See <see cref="CachingStoreProviderData.SlidingExpiration"/>.
        /// </summary>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription(SR.Keys.SlidingExpirationDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public int SlidingExpiration
        {
            get { return cachingStoreProviderData.SlidingExpiration; }
            set { cachingStoreProviderData.SlidingExpiration = value; }
        }

        /// <summary>
        /// See <see cref="CachingStoreProviderData.CacheManager"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.CacheInstanceDescription)]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CacheManagerNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public CacheManagerNode CacheManager
        {
            get { return cacheManagerNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                this.cacheManagerNode = (CacheManagerNode)service.CreateReference(cacheManagerNode, value, onCacheManagerNodeRemoved, onCacheManagerNodeRenamed);
                cachingStoreProviderData.CacheManager = (cacheManagerNode == null) ? String.Empty : cacheManagerNode.Name;
            }
        }

        /// <summary>
        /// Returns the fully qualified assembly name of a <see cref="CachingStoreProvider"/>
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return typeof(CachingStoreProvider).AssemblyQualifiedName; }
        }

        /// <summary>
        /// See <see cref="ConfigurationNode.ResolveNodeReferences"/>.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            CacheManagerCollectionNode categoryCollectionNode = Hierarchy.FindNodeByType(typeof(CacheManagerCollectionNode)) as CacheManagerCollectionNode;
            Debug.Assert(categoryCollectionNode != null, "How is it that the cache managers are not there?");
            if (categoryCollectionNode != null)
            {
                CacheManager = Hierarchy.FindNodeByName(categoryCollectionNode, this.cachingStoreProviderData.CacheManager) as CacheManagerNode;
            }
        }

        /// <summary>
        /// Initializes the Caching defaults.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            CreateCacheManagerSettingsNodeNode();
            if (cachingStoreProviderData.CacheManager == null)
            {
                cachingStoreProviderData.CacheManager = SR.DefaultCacheManager;
            }
            ResolveNodeReferences();
        }

        private void OnCacheManagerNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.cacheManagerNode = null;
        }

        private void OnCacheManagerNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            cachingStoreProviderData.CacheManager = e.Node.Name;
        }

        private void CreateCacheManagerSettingsNodeNode()
        {
            if (!CacheManagerSettingsNodeExists())
            {
                AddConfigurationSectionCommand cmd = new AddConfigurationSectionCommand(Site, typeof(CacheManagerSettingsNode), CacheManagerSettings.SectionName);
                cmd.Execute(Hierarchy.RootNode);
            }
        }

        private bool CacheManagerSettingsNodeExists()
        {
            CacheManagerSettingsNode node = Hierarchy.FindNodeByType(typeof(CacheManagerSettingsNode)) as CacheManagerSettingsNode;
            return (node != null);
        }
    }
}