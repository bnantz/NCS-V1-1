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
    /// Represents a collection of <see cref="SymmetricCryptoProviderNode"/> objects.
    /// </summary>
    [Image(typeof(SymmetricCryptoProviderCollectionNode))]
    public class SymmetricCryptoProviderCollectionNode : ConfigurationNode
    {
        private SymmetricCryptoProviderDataCollection symmetricCryptoProviderDataCollection;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public SymmetricCryptoProviderCollectionNode() : this(new SymmetricCryptoProviderDataCollection())
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="symmetricCryptoProviderDataCollection">The corresponding runtime configuration data.</param>
        public SymmetricCryptoProviderCollectionNode(SymmetricCryptoProviderDataCollection symmetricCryptoProviderDataCollection) : base()
        {
            if (symmetricCryptoProviderDataCollection == null)
            {
                throw new ArgumentNullException("symmetricCryptoProviderDataCollection");
            }
            this.symmetricCryptoProviderDataCollection = symmetricCryptoProviderDataCollection;
        }

        /// <summary>
        /// Returns runtime configuration data.
        /// </summary>
        /// <returns>Runtime configuration data.</returns>
        [Browsable(false)]
        public SymmetricCryptoProviderDataCollection SymmetricCryptoProviderDataCollection
        {
            get
            {
                symmetricCryptoProviderDataCollection.Clear();
                foreach (SymmetricCryptoProviderNode node in this.Nodes)
                {
                    this.symmetricCryptoProviderDataCollection[node.SymmetricCryptoProviderData.Name] = node.SymmetricCryptoProviderData;
                }
                return this.symmetricCryptoProviderDataCollection;
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
            Site.Name = SR.SymmetricProviderCollectionNodeName;
            CreateDynamicNodes(symmetricCryptoProviderDataCollection);
        }

        /// <summary>
        /// <para>Adds the default menu items.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            AddMenuItem(ConfigurationMenuItem.CreateAddChildNodeMenuItem(Site, this, typeof(CustomSymmetricCryptoProviderNode), SR.CustomSymmetricCryptoProviderNodeName, true));
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.SymmetricAlgorithmProviderNodeName,
                                                                   new AddSymmetricAlgorithmProviderNodeCommand(Site, typeof(SymmetricAlgorithmProviderNode)),
                                                                   this,
                                                                   Shortcut.None,
                                                                   SR.GenericCreateStatusText(SR.SymmetricAlgorithmProviderNodeName),
                                                                   InsertionPoint.New);
            AddMenuItem(item);
            item = new ConfigurationMenuItem(SR.DpapiSymmetricCryptoProviderNodeName,
                                             new AddDpapiSymmetricProviderNodeCommand(Site, typeof(DpapiSymmetricCryptoProviderNode)),
                                             this,
                                             Shortcut.None,
                                             SR.GenericCreateStatusText(SR.DpapiSymmetricCryptoProviderNodeName),
                                             InsertionPoint.New);
            AddMenuItem(item);
        }
    }
}