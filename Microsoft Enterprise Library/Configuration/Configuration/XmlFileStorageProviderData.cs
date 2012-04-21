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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a storage provider to read and write data in configuration from an Xml file.</para>
    /// </summary>    	
    [XmlRoot("storageProvider", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class XmlFileStorageProviderData : StorageProviderData
    {
        private string path;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlFileStorageProviderData"/> class.</para>
        /// </summary>
        public XmlFileStorageProviderData() : this(string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlFileStorageProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the provider.</para>
        /// </param>
        public XmlFileStorageProviderData(string name) : this(name, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlFileStorageProviderData"/> class with a name and path to the file.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the provider.</para>
        /// </param>
        /// <param name="path">
        /// <para>The path to the file with the data.</para>
        /// </param>
        public XmlFileStorageProviderData(string name, string path) : base(name, typeof(XmlFileStorageProvider).AssemblyQualifiedName)
        {
            this.path = path;
        }

        /// <summary>
        /// <para>Gets or sets the path of the Xml file to store configuration.</para>
        /// </summary>
        /// <value>
        /// <para>The path of the Xml file to store configuration. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>path</c> attribute in configuration for the provider.</para>
        /// </remarks>        
        [XmlAttribute("path")]
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        /// <summary>
        /// <para>Gets the fully qualified assembly name for a <see cref="XmlFileStorageProvider"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The fully qualified assembly name for a <see cref="XmlFileStorageProvider"/>.</para>
        /// </value>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(XmlFileStorageProvider).AssemblyQualifiedName; }
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
            return new XmlFileStorageProviderData(Name, path);
        }
    }
}