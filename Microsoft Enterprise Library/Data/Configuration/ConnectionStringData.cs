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
    /// <para>Represents a connection string for an <see cref="InstanceData"/> database.</para>
    /// <seealso cref="System.Data.IDbConnection.ConnectionString"/>
    /// </summary>
    [XmlRoot("connectionString", Namespace=DatabaseSettings.ConfigurationNamespace)]
    [XmlInclude(typeof(OracleConnectionStringData))]
    public class ConnectionStringData
    {
        private string name;
        private ParameterDataCollection parameters;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConnectionStringData"/> class.</para>
        /// </summary>
        public ConnectionStringData()
        {
            parameters = new ParameterDataCollection();
            this.name = string.Empty;
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConnectionStringData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the connection string.</para>
        /// </param>
        public ConnectionStringData(string name) : this()
        {
            this.name = name;
        }

        /// <summary>
        /// <para>Gets or sets the name of the <see cref="ConnectionStringData"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the ConnectionString. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>name</c> attribute in configuration for the connection string.</para>
        /// </remarks>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// <para>Gets the <see cref="ParameterDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The parameters of the connection string. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>parameters</c> element in configuration for the connection string.</para>
        /// </remarks>
        [XmlArray(ElementName="parameters", Namespace=DatabaseSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="parameter", Type=typeof(ParameterData), Namespace=DatabaseSettings.ConfigurationNamespace)]
        public ParameterDataCollection Parameters
        {
            get { return this.parameters; }
        }
    }
}