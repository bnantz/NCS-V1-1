//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents extensible configuration data for a custom
    /// roles provider.
    /// </summary>
    [XmlRoot("rolesProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    public class CustomRolesProviderData : RolesProviderData
    {
        private NameValueItemCollection extensions;
        private string typeName;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="CustomRolesProviderData"/> class.
        /// </summary>
        public CustomRolesProviderData() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="CustomRolesProviderData"/> class with
        /// the specified name and type name.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        /// <param name="typeName">The type name of a class that implements <see cref="IRolesProvider"/></param>
        public CustomRolesProviderData(string name, string typeName) : base(name)
        {
            this.typeName = typeName;
            this.extensions = new NameValueItemCollection();
        }

        /// <summary>
        /// <para>Gets the <seealso cref="Extensions"/> defined as a <see cref="NameValueItemCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="XmlAttributeAttribute"/>s defined for the data protection provider. The default is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </value>
        /// <remarks>
        /// <para>The name of the attribute maps to the key in the collection, and the value is the value.</para>
        /// </remarks>
        [XmlArray("extensions")]
        [XmlArrayItem("extension", typeof(NameValueItem))]
        public NameValueItemCollection Extensions
        {
            get { return this.extensions; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="System.Type"/> name of the provider.</para>
        /// </summary>
        /// <value>
        /// <para>The type name of the provider. The default is an empty string.</para>
        /// </value>
        [XmlAttribute("type")]
        public override string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
    }
}