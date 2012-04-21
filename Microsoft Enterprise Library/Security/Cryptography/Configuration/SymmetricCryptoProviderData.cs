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
    /// <para>Represents the common configuration data for all symmetric crypto providers.</para>
    /// </summary>
    [XmlRoot("symmetricCryptoProvider", Namespace=CryptographySettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomSymmetricCryptoProviderData))]
    [XmlInclude(typeof(SymmetricAlgorithmProviderData))]
    [XmlInclude(typeof(DpapiSymmetricCryptoProviderData))]
    public abstract class SymmetricCryptoProviderData : ProviderData
    {
        /// <summary>
        /// <para>Initializes a new instance of <see cref="SymmetricCryptoProviderData"/> class.</para>
        /// </summary>
        protected SymmetricCryptoProviderData()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of <see cref="SymmetricCryptoProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name"><para>The name of the provider.</para></param>
        protected SymmetricCryptoProviderData(string name) : base(name)
        {
        }

    }
}