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

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a configuration section in configuration.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>configurationSection</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("configurationSection", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class ReadOnlyConfigurationSectionData : ConfigurationSectionData
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionData"/> class.</para>
        /// </summary>
        public ReadOnlyConfigurationSectionData() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the section.</para>
        /// </param>
        public ReadOnlyConfigurationSectionData(string name) : this(name, false, null, null)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the section.</para>
        /// </param>
        /// <param name="encrypt">
        /// <para>Determines if the section is encrypted or not.</para>
        /// </param>
        public ReadOnlyConfigurationSectionData(string name, bool encrypt) : this(name, encrypt, null, null)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionData"/> class with a name, a cache, a storage provider, a data protection provider and a data transformer.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the section.</para>
        /// </param>
        /// <param name="encrypt">
        /// <para>Determines if the section is encrypted or not.</para>
        /// </param>
        /// <param name="storageProvider">
        /// <para>The <see cref="StorageProviderData"/> used for the configuration section.</para>
        /// </param>        
        /// <param name="transformer">
        /// <para>The <see cref="TransformerData"/> used for the section.</para>
        /// </param>                
        /// <seealso cref="StorageProviderData"/>        
        /// <seealso cref="TransformerData"/>
        public ReadOnlyConfigurationSectionData(string name, bool encrypt, StorageProviderData storageProvider, TransformerData transformer) : base(name, encrypt, storageProvider, transformer)
        {
        }
    }
}