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
    [XmlInclude(typeof(ReadOnlyConfigurationSectionData))]
    public class ConfigurationSectionData
    {
        private StorageProviderData storageProvider;
        private TransformerData transformer;
        private string name;
        private bool encrypt;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionData"/> class.</para>
        /// </summary>
        public ConfigurationSectionData() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationSectionData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the section.</para>
        /// </param>
        public ConfigurationSectionData(string name) : this(name, false, null, null)
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
        public ConfigurationSectionData(string name, bool encrypt) : this(name, encrypt, null, null)
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
        public ConfigurationSectionData(string name, bool encrypt, StorageProviderData storageProvider, TransformerData transformer)
        {
            this.name = name;
            this.encrypt = encrypt;
            this.storageProvider = storageProvider;
            this.transformer = transformer;
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="StorageProviderData"/> settings for the section.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="StorageProviderData"/> settings for the section. The default is <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>storageProvider</c> attribute in configuration for the configuration section.</para>
        /// </remarks>
        [XmlElement("storageProvider", IsNullable=false)]
        public StorageProviderData StorageProvider
        {
            get { return this.storageProvider; }
            set { this.storageProvider = value; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="TransformerData"/> settings for the section.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="TransformerData"/> settings for the section. The default is <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>dataTransformer</c> attribute in configuration for the configuration section.</para>
        /// </remarks>
        [XmlElement("dataTransformer", IsNullable=true)]
        public TransformerData Transformer
        {
            get { return this.transformer; }
            set { this.transformer = value; }
        }

        /// <summary>
        /// <para>Gets or sets the name of the configuration section.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the <see cref="ConfigurationSectionData"/>. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>name</c> attribute in configuration for the configuration section.</para>
        /// </remarks>
        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// <para>Gets or sets if the section will be encrypted.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the section will be encrypted; otherwise, <see langword="false"/>. The deafult is <see langword="false"/>.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>encrypt</c> attribute in configuration for the configuration section.</para>
        /// </remarks>
        [XmlAttribute("encrypt")]
        public bool Encrypt
        {
            get { return encrypt; }
            set { encrypt = value; }
        }
    }
}