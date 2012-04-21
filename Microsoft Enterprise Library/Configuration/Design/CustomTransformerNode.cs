//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a node for a <see cref="CustomTransformerData"/> implementation of the current configuration.</para>
    /// </summary>
    public class CustomTransformerNode : TransformerNode
    {
        private CustomTransformerData transformerData;

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="CustomTransformerNode"/>..</para>
        /// </summary>
        public CustomTransformerNode() : this(new CustomTransformerData(SR.CustomTransformerNodeDefaultName))
        {
        }

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="CustomTransformerNode"/> with the custom runtime configuration data.</para>
        /// </summary>
        /// <param name="transformerData">
        /// <para>A <see cref="CustomTransformerData"/> instance containing the configuration data.</para>
        /// </param>
        public CustomTransformerNode(CustomTransformerData transformerData) : base(transformerData)
        {
            this.transformerData = transformerData;
        }

        /// <summary>
        /// <para>Gets the set of custom attributes for the node.</para>
        /// </summary>
        /// <value>
        /// <para>A <see cref="NameValueItemCollection"/> representing the custom attributes.</para>
        /// </value>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.CustomTransformerAttributesDescription)]
        public NameValueItemCollection Attributes
        {
            get { return transformerData.Attributes; }
        }

        /// <summary>
        /// <para>Gets or sets the fully qualified assembly name of the <see cref="ITransformer"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The fully qualified assembly name of the <see cref="ITransformer"/>.</para>
        /// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ITransformer))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.CustomTransformerNodeTypeNameDescription)]
        public virtual string TypeName
        {
            get { return transformerData.TypeName; }
            set { transformerData.TypeName = value; }
        }
    }
}