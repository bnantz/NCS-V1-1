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
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents the base class for all storage providers defineing the common information for a storage provider.</para>
    /// </summary>
    [Image(typeof(StorageProviderNode))]
    public abstract class StorageProviderNode : ConfigurationNode
    {
        private StorageProviderData storageProviderData;

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="StorageProviderNode"/> with the runtime configuration data.</para>
        /// </summary>
        /// <param name="data">
        /// <para>The runtime data for the storage provider..</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="data"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        protected StorageProviderNode(StorageProviderData data) : base()
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this.storageProviderData = data;
        }

        /// <summary>
        /// <para>Gets the <see cref="StorageProviderData"/> for this node.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="StorageProviderData"/> for this node.</para>
        /// </value>
        [Browsable(false)]
        public virtual StorageProviderData StorageProvider
        {
            get { return this.storageProviderData; }
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = storageProviderData.Name;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            Debug.Assert(e.Node != null, "The node should be set.");
            storageProviderData.Name = e.Node.Name;
        }
    }
}