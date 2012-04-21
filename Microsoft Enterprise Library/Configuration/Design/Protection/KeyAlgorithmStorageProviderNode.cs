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

using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Represents a <see cref="IKeyAlgorithmPairStorageProvider"/> node in a <see cref="IUIHierarchy"/> used for encrypting and decrypting configuration. This class is abstract.</para>
    /// </summary>
    [Image(typeof(KeyAlgorithmStorageProviderNode))]
    public abstract class KeyAlgorithmStorageProviderNode : ConfigurationNode
    {
        private KeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData;
        private KeyAlgorithmPair keyAlgorithmPair;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="KeyAlgorithmStorageProviderNode"/> class with a <see cref="keyAlgorithmPairStorageProviderData"/> object.</para>
        /// </summary>
        /// <param name="keyAlgorithmPairStorageProviderData">
        /// <para>The runtime configuration data for the <see cref="IKeyAlgorithmPairStorageProvider"/>.</para>
        /// </param>
        protected KeyAlgorithmStorageProviderNode(KeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData) : base()
        {
            this.keyAlgorithmPairStorageProviderData = keyAlgorithmPairStorageProviderData;
        }

        /// <summary>
        /// <para>Gets the runtime configuration data for the <see cref="IKeyAlgorithmPairStorageProvider"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The runtime configuration data for the <see cref="IKeyAlgorithmPairStorageProvider"/>.</para>
        /// </returns>
        [Browsable(false)]
        public virtual KeyAlgorithmPairStorageProviderData KeyAlgorithmStorageProviderData
        {
            get { return keyAlgorithmPairStorageProviderData; }
        }

        /// <summary>
        /// <para>Gets or sets the key algorithm pair.</para>
        /// </summary>
        /// <value>
        /// <para>The key algorithm pair</para>
        /// </value>
        [Browsable(false)]
        public virtual KeyAlgorithmPair KeyAlgorithmPair
        {
            get { return keyAlgorithmPair; }
            set { keyAlgorithmPair = value; }
        }

        /// <summary>
        /// <para>Sets the <see cref="ConfigurationNode.Name"/> of the node when sited.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = keyAlgorithmPairStorageProviderData.Name;
        }

        /// <summary>
        /// <para>Sets the name for the <see cref="KeyAlgorithmPairStorageProviderData"/> object based on the new name.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            Debug.Assert(e.Node != null, "The node should be set.");
            keyAlgorithmPairStorageProviderData.Name = e.Node.Name;
        }

    }
}