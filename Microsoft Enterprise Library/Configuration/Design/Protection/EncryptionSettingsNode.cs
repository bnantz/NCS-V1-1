//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Represents a node in a <see cref="IUIHierarchy"/> that contains the <see cref="IKeyAlgorithmPairStorageProvider"/> used for encrypting and decrypting configuration.</para>
    /// </summary>
    [Image(typeof(EncryptionSettingsNode))]
    public class EncryptionSettingsNode : ConfigurationNode
    {
        private KeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="EncryptionSettingsNode"/> class.</para>
        /// </summary>
        public EncryptionSettingsNode() : base()
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="EncryptionSettingsNode"/> class with a <see cref="keyAlgorithmPairStorageProviderData"/> object.</para>
        /// </summary>
        /// <param name="keyAlgorithmPairStorageProviderData"><para>The <see cref="IKeyAlgorithmPairStorageProvider"/> configuration data.</para></param>
        public EncryptionSettingsNode(KeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData) : this()
        {
            if (keyAlgorithmPairStorageProviderData == null)
            {
                throw new ArgumentNullException("keyAlgorithmPairStorageProviderData");
            }
            this.keyAlgorithmPairStorageProviderData = keyAlgorithmPairStorageProviderData;
        }

        /// <summary>
        /// <para>Gets the <see cref="KeyAlgorithmPair"/> for the encryption settings.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="KeyAlgorithmPair"/> for the encryption settings.</para>
        /// </value>
        [ReadOnly(true)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.EncryptionSettingsNodeKeyAlgorithmPairDescription)]
        public string KeyAlgorithmPair
        {
            get { return SR.EncryptionSettingsNodeKeyAlgorithmPairDescription; }
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
        /// <para>Returns the configured <see cref="KeyAlgorithmPairStorageProviderData"/>.</para>
        /// </summary>
        /// <returns>The <see cref="KeyAlgorithmPairStorageProviderData"/>, or <see langword="null"/> (Nothing in Visual Basic) if no storage provider is configured.</returns>
        [Browsable(false)]
        public virtual KeyAlgorithmPairStorageProviderData KeyAlgorithmPairStorageProviderData
        {
            get
            {
                keyAlgorithmPairStorageProviderData = null;
                if (Nodes.Count > 0)
                {
                    KeyAlgorithmStorageProviderNode storageNode = (KeyAlgorithmStorageProviderNode)Nodes[0];
                    keyAlgorithmPairStorageProviderData = storageNode.KeyAlgorithmStorageProviderData;
                }
                return keyAlgorithmPairStorageProviderData;
            }
        }

        /// <summary>
        /// <para>Adds a set of <see cref="ConfigurationMenuItem"/> to the user interface to create any derived types of <see cref="KeyAlgorithmStorageProviderNode"/> at design time.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems ();
            CreateDynamicMenuItems(typeof(KeyAlgorithmStorageProviderNode));
        }

        /// <summary>
        /// <para>Sets the name of the node and displays any <see cref="KeyAlgorithmStorageProviderNode"/> objects that exits in configuration.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.DefaultEncryptionSettingsNodeName;
            if ((Nodes.Count == 0) && (keyAlgorithmPairStorageProviderData != null))
            {
                INodeCreationService service = ServiceHelper.GetNodeCreationService(Site);
                Nodes.Add(service.CreateNode(keyAlgorithmPairStorageProviderData.GetType(), new object[] {keyAlgorithmPairStorageProviderData}));
            }
        }
    }
}