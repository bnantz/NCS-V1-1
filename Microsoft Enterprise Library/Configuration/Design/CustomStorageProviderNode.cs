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
    /// <para>Represents a node for a <see cref="CustomStorageProviderData"/> implementation of the current configuration.</para>
    /// </summary>
    public class CustomStorageProviderNode : StorageProviderNode
    {
        private CustomStorageProviderData storageProviderData;

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="CustomStorageProviderNode"/>.</para>
        /// </summary>
        public CustomStorageProviderNode() : this(new CustomStorageProviderData(SR.CustomStorageProviderNodeDefaultName))
        {
        }

        /// <summary>
        /// <para>Initalize a new instance of the <see cref="CustomStorageProviderNode"/> with the custom runtime configuration data.</para>
        /// </summary>
        /// <param name="storageProviderData">
        /// <para>A <see cref="CustomStorageProviderData"/> instance containing the configuration data.</para>
        /// </param>
        public CustomStorageProviderNode(CustomStorageProviderData storageProviderData) : base(storageProviderData)
        {
            this.storageProviderData = storageProviderData;
        }

        /// <summary>
        /// <para>Gets the set of custom attributes for the node.</para>
        /// </summary>
        /// <value>
        /// <para>A <see cref="NameValueItemCollection"/> representing the custom attributes.</para>
        /// </value>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.CustomStorageProviderAttributesDescription)]
        public NameValueItemCollection Attributes
        {
            get { return storageProviderData.Attributes; }
        }

        /// <summary>
        /// <para>Gets or sets the fully qualified assembly name of the <see cref="IStorageProviderReader"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The fully qualified assembly name of the <see cref="IStorageProviderReader"/>.</para>
        /// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IStorageProviderReader))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.CustomStorageProviderNodeTypeNameDescription)]
        public virtual string TypeName
        {
            get { return storageProviderData.TypeName; }
            set { storageProviderData.TypeName = value; }
        }
    }
}