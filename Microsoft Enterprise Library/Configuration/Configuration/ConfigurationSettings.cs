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

using System;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents the root of the configuration graph.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>enterpriselibrary.configurationSettings</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("enterpriselibrary.configurationSettings", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    [Serializable]
    public class ConfigurationSettings
    {
        private XmlIncludeTypeDataCollection xmlIncludeTypes;

        /// <summary>
        /// <para>Gets the section name for the library in configuration.</para>
        /// </summary>
        /// <value>
        /// <para>The section name for the library in configuration.</para>
        /// </value>
        /// <remarks>
        /// <para>The secion name is enterpriselibrary.configurationSettings.</para>
        /// </remarks>
        public const string SectionName = "enterpriselibrary.configurationSettings";

        private ConfigurationSectionDataCollection configurationSections;
        private KeyAlgorithmPairStorageProviderData keyAlgorithmPairStorageProviderData;
        private string applicationName;

        /// <summary>
        /// <para>Gets the Xml namespace for this root node.</para>
        /// </summary>
        /// <value>
        /// <para>The Xml namespace for this root node.</para>
        /// </value>
        public const string ConfigurationNamespace = "http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration";

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationSettings"/> class.</para>
        /// </summary>
        public ConfigurationSettings() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationSettings"/> class with a default section name.</para>
        /// </summary>
        /// <param name="applicationName">
        /// <para>The name of the application for these settings.</para>
        /// </param>
        public ConfigurationSettings(string applicationName)
        {
            this.applicationName = applicationName;
            configurationSections = new ConfigurationSectionDataCollection();
            xmlIncludeTypes = new XmlIncludeTypeDataCollection();
        }

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationSectionDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The configuration sections available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>configurationSections</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="configurationSections")]
        [XmlArrayItem(ElementName="configurationSection", Type=typeof(ConfigurationSectionData))]
        public ConfigurationSectionDataCollection ConfigurationSections
        {
            get { return configurationSections; }
        }

        /// <summary>
        /// <para>Gets or sets the name of the application for these settings.</para>
        /// </summary>
        /// <value>
        /// <para>The name of application for these settings. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>applicationName</c> attribute in configuration for the block configuration.</para>
        /// </remarks>
        [XmlAttribute("applicationName")]
        public string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="ConfigurationSectionData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="ConfigurationSectionData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <seealso cref="ConfigurationSectionData.Name"/>
        [XmlIgnore]
        public ConfigurationSectionData this[string name]
        {
            get { return configurationSections[name]; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="KeyAlgorithmPairStorageProviderData"/> for encrypting sections.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="KeyAlgorithmPairStorageProviderData"/> for encrypting sections.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>keyAlgorithmStorageProvider</c> attribute in configuration for the configuration.</para>
        /// </remarks>
        [XmlElement("keyAlgorithmStorageProvider", IsNullable=true)]
        public KeyAlgorithmPairStorageProviderData KeyAlgorithmPairStorageProviderData
        {
            get { return keyAlgorithmPairStorageProviderData; }
            set { keyAlgorithmPairStorageProviderData = value; }
        }

        /// <summary>
        /// <para>Gets the <see cref="XmlIncludeTypeDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The include types available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>includeTypes</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="includeTypes", Namespace=ConfigurationSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="includeType", Type=typeof(XmlIncludeTypeData), Namespace=ConfigurationSettings.ConfigurationNamespace)]
        public XmlIncludeTypeDataCollection XmlIncludeTypes
        {
            get { return xmlIncludeTypes; }
        }
    }
}