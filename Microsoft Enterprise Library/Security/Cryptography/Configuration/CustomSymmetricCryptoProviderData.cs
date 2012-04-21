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

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for a custom symmetric provider.</para>
    /// </summary>
    [XmlRoot("symmetricCryptoProvider", Namespace=CryptographySettings.ConfigurationNamespace)]
    public class CustomSymmetricCryptoProviderData : SymmetricCryptoProviderData
    {
        private NameValueItemCollection extensions;
        private string typeName;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="CustomSymmetricCryptoProviderData"/> class.</para>
        /// </summary>
        public CustomSymmetricCryptoProviderData() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="CustomSymmetricCryptoProviderData"/> class with the specified name and type name.</para>
        /// </summary>
        /// <param name="name"><para>The name of the provider.</para></param>
        /// <param name="typeName"><para>The type name of a class that implements <see cref="ISymmetricCryptoProvider"/>.</para></param>
        public CustomSymmetricCryptoProviderData(string name, string typeName) : base(name)
        {
            this.typeName = typeName;
            this.extensions = new NameValueItemCollection();
        }

        /// <summary>
        /// <para>Gets the <seealso cref="Extensions"/> defined as a 
        /// <see cref="NameValueItemCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>A <see cref="NameValueItemCollection"/>.</para>
        /// </value>
        [XmlArray("extensions", Namespace=CryptographySettings.ConfigurationNamespace)]
        [XmlArrayItem("extension", typeof(NameValueItem), Namespace=CryptographySettings.ConfigurationNamespace)]
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