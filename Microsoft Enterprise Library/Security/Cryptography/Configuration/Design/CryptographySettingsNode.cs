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
    /// Provides designtime configuration for <see cref="CryptographySettings"/>.
    /// </summary>
    [Image(typeof(CryptographySettingsNode))]
    public class CryptographySettingsNode : ConfigurationNode
    {
        private CryptographySettings cryptographySettings;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public CryptographySettingsNode() : this(new CryptographySettings())
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="data">The corresponding runtime configuration data.</param>
        public CryptographySettingsNode(CryptographySettings data) : base()
        {
            this.cryptographySettings = data;
        }

        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Returns runtime configuration data.
        /// </summary>
        /// <returns>Runtime configuration data.</returns>
        [Browsable(false)]
        public CryptographySettings CryptographySettings
        {
            get
            {
                GetHashProviderDataCollection();
                GetSymmetricCryptoProviderDataCollection();
                return cryptographySettings;
            }
        }

        private void GetHashProviderDataCollection()
        {
            HashProviderCollectionNode hashProviderCollectionNode = Hierarchy.FindNodeByType(typeof(HashProviderCollectionNode)) as HashProviderCollectionNode;
            if (hashProviderCollectionNode == null) return;
            if (Object.ReferenceEquals(cryptographySettings.HashProviders, hashProviderCollectionNode.HashProviderDataCollection)) return;

            cryptographySettings.HashProviders.Clear();
            foreach (HashProviderData hashProviderData in hashProviderCollectionNode.HashProviderDataCollection)
            {
                cryptographySettings.HashProviders[hashProviderData.Name] = hashProviderData;
            }
        }

        private void GetSymmetricCryptoProviderDataCollection()
        {
            SymmetricCryptoProviderCollectionNode symmetricCryptoProviderCollectionNode = Hierarchy.FindNodeByType(typeof(SymmetricCryptoProviderCollectionNode)) as SymmetricCryptoProviderCollectionNode;
            if (symmetricCryptoProviderCollectionNode == null) return;
            if (Object.ReferenceEquals(cryptographySettings.SymmetricCryptoProviders, symmetricCryptoProviderCollectionNode.SymmetricCryptoProviderDataCollection)) return;

            cryptographySettings.SymmetricCryptoProviders.Clear();
            foreach (SymmetricAlgorithmProviderData symmetricAlgorithmProviderData in symmetricCryptoProviderCollectionNode.SymmetricCryptoProviderDataCollection)
            {
                cryptographySettings.SymmetricCryptoProviders[symmetricAlgorithmProviderData.Name] = symmetricAlgorithmProviderData;
            }
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.CryptographyNodeName;
            Nodes.Add(new HashProviderCollectionNode(cryptographySettings.HashProviders));
            Nodes.Add(new SymmetricCryptoProviderCollectionNode(cryptographySettings.SymmetricCryptoProviders));
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            AddMenuItem(typeof(HashProviderCollectionNode), SR.HashProviderCollectionCommandName);
            AddMenuItem(typeof(SymmetricCryptoProviderCollectionNode), SR.SymmetricProviderCollectionCommandName);
        }

        private void AddMenuItem(Type type, string text)
        {
            ConfigurationMenuItem item = new ConfigurationMenuItem(text,
                new AddChildNodeCommand(Site, type),
                this,
                Shortcut.None,
                SR.GenericCreateStatusText(text),
                InsertionPoint.New);
            bool contains = Hierarchy.ContainsNodeType(this, type);
            item.Enabled = !contains;
            AddMenuItem(item);
        }
    }
}