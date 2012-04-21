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
    /// <para>Represents the common configuration data for all hash providers.</para>
    /// </summary>
    [XmlRoot("hashProvider", Namespace=CryptographySettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomHashProviderData))]
    [XmlInclude(typeof(HashAlgorithmProviderData))]
    [XmlInclude(typeof(KeyedHashAlgorithmProviderData))]
    public abstract class HashProviderData : ProviderData
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="HashProviderData"/> class.</para>
        /// </summary>
        protected HashProviderData()
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="HashProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name"><para>The name of the provider.</para></param>
        protected HashProviderData(string name) : base(name)
        {
        }
    }
}