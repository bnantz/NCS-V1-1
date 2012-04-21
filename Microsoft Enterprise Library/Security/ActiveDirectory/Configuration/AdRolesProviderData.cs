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

namespace Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration
{
    /// <summary>
    /// Configuration data for the Active Directory Roles Provider.
    /// </summary>
    [XmlRoot("rolesProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    public class AdRolesProviderData : RolesProviderData
    {
        private string providerType;
        private string server;
        private string userPartition;
    	private string accountName;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public AdRolesProviderData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the  <see cref="AdRolesProviderData"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the provider</param>
        public AdRolesProviderData(string name) : this(name, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the  <see cref="AdRolesProviderData"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the provider</param>
        /// <param name="providerType">
        /// The Active Directory provider type name.
        /// </param>
        /// <param name="server">
        /// The name of the server hosting the Active Directory Service.
        /// </param>
        /// <param name="userPartition">
        /// The partition within the Active Directory that contains the user objects/data.
        /// </param>
        public AdRolesProviderData(string name, string providerType, string server, string userPartition) : base(name)
        {
            this.providerType = providerType;
            this.server = server;
            this.userPartition = userPartition;
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="System.Type"/> name of the provider.</para>
        /// </summary>
        /// <value>
        /// <para>The type name of the provider. The default is an empty string.</para>
        /// </value>
        public override string TypeName
        {
            get { return typeof(AdRolesProvider).AssemblyQualifiedName; }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the Active Directory provider type name.
        /// </summary>
        /// <value>Provider Type name.</value>
        [XmlAttribute("providerType")]
        public string ProviderType
        {
            get { return this.providerType; }
            set { this.providerType = value; }
        }

        /// <summary>
        /// Gets or sets the name of the server hosting the Active Directory Service.
        /// </summary>
        /// <value>Service Host Name.</value>
        [XmlAttribute("server")]
        public string Server
        {
            get { return this.server; }
            set { this.server = value; }
        }

        /// <summary>
        /// Gets or sets the partition within the Active Directory that contains the user objects/data.
        /// </summary>
        /// <value>Active Directory Partition.</value>
        [XmlAttribute("userPartition")]
        public string UserPartition
        {
            get { return this.userPartition; }
            set { this.userPartition = value; }
        }

		/// <summary>
		/// Name of field in schema to use to find the current account name
		/// </summary>
		/// <value>Active Directory Account Name</value>
		[XmlAttribute("accountName")]
		public string AccountName
		{
			get { return this.accountName; }
			set { this.accountName = value; }
    }
}
}
