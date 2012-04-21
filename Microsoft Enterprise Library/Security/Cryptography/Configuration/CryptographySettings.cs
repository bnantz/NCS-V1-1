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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for cryptography.</para>
    /// </summary>
    [XmlRoot("enterpriseLibrary.securityCryptographySettings", Namespace=CryptographySettings.ConfigurationNamespace)]
    public class CryptographySettings
    {
        /// <summary>
        /// Configuration section name for cryptography settings.
        /// </summary>
        public const string SectionName = "securityCryptographyConfiguration";

        private HashProviderDataCollection hashProviders;
        private SymmetricCryptoProviderDataCollection symmetricCryptoProviders;

        /// <summary>
        /// <para>Gets the Xml namespace for this root node.</para>
        /// </summary>
        /// <value>
        /// <para>The Xml namespace for this root node.</para>
        /// </value>
        public const string ConfigurationNamespace = "http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/security/cryptography";

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CryptographySettings"/> class.</para>
        /// </summary>
        public CryptographySettings()
        {
            this.hashProviders = new HashProviderDataCollection();
            this.symmetricCryptoProviders = new SymmetricCryptoProviderDataCollection();
        }

        /// <summary>
        /// <para>Gets and sets the hash providers.</para>
        /// </summary>
        /// <value>
        /// <para>The hash providers.</para>
        /// </value>
        [XmlArray("hashProviders", Namespace=CryptographySettings.ConfigurationNamespace)]
        [XmlArrayItem("hashProvider", typeof(HashProviderData), Namespace=CryptographySettings.ConfigurationNamespace)]
        public HashProviderDataCollection HashProviders
        {
            get { return this.hashProviders; }
        }

        /// <summary>
        /// <para>Gets the symmetric cryptography providers.</para>
        /// </summary>
        /// <value>
        /// <para>The symmetric cryptography providers.</para>
        /// </value>
        [XmlArray("symmetricCryptoProviders", Namespace=CryptographySettings.ConfigurationNamespace)]
        [XmlArrayItem("symmetricCryptoProvider", typeof(SymmetricCryptoProviderData), Namespace=CryptographySettings.ConfigurationNamespace)]
        public SymmetricCryptoProviderDataCollection SymmetricCryptoProviders
        {
            get { return this.symmetricCryptoProviders; }
        }
    }
}