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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// The design time representation of the runtime configuration data
    /// for an authentication provider.
    /// </summary>
    [Image(typeof(SecurityCacheProviderCollectionNode))]
    public class SecurityCacheProviderCollectionNode : ConfigurationNode
    {
        private SecurityCacheProviderDataCollection securityCacheProviderDataCollection;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SecurityCacheProviderCollectionNode"/> class.
        /// </summary>
        public SecurityCacheProviderCollectionNode() : this(new SecurityCacheProviderDataCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SecurityCacheProviderCollectionNode"/>
        /// class from the specified data.
        /// </summary>
        /// <param name="securityCacheProviderDataCollection">The configuration data for a security cache provider collection.</param>
        public SecurityCacheProviderCollectionNode(SecurityCacheProviderDataCollection securityCacheProviderDataCollection) : base()
        {
            this.securityCacheProviderDataCollection = securityCacheProviderDataCollection;
        }

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
        public virtual SecurityCacheProviderDataCollection SecurityCacheProviderDataCollection
        {
            get
            {
                securityCacheProviderDataCollection.Clear();
                foreach (SecurityCacheProviderNode node in this.Nodes)
                {
                    securityCacheProviderDataCollection[node.SecurityCacheProviderData.Name] = node.SecurityCacheProviderData;
                }
                return securityCacheProviderDataCollection;
            }
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.SecurityCacheCollectionNodeCommandName;
            CreateDynamicNodes(securityCacheProviderDataCollection);
        }

        protected override void OnAddMenuItems()
        {
            ConfigurationMenuItem.CreateValidateNodeCommand(Site, this);
            CreateDynamicMenuItems(typeof(SecurityCacheProviderNode));
        }
    }
}