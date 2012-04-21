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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Node that represents a StorageEncryptionNode
    /// </summary>
    [Image(typeof(StorageEncryptionNode))]
    public abstract class StorageEncryptionNode : ConfigurationNode
    {
        private StorageEncryptionProviderData data;

        /// <summary>
        /// Creates node with sepecifed display name and configuration data.
        /// </summary>
        /// <param name="data">The configuration data.</param>
        protected StorageEncryptionNode(StorageEncryptionProviderData data) : base()
        {
            this.data = data;
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual StorageEncryptionProviderData StorageEncryptionProviderData
        {
            get { return data; }
        }

        /// <summary>
        /// <para>Set the name of the node from the data.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = data.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            data.Name = e.Node.Name;
        }
    }
}