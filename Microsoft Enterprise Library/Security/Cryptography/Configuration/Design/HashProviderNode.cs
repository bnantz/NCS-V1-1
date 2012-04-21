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
    /// Abstract class for nodes which provide designtime configuration for classes which derive from
    /// <see cref="HashProviderData"/>.
    /// </summary>
    [Image(typeof(HashProviderNode))]
    public abstract class HashProviderNode : ConfigurationNode
    {
        private HashProviderData hashProviderData;

        /// <summary>
        /// Initializes with a defined display name and with defined configuration.
        /// </summary>
        /// <param name="hashProviderData"></param>
        protected HashProviderNode(HashProviderData hashProviderData) : base( /*SR.HashProviderNodeName*/)
        {
            if (hashProviderData == null)
            {
                throw new ArgumentNullException("hashProviderData");
            }
            this.hashProviderData = hashProviderData;
        }

        /// <summary>
        /// Returns runtime configuration data.
        /// </summary>
        /// <returns>Runtime configuration data.</returns>
        [Browsable(false)]
        public virtual HashProviderData HashProviderData
        {
            get { return this.hashProviderData; }
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = hashProviderData.Name;
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            hashProviderData.Name = e.Node.Name;
        }
    }
}