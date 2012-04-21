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
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design
{
	/// <summary>
	/// Node that represents a CacheSymmetricEncryptionProviderData
	/// </summary>
	[ServiceDependency(typeof(ILinkNodeService))]
    public class SymmetricStorageEncryptionNode : StorageEncryptionNode
	{
		private SymmetricStorageEncryptionProviderData symmetricStorageEncryptionProviderData;
		private SymmetricCryptoProviderNode symmetricProvider;
		private ConfigurationNodeChangedEventHandler onSymmetricProviderRemoved;
		private ConfigurationNodeChangedEventHandler onSymmetricProviderRenamed;

		/// <summary>
		/// Creates node with initial data.
		/// </summary>
		public SymmetricStorageEncryptionNode() : this(new SymmetricStorageEncryptionProviderData("Storage Encryption"))
		{
		}

		/// <summary>
		/// Creates node with sepecifed configuration data.
		/// </summary>
		/// <param name="data">The configuration data.</param>
		public SymmetricStorageEncryptionNode(SymmetricStorageEncryptionProviderData data) : base(data)  // TODO - SR
		{
			this.symmetricStorageEncryptionProviderData = data;
			this.onSymmetricProviderRemoved += new ConfigurationNodeChangedEventHandler(this.OnSymmetricProviderRemoved);
			this.onSymmetricProviderRenamed += new ConfigurationNodeChangedEventHandler(this.OnSymmetricProviderRenamed);

		}

		/// <summary>
		/// Gets or sets the symmetric crypto instance that will be used to encrypt and decrypt storage data.
		/// </summary>
		/// <value>An <see cref="SymmetricCryptoProviderNode"/> object.</value>
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
		[ReferenceType(typeof(SymmetricCryptoProviderNode))]
		[Required]
		public SymmetricCryptoProviderNode SymmetricProvider
		{
			get { return this.symmetricProvider; }
			set
			{
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
			    Debug.Assert(service != null, "Could not get the ILinkNodeService");
                this.symmetricProvider = (SymmetricCryptoProviderNode)service.CreateReference(symmetricProvider, value, onSymmetricProviderRemoved, onSymmetricProviderRenamed);
                this.symmetricStorageEncryptionProviderData.SymmetricInstance  = string.Empty;
                if (this.symmetricProvider != null)
                {
                    this.symmetricStorageEncryptionProviderData.SymmetricInstance  = this.symmetricProvider.Name;
                }
			}
		}

        /// <summary>
        /// Gets the storage encryption data based on the Name on the user interface
        /// </summary>
        /// <returns>Storage encryption provider data read from user input</returns>
		public override StorageEncryptionProviderData StorageEncryptionProviderData
		{
            get
            {
                return base.StorageEncryptionProviderData;
            }
		}

		/// <summary>
		/// Resolves the symmetric crypto node reference.
		/// </summary>
		public override void ResolveNodeReferences()
		{
            base.ResolveNodeReferences();
            SymmetricCryptoProviderCollectionNode collectionNode = Hierarchy.FindNodeByType(typeof(SymmetricCryptoProviderCollectionNode)) as SymmetricCryptoProviderCollectionNode;
            SymmetricProvider = Hierarchy.FindNodeByName(collectionNode, this.symmetricStorageEncryptionProviderData.SymmetricInstance) as SymmetricCryptoProviderNode;
			
		}

        /// <summary>
        /// Initializes the cryptography configuration section in the configuration tree
        /// </summary>
		protected override void AddDefaultChildNodes()
		{
			base.AddDefaultChildNodes();

			CreateCryptographySettingsNode();
		}

        private void CreateCryptographySettingsNode()
        {
            if (!CryptographySettingsNodeExists())
            {
                AddConfigurationSectionCommand cmd = new AddConfigurationSectionCommand(Site, typeof(CryptographySettingsNode), CryptographySettings.SectionName);
                cmd.Execute(Hierarchy.RootNode);
            }
        }

        private bool CryptographySettingsNodeExists()
        {
            CryptographySettingsNode node = Hierarchy.FindNodeByType(typeof(CryptographySettingsNode)) as CryptographySettingsNode;
            return (node != null);
        }

		private void OnSymmetricProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
		{
			this.SymmetricProvider = null;
		}

		private void OnSymmetricProviderRenamed(object sender, ConfigurationNodeChangedEventArgs args)
		{
		    symmetricStorageEncryptionProviderData.SymmetricInstance = args.Node.Name;
		}
	}
}