//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// <para>Represents the root configuration for data.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>databaseSettings</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("enterpriseLibrary.databaseSettings", Namespace=DatabaseSettings.ConfigurationNamespace)]
    public class DatabaseSettings
    {
        /// <summary>
        /// The name of the data configuration section.
        /// </summary>
        public const string SectionName = "dataConfiguration";
        private DatabaseTypeDataCollection databaseTypes;
        private InstanceDataCollection instances;
        private ConnectionStringDataCollection connectionStrings;
        private string defaultInstance;

        /// <summary>
        /// <para>Gets the Xml namespace for this root node.</para>
        /// </summary>
        /// <value>
        /// <para>The Xml namespace for this root node.</para>
        /// </value>
        public const string ConfigurationNamespace = "http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/data";

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DatabaseSettings"/> class.</para>
        /// </summary>
        public DatabaseSettings()
        {
            this.databaseTypes = new DatabaseTypeDataCollection();
            this.instances = new InstanceDataCollection();
            this.connectionStrings = new ConnectionStringDataCollection();
        }

        /// <summary>
        /// <para>Gets the <see cref="DatabaseTypeDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The database types available in configuration.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>databaseTypes</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="databaseTypes", Namespace=DatabaseSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="databaseType", Type=typeof(DatabaseTypeData), Namespace=DatabaseSettings.ConfigurationNamespace)]
        public DatabaseTypeDataCollection DatabaseTypes
        {
            get { return this.databaseTypes; }
        }

        /// <summary>
        /// <para>Gets the <see cref="InstanceDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The database instances available in configuration.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>instances</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="instances", Namespace=DatabaseSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="instance", Type=typeof(InstanceData), Namespace=DatabaseSettings.ConfigurationNamespace)]
        public InstanceDataCollection Instances
        {
            get { return this.instances; }
        }

        /// <summary>
        /// <para>Gets the <see cref="ConnectionStringDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The connection strings available in configuration.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>connectionString</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="connectionStrings", Namespace=DatabaseSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="connectionString", Type=typeof(ConnectionStringData), Namespace=DatabaseSettings.ConfigurationNamespace)]
        public ConnectionStringDataCollection ConnectionStrings
        {
            get { return this.connectionStrings; }
        }

        /// <summary>
        /// <para>Gets or sets the default database instance.</para>
        /// </summary>
        /// <value>
        /// <para>The default database instance.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>defaultInstance</c> element in configuration.</para>
        /// </remarks>
        [XmlAttribute("defaultInstance")]
        public string DefaultInstance
        {
            get { return this.defaultInstance; }
            set { this.defaultInstance = value; }
        }
    }
}