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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Configuration
{
    /// <summary>
    /// Configuration data for the Database Authentication Provider.
    /// </summary>
    [XmlRoot("authenticationProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    public class DbAuthenticationProviderData : AuthenticationProviderData
    {
        private string database;
        private string hashProvider;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DbAuthenticationProviderData"/> class.</para>
        /// </summary>
        public DbAuthenticationProviderData() : this(string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DbAuthenticationProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the provider.</para>
        /// </param>
        public DbAuthenticationProviderData(string name) : this(name, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DbAuthenticationProviderData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the provider.</para>
        /// </param>
        /// <param name="database">
        /// <para>The named database to use for the provider.</para>
        /// </param>
        /// <param name="hashProvider">
        /// <para>The named hash provider to use for this provider.</para>
        /// </param>
        public DbAuthenticationProviderData(string name, string database, string hashProvider) : base(name)
        {
            this.database = database;
            this.hashProvider = hashProvider;
        }

        /// <summary>
        /// Gets or sets the configured Database instance name.
        /// </summary>
        /// <value>The configured database instance.</value>
        [XmlAttribute("database")]
        public string Database
        {
            get { return this.database; }
            set { this.database = value; }
        }

        /// <summary>
        /// Gets or sets the configured cryptography hash instance name.
        /// </summary>
        /// <value>The configured cryptography hash instance.</value>
        [XmlAttribute("hashProvider")]
        public string HashProvider
        {
            get { return this.hashProvider; }
            set { this.hashProvider = value; }
        }

        /// <summary>
        /// Gets the assembly qualified name of this provider.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(DbAuthenticationProvider).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}