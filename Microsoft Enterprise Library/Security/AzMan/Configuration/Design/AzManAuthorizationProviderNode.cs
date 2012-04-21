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

using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design
{
    /// <summary>
    /// Node representing configuration from <see cref="AzManAuthorizationProviderData"/>.
    /// </summary>
    public class AzManAuthorizationProviderNode : AuthorizationProviderNode
    {
        private const string storeLocation = "msxml://c:/myAuthStore.xml";
        private const string applicationName = "My Application";
        private static readonly string scopeName = string.Empty;
        private const string auditIdentifierPrefix = "AzMan Authorization Provider";

        private AzManAuthorizationProviderData azManAuthorizationProviderData;

		/// <summary>
		/// Default constructor
		/// </summary>
        public AzManAuthorizationProviderNode() : this(new AzManAuthorizationProviderData(SR.AzManProvider, storeLocation, applicationName, auditIdentifierPrefix, scopeName))
        {
        }

		/// <summary>
		/// Constructor for AzManAuthorizationProviderDataNode.
		/// </summary>
		/// <param name="azManAuthorizationProviderData">Configuration data for <see cref="AzManAuthorizationProvider"></see></param>
        public AzManAuthorizationProviderNode(AzManAuthorizationProviderData azManAuthorizationProviderData) : base(azManAuthorizationProviderData)
        {
            this.azManAuthorizationProviderData = azManAuthorizationProviderData;
        }

        /// <summary>
        /// See <see cref="AzManAuthorizationProviderData.Application"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.ApplicationDescription)]
        public string Application
        {
            get { return azManAuthorizationProviderData.Application; }
            set { azManAuthorizationProviderData.Application = value; }
        }

        /// <summary>
        /// See <see cref="AzManAuthorizationProviderData.Scope"/>.
        /// </summary>
        [SRDescription(SR.Keys.ScopeDescription)]
        public string Scope
        {
            get { return azManAuthorizationProviderData.Scope; }
            set { azManAuthorizationProviderData.Scope = value; }
        }

        /// <summary>
        /// See <see cref="AzManAuthorizationProviderData.StoreLocation"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.StoreLocationDescription)]
        public string StoreLocation
        {
            get { return azManAuthorizationProviderData.StoreLocation; }
            set { azManAuthorizationProviderData.StoreLocation = value; }
        }

        /// <summary>
        /// See <see cref="AzManAuthorizationProviderData.AuditIdentifierPrefix"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.AuditIdentifierPrefixDescription)]
        public string AuditIdentifierPrefix
        {
            get { return azManAuthorizationProviderData.AuditIdentifierPrefix; }
            set { azManAuthorizationProviderData.AuditIdentifierPrefix = value; }
        }

        /// <summary>
        /// The fully qualified type name for the AzMan provider 
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return typeof(AzManAuthorizationProvider).AssemblyQualifiedName; }
        }

        /// <summary>
        /// See <see cref="AuthorizationProviderNode.AuthorizationProviderData"/>.
        /// </summary>
        [Browsable(false)]
        public override AuthorizationProviderData AuthorizationProviderData
        {
            get { return base.AuthorizationProviderData; }
        }
    }
}