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
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// Represents the configuration data used to initialize
    /// a <see cref="Database"/> object.
    /// </summary>
    [XmlRoot("databaseProvider", Namespace=DatabaseSettings.ConfigurationNamespace)]
    public class DatabaseProviderData : ProviderData
    {
        private ConnectionStringData connectionStringData;
        private string typeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseProviderData"/> class.
        /// </summary>
        /// <param name="instance">An <see cref="InstanceData"/> object.</param>
        /// <param name="type">A <see cref="DatabaseTypeData"/> object.</param>
        /// <param name="connectionString">A <see cref="ConnectionStringData"/> object.</param>
        public DatabaseProviderData(
            InstanceData instance,
            DatabaseTypeData type,
            ConnectionStringData connectionString) : this(instance.Name, type.TypeName, connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseProviderData"/> class.
        /// </summary>
        /// <param name="name">A name.</param>
        public DatabaseProviderData(string name) : this(name, string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseProviderData"/> class.
        /// </summary>
        /// <param name="name">A name.</param>
        /// <param name="typeName">A type name of a class that implements the <see cref="Database"/> class.</param>
        /// <param name="connectionStringData">A <see cref="ConnectionStringData"/> object.</param>
        public DatabaseProviderData(string name, string typeName, ConnectionStringData connectionStringData) : base(name)
        {
            this.typeName = typeName;
            this.connectionStringData = connectionStringData;
        }

        /// <summary>
        /// Gets the <see cref="ConnectionStringData"/> object.
        /// </summary>
        public ConnectionStringData ConnectionStringData
        {
            get { return this.connectionStringData; }
        }

        /// <summary>
        /// <para>When implemented by a class, Gets or sets the <see cref="System.Type"/> name of the provider.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the provider. The default is an empty string.</para>
        /// </value>
        /// <remarks>
        /// <para><b>Not to implementers:</b> You do not have to implement the set operation.  If you have only one type for your data object you can ignore the set.</para>
        /// </remarks>
        public override string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

    }
}