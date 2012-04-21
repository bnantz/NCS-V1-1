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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents a type to be included in the <see cref="XmlSerializerTransformer"/> when serializing and deserializing objects.
    /// </para>
    /// </summary>	
	[Image(typeof(XmlIncludeTypeNode))]
	public class XmlIncludeTypeNode : ConfigurationNode
    {
        private XmlIncludeTypeData xmlIncludeTypeData;

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="XmlIncludeTypeNode"/> class.
        /// </para>
        /// </summary>
        public XmlIncludeTypeNode() : this(new XmlIncludeTypeData(SR.DefaultXmlIncludeTypeNodeName))
        {
        }

        /// <summary>
        /// <para>
        /// Initialize a new instance of the <see cref="XmlIncludeTypeNode"/> class with display name and a <see cref="XmlIncludeTypeData"/> object.
        /// </para>
        /// </summary>
        /// <param name="xmlIncludeTypeData">
        /// <para>The configuration data that this node represents.</para>
        /// </param>
        public XmlIncludeTypeNode(XmlIncludeTypeData xmlIncludeTypeData) : base()
        {
            this.xmlIncludeTypeData = xmlIncludeTypeData;
        }

        /// <summary>
        /// <para>Gets or sets the fully qualified name of a class that implements the <see cref="ITransformer"/> interface.</para>
        /// </summary>
        /// <value>
        /// <para>The fully qualified name of a class that implements the <see cref="ITransformer"/> interface.</para>
        /// </value>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.TypeNameDescription)]
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Object))]
        [TypeValidation]
        public string TypeName
        {
            get { return this.xmlIncludeTypeData.TypeName; }
            set { this.xmlIncludeTypeData.TypeName = value; }
        }

        /// <summary>
        /// <para>Gets the <see cref="StorageProviderData"/> for this node.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="StorageProviderData"/> for this node.</para>
        /// </value>
        [Browsable(false)]
        public virtual XmlIncludeTypeData XmlIncludeTypeData
        {
            get { return this.xmlIncludeTypeData; }
        }

        /// <summary>
        /// <para>Sets the name of node when sited to match the underlying storage name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = xmlIncludeTypeData.Name;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            Debug.Assert(e.Node != null, "The node should be set.");
            xmlIncludeTypeData.Name = e.Node.Name;
        }
    }
}