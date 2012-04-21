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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Node that represents a Custom CacheStorageNode
    /// </summary>
    public class CustomCacheStorageNode : CacheStorageNode
    {
        private CustomCacheStorageData customCacheStorageData;

        /// <summary>
        /// Creates node with initial data.
        /// </summary>
        public CustomCacheStorageNode() : this(new CustomCacheStorageData(SR.DefaultCacheStorageNodeName))
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomCacheStorageNode"/> with the <see cref="CustomCacheStorageData"/>.
        /// </summary>
        /// <param name="customCacheStorageData">The <see cref="CustomCacheStorageData"/>.</param>
        public CustomCacheStorageNode(CustomCacheStorageData customCacheStorageData) : base(customCacheStorageData)
        {
            this.customCacheStorageData = customCacheStorageData;
        }

        /// <summary>
        /// See <see cref="CustomCacheStorageData.Extensions"/>.
        /// </summary>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.CustomCacheStorageExtensionsDescription)]
        public NameValueItemCollection Extensions
        {
            get { return customCacheStorageData.Extensions; }
        }
    }
}