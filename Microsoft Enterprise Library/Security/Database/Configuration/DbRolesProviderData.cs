//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration
{
    /// <summary>
    /// Configuration data for the Database Roles Provider.
    /// </summary>
    [XmlRoot("rolesProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    public class DbRolesProviderData : RolesProviderData
    {
        private string database;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DbRolesProviderData"/> class.</para>
        /// </summary>
        public DbRolesProviderData() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DbRolesProviderData"/> class with a name and a database instance name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the provider.</para>
        /// </param>
        /// <param name="database">
        /// <para>The name of the database instance where the roles are persisted.</para>
        /// </param>
        public DbRolesProviderData(string name, string database) : base(name)
        {
            this.database = database;
        }

        /// <summary>
        /// Gets or sets the configured Database instance name.
        /// </summary>
        /// <value>The configured database instance.</value>
        [XmlAttribute(AttributeName = "database")]
        public string Database
        {
            get { return this.database; }
            set { this.database = value; }
        }

        /// <summary>
        /// Gets the assembly qualified name of this provider.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(DbRolesProvider).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}