//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="HashProviderDataCollection"/>.
    /// </summary>
    [Image(typeof(HashProviderCollectionNode))]
    public class HashProviderCollectionNode : ConfigurationNode
    {
        private HashProviderDataCollection hashProviderDataCollection;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public HashProviderCollectionNode() : this(new HashProviderDataCollection())
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="hashProviderDataCollection">The corresponding runtime configuration data.</param>
        public HashProviderCollectionNode(HashProviderDataCollection hashProviderDataCollection) : base()
        {
            if (hashProviderDataCollection == null)
            {
                throw new ArgumentNullException("hashProviderDataCollection");
            }
            this.hashProviderDataCollection = hashProviderDataCollection;
        }

        /// <summary>
        /// Returns runtime configuration data.
        /// </summary>
        /// <returns>Runtime configuration data.</returns>
        [Browsable(false)]
        public HashProviderDataCollection HashProviderDataCollection
        {
            get
            {
                hashProviderDataCollection.Clear();
                foreach (HashProviderNode node in Nodes)
                {
                    hashProviderDataCollection[node.HashProviderData.Name] = node.HashProviderData;
                }
                return hashProviderDataCollection;
            }
        }

        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.HashProviderCollectionNodeName;
            CreateDynamicNodes(hashProviderDataCollection);
        }

        /// <summary>
        /// <para>Adds the default menu items.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            AddMenuItem(ConfigurationMenuItem.CreateAddChildNodeMenuItem(Site, this, typeof(CustomHashProviderNode), SR.CustomHashProviderNodeName, true));
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.HashAlgorithmProviderNodeName,
                                                                   new AddHashAlgorithmProviderNodeCommand(Site, typeof(HashAlgorithmProviderNode)),
                                                                   this,
                                                                   Shortcut.None,
                                                                   SR.GenericCreateStatusText(SR.HashAlgorithmProviderNodeName),
                                                                   InsertionPoint.New);
            AddMenuItem(item);
        }
    }
}