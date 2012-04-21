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
    /// <para>Represents the base class for all transformers defineing the common information for a transformer.</para>
    /// </summary>
    [Image(typeof(TransformerNode))]
    public abstract class TransformerNode : ConfigurationNode
    {
        private TransformerData transformerData;

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="TransformerNode"/> with the runtime configuration data.</para>
        /// </summary>
        /// <param name="transformerData">
        /// <para>The runtime data for the transformer..</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="transformerData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        protected TransformerNode(TransformerData transformerData) : base()
        {
            if (transformerData == null)
            {
                throw new ArgumentNullException("data");
            }
            this.transformerData = transformerData;
        }

        /// <summary>
        /// <para>Gets the <see cref="StorageProviderData"/> for this node.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="StorageProviderData"/> for this node.</para>
        /// </value>
        [Browsable(false)]
        public virtual TransformerData TransformerData
        {
            get { return this.transformerData; }
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = transformerData.Name;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            Debug.Assert(e.Node != null, "The node should be set.");
            transformerData.Name = e.Node.Name;
        }
    }
}