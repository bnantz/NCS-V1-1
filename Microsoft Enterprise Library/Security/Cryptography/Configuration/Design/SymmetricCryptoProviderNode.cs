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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="SymmetricCryptoProviderData"/>.
    /// </summary>
    [Image(typeof(SymmetricCryptoProviderNode))]
    public abstract class SymmetricCryptoProviderNode : ConfigurationNode
    {
        private SymmetricCryptoProviderData symmetricCryptoProviderData;

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="symmetricCryptoProviderData">The corresponding runtime configuration data.</param>
        protected SymmetricCryptoProviderNode(SymmetricCryptoProviderData symmetricCryptoProviderData) : base( /*SR.SymmetricCryptoProviderNodeName*/)
        {
            if (symmetricCryptoProviderData == null)
            {
                throw new ArgumentNullException("symmetricCryptoProviderData");
            }
            this.symmetricCryptoProviderData = symmetricCryptoProviderData;
        }

        /// <summary>
        /// Returns runtime configuration data.
        /// </summary>
        /// <returns>Runtime configuration data.</returns>
        [Browsable(false)]
        public virtual SymmetricCryptoProviderData SymmetricCryptoProviderData
        {
            get { return this.symmetricCryptoProviderData; }
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = symmetricCryptoProviderData.Name;
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            symmetricCryptoProviderData.Name = e.Node.Name;
        }
    }
}