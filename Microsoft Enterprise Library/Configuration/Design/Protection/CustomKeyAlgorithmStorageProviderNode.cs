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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Represents a custom key algorithm pair storage provider in a <see cref="IUIHierarchy"/>.</para>
    /// </summary>
    public class CustomKeyAlgorithmStorageProviderNode : KeyAlgorithmStorageProviderNode
    {
        private CustomKeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData;

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="CustomKeyAlgorithmStorageProviderNode"/>.</para>
        /// </summary>
        public CustomKeyAlgorithmStorageProviderNode() : this(new CustomKeyAlgorithmPairStorageProviderData(SR.CustomKeyAlgorithmPairStorageProviderNodeDefaultName))
        {
        }

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="CustomKeyAlgorithmStorageProviderNode"/> with the custom runtime configuration data.</para>
        /// </summary>
        /// <param name="keyAlgorithmPairStorageProviderData">
        /// <para>A <see cref="CustomKeyAlgorithmPairStorageProviderData"/> instance containing the configuration data for the provider.</para>
        /// </param>
        public CustomKeyAlgorithmStorageProviderNode(CustomKeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData) : base(keyAlgorithmPairStorageProviderData)
        {
            this.keyAlgorithmPairStorageProviderData = keyAlgorithmPairStorageProviderData;
        }

        /// <summary>
        /// <para>Gets a <see cref="NameValueItemCollection"/> representing the custom attributes used to configure the provider.</para>
        /// </summary>
        /// <value>
        /// <para>A <see cref="NameValueItemCollection"/> representing the custom attributes used to configure the provider.</para>
        /// </value>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.CustomKeyAlgorithmPairStorageProviderAttributesDescription)]
        public NameValueItemCollection Attributes
        {
            get { return keyAlgorithmPairStorageProviderData.Attributes; }
        }

        /// <summary>
        /// <para>Gets or sets the fully qualified type name of the <see cref="ITransformer"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The fully qualified type name of the <see cref="ITransformer"/>.</para>
        /// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ITransformer))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.CustomKeyAlgorithmPairStorageProviderNodeTypeNameDescription)]
        public virtual string TypeName
        {
            get { return keyAlgorithmPairStorageProviderData.TypeName; }
            set { keyAlgorithmPairStorageProviderData.TypeName = value; }
        }
    }
}