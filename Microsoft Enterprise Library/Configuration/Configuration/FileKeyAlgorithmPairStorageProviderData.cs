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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a storage provider to read and write data for a <see cref="KeyAlgorithmPair"/> for a file.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>keyAlgorithmStorageProvider</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("fileKeyAlgorithmStorageProvider", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class FileKeyAlgorithmPairStorageProviderData : KeyAlgorithmPairStorageProviderData
    {
        private string path;
        private DpapiSettingsData dpapiSettings;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileKeyAlgorithmPairStorageProviderData"/> class.</para>
        /// </summary>
        public FileKeyAlgorithmPairStorageProviderData() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileKeyAlgorithmPairStorageProviderData"/> class with a name, fully qualified type name and file path.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the storage provider.</para>
        /// </param>
        /// <param name="path">
        /// <para>The path to the file to store the pair.</para>
        /// </param>
        public FileKeyAlgorithmPairStorageProviderData(string name, string path) : this(name, path, null)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileKeyAlgorithmPairStorageProviderData"/> class with a name, fully qualified type name, file path and the DPAPI settings.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the storage provider.</para>
        /// </param>
        /// <param name="path">
        /// <para>The path to the file to store the pair.</para>
        /// </param>
        /// <param name="dpapiSettings">
        /// <para>A <see cref="DpapiSettingsData"/> object.</para>
        /// </param>
        public FileKeyAlgorithmPairStorageProviderData(string name, string path, DpapiSettingsData dpapiSettings) : base(name)
        {
            this.path = path;
            this.dpapiSettings = dpapiSettings;
        }

        /// <summary>
        /// <para>Gets or sets the path of the file for storage.</para>
        /// </summary>
        /// <value>
        /// <para>The path of the file for storage. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>path</c> attribute in configuration for the provider.</para>
        /// </remarks>        
        [XmlAttribute("path")]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// <para>Gets the settings for DPAPI protection (optional).</para>
        /// </summary>
        /// <value>
        /// <para>The settings for DPAPI protection (optional).</para>
        /// </value>
        [XmlElement("dpapiSettings", IsNullable=true, Namespace=ConfigurationSettings.ConfigurationNamespace)]
        public DpapiSettingsData DpapiSettings
        {
            get { return dpapiSettings; }
            set { dpapiSettings = value; }
        }

        /// <summary>
        /// <para>Gets the fully qualified assembly name for a <see cref="FileKeyAlgorithmPairStorageProvider"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The fully qualified assembly name for a <see cref="FileKeyAlgorithmPairStorageProvider"/>.</para>
        /// </value>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(FileKeyAlgorithmPairStorageProvider).AssemblyQualifiedName; }
            set
            {
            }
        }

        /// <summary>
        /// <para>Creates a new object that is a copy of the current instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new object that is a copy of this instance.</para>
        /// </returns>
        /// <remarks>
        /// <para>This clone does a deep copy.</para>
        /// </remarks>
        public override object Clone()
        {
            return new FileKeyAlgorithmPairStorageProviderData(Name, path,
                                                               (null != dpapiSettings) ? new DpapiSettingsData(dpapiSettings.Entropy, dpapiSettings.Mode) : null);
        }

    }
}