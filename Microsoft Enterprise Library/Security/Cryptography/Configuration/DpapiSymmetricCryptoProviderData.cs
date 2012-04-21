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
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for a DPAPI Symmetric Cryptography Provider.</para>
    /// </summary>
    [XmlRoot("symmetricCryptoProvider", Namespace=CryptographySettings.ConfigurationNamespace)]
    public class DpapiSymmetricCryptoProviderData : SymmetricCryptoProviderData
    {
        private DpapiSettingsData data;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DpapiSymmetricCryptoProviderData"/> class.</para>
        /// </summary>
        public DpapiSymmetricCryptoProviderData()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DpapiSymmetricCryptoProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">The name for the provider.</param>
        public DpapiSymmetricCryptoProviderData(string name) : this(name, null)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DpapiSymmetricCryptoProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">The name for the <see cref="DpapiSymmetricCryptoProviderData"/>.</param>
        /// <param name="data">The <see cref="DpapiSettingsData"/> for the provider.</param>
        public DpapiSymmetricCryptoProviderData(string name, DpapiSettingsData data) : base(name)
        {
            this.data = data;
        }

        /// <summary>
        /// <para>Gets or sets the DPAPI settings.</para>
        /// </summary>
        /// <value>
        /// <para>The DPAPI settings.</para>
        /// </value>
        [XmlElement("dpapiSettings")]
        public DpapiSettingsData DataProtectionMode
        {
            get { return this.data; }
            set { this.data = value; }
        }

        /// <summary>
        /// Gets the assembly qualified name for this provider.
        /// </summary>
        /// <value>
        /// <para>Alwasy return the <see cref="Type.AssemblyQualifiedName"/> of <see cref="DpapiSymmetricCryptoProvider"/>.</para>
        /// </value>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(DpapiSymmetricCryptoProvider).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}