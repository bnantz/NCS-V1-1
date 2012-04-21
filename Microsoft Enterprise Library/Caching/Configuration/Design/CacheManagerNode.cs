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
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Node representing configuration from <see cref="CacheManagerData"/>.
    /// </summary>
    [Image(typeof(CacheManagerNode))]
    public class CacheManagerNode : ConfigurationNode
    {
        internal const int expirationPollFreq = 60;
        internal const int maxElementsInCache = 1000;
        internal const int numberToRemoveWhenScavenging = 10;

        private CacheManagerData cacheManagerData;
        private readonly string NullBackingStoreTypeName = typeof(NullBackingStore).AssemblyQualifiedName;

        /// <summary>
        /// Creates node with initial data.
        /// </summary>
        public CacheManagerNode() : this(new CacheManagerData(SR.DefaultCacheManagerNodeName, expirationPollFreq, maxElementsInCache, numberToRemoveWhenScavenging))
        {
        }

		/// <summary>
		/// Initializes  new instance of the <see cref="CacheManagerNode"/> class with the given settings data.
		/// </summary>
		/// <param name="cacheManagerData">The settings objet to use.</param>
        public CacheManagerNode(CacheManagerData cacheManagerData) : base()
        {
            if (cacheManagerData == null)
            {
                throw new ArgumentNullException("cacheManagerData");
            }
            this.cacheManagerData = cacheManagerData;
        }

        /// <summary>
        /// See <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData.ExpirationPollFrequencyInSeconds"/>.
        /// </summary>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription(SR.Keys.ExpirationPollFrequencyInSecondsDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public int ExpirationPollFrequencyInSeconds
        {
            get { return cacheManagerData.ExpirationPollFrequencyInSeconds; }
            set { cacheManagerData.ExpirationPollFrequencyInSeconds = value; }
        }

        /// <summary>
        /// See <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData.MaximumElementsInCacheBeforeScavenging"/>.
        /// </summary>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription(SR.Keys.MaximumElementsInCacheBeforeScavengingDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public int MaximumElementsInCacheBeforeScavenging
        {
            get { return cacheManagerData.MaximumElementsInCacheBeforeScavenging; }
            set { cacheManagerData.MaximumElementsInCacheBeforeScavenging = value; }
        }

        /// <summary>
        /// See <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheManagerData.NumberToRemoveWhenScavenging"/>
        /// </summary>
        [Required]
        [AssertRange(0, RangeBoundaryType.Inclusive, Int32.MaxValue, RangeBoundaryType.Inclusive)]
        [SRDescription(SR.Keys.NumberToRemoveWhenScavengingDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public int NumberToRemoveWhenScavenging
        {
            get { return cacheManagerData.NumberToRemoveWhenScavenging; }
            set { cacheManagerData.NumberToRemoveWhenScavenging = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual CacheManagerData CacheManagerData
        {
            get
            {
                cacheManagerData.CacheStorage = GetCacheStorageData();
                return cacheManagerData;
            }
        }

		/// <summary>
		/// Creates a default <see cref="CacheStorageNode"/> when this node is sited.
		/// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = cacheManagerData.Name;
            if (cacheManagerData.CacheStorage != null && cacheManagerData.CacheStorage.Name != null && cacheManagerData.CacheStorage.TypeName != NullBackingStoreTypeName)
            {
                INodeCreationService service = GetService(typeof(INodeCreationService)) as INodeCreationService;
                Debug.Assert(service != null, "Could not get the INodeCreationService.");
                ConfigurationNode node = service.CreateNode(cacheManagerData.CacheStorage.GetType(), new object[] {cacheManagerData.CacheStorage});
                Nodes.Add(node);
            }
        }

		/// <summary>
		/// <para>Raises the Renamed event.</para>
		/// </summary>
		/// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
		protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            cacheManagerData.Name = e.Node.Name;
        }

        /// <summary>
        /// <para>Adds the default menu items.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            CreateDynamicMenuItems(typeof(CacheStorageNode));
        }

        private CacheStorageData GetCacheStorageData()
        {
            CacheStorageData cacheStorageData;

            if (Nodes.Count == 0)
            {
                cacheStorageData = new CustomCacheStorageData(SR.NullStorageName, NullBackingStoreTypeName);
            }
            else
            {
                cacheStorageData = ((CacheStorageNode)Nodes[0]).CacheStorageData;
            }

            return cacheStorageData;
        }

    }
}