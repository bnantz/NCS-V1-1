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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for an IsolatedStorageCacheStorageData
    /// </summary>
    public class IsolatedStorageCacheStorageNode : CacheStorageNode
    {
        private IsolatedStorageCacheStorageData isolatedStorageCacheStorageData;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public IsolatedStorageCacheStorageNode() : this(new IsolatedStorageCacheStorageData(SR.DefaultIsolatedStorageNodeName))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="isolatedStorageCacheStorageData">Configuration data.</param>
        public IsolatedStorageCacheStorageNode(IsolatedStorageCacheStorageData isolatedStorageCacheStorageData) : base(isolatedStorageCacheStorageData)
        {
            this.isolatedStorageCacheStorageData = isolatedStorageCacheStorageData;
        }

        /// <summary>
        /// Gets the fully qualified assembly name for an <c>IsolatedStorageBackingStore</c>.
        /// </summary>
        [Browsable(false)]
        public override string Type
        {
            get { return isolatedStorageCacheStorageData.TypeName; }
        }

        /// <summary>
        /// See <see cref="IsolatedStorageCacheStorageData.PartitionName"/>.
        /// </summary>
        [SRDescription(SR.Keys.IsolatedStorageAreaNameDescription)]
        [Required]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string PartitionName
        {
            get { return isolatedStorageCacheStorageData.PartitionName; }
            set { isolatedStorageCacheStorageData.PartitionName = value; }
        }
    }
}