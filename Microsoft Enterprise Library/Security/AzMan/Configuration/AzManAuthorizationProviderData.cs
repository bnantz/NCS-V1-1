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

using System.IO;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration
{
    /// <summary>
    /// Represents the configuration settings for the <see cref="AzManAuthorizationProvider"/>.
    /// </summary>
    [XmlRoot("authorizationProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    public class AzManAuthorizationProviderData : AuthorizationProviderData
    {
        private const string StoreLocationTargetToken = "{currentPath}";
        private string storeLocation;
        private string applicationName;
        private string auditIdentifierPrefix;
        private string scopeName;

		/// <summary>
		/// Default constructor
		/// </summary>
        public AzManAuthorizationProviderData()
        {
        }

		/// <summary>
		/// Constructor for AzManAuthorizationProviderData. Use this constructor when you know only
		/// the name of the AzManAuthorizationProvider you are creating
		/// </summary>
		/// <param name="name">Name of <see cref="AzManAuthorizationProvider"></see> found in configuration</param>
        public AzManAuthorizationProviderData(string name) : this(name, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

		/// <summary>
		/// Full constructor for AzManAuthorizationProviderData.
		/// </summary>
		/// <param name="name">Name of <see cref="AzManAuthorizationProvider"></see> found in configuration</param>
		/// <param name="storeLocation">Location of the authorization store, Active Directory or xml file</param>
		/// <param name="applicationName">Name of the AzMan application.</param>
		/// <param name="auditIdentifierPrefix">Audit identifier prefix to prepend to the generated audit identifer</param>
		/// <param name="scopeName">Optional name of the application scope</param>
        public AzManAuthorizationProviderData(string name, string storeLocation, string applicationName, string auditIdentifierPrefix, string scopeName) : base(name)
        {
            this.storeLocation = storeLocation;
            this.applicationName = applicationName;
            this.auditIdentifierPrefix = auditIdentifierPrefix;
            this.scopeName = scopeName;
        }

        /// <summary>
        /// Location of the authorization store, Active Directory or xml file.
        /// </summary>
        /// <remarks>Absolute file paths are required for xml storage.  
        /// View this link for more information about the expected format http://msdn.microsoft.com/library/default.asp?url=/library/en-us/security/security/azauthorizationstore_initialize.asp.</remarks>
        [XmlAttribute("storeLocation")]
        public string StoreLocation
        {
            get { return this.storeLocation; }
            set
            {
                string store = value;
                if (store.IndexOf(StoreLocationTargetToken) > -1)
                {
                    string dir = Directory.GetCurrentDirectory().Replace(@"\", "/");
                    store = store.Replace(StoreLocationTargetToken, dir);
                }
                this.storeLocation = store;
            }
        }

        /// <summary>
        /// Name of the AzMan application.
        /// </summary>
        [XmlAttribute("application")]
        public string Application
        {
            get { return this.applicationName; }
            set { this.applicationName = value; }
        }

        /// <summary>
        /// Optional name of the application scope.
        /// </summary>
        [XmlAttribute("scope")]
        public string Scope
        {
            get { return this.scopeName; }
            set { this.scopeName = value; }
        }

        /// <summary>
        /// Audit identifier prefix to append to the generated audit identifer.
        /// </summary>
        /// <remarks>
        /// The audit identifier is generated to be "prefix username:operation"
        /// </remarks>
        [XmlAttribute("auditIdentifierPrefix")]
        public string AuditIdentifierPrefix
        {
            get { return this.auditIdentifierPrefix; }
            set { this.auditIdentifierPrefix = value; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="System.Type"/> name of the provider.</para>
        /// </summary>
        /// <value>
        /// <para>The type name of the provider. The default is an empty string.</para>
        /// </value>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(AzManAuthorizationProvider).AssemblyQualifiedName; }
            set
            {
        }
        }

    }
}
