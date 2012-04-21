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

namespace Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Configuration.Design
{
    /// <summary>
    /// The design time representation of the runtime 
    /// <see cref="AdRolesProvider"/> class.
    /// </summary>
    public class ActiveDirectoryRolesProviderNode : RolesProviderNode
    {
        private AdRolesProviderData adRolesProviderData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectoryRolesProviderNode"/> class.
        /// </summary>
        public ActiveDirectoryRolesProviderNode() : this(new AdRolesProviderData(SR.ActiveDirectoryRolesProviderDisplayName, string.Empty, string.Empty, string.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDirectoryRolesProviderNode"/>
        /// class with the specified <see cref="AdRolesProviderData"/>.
        /// </summary>
        /// <param name="adRolesProviderData">A <see cref="AdRolesProviderData"/> object.</param>
        public ActiveDirectoryRolesProviderNode(AdRolesProviderData adRolesProviderData) : base(adRolesProviderData)
        {
            this.adRolesProviderData = adRolesProviderData;
			this.adRolesProviderData.AccountName = SR.AccountNameFieldName;
        }

        /// <summary>
        /// See <see cref="AdRolesProviderData.ProviderType"/>.
        /// </summary>
        /// <value>See <see cref="AdRolesProviderData.ProviderType"/>.</value>
        [SRDescription(SR.Keys.ProviderTypePropertyDescription)]
        [Required]
        public string ProviderType
        {
            get { return this.adRolesProviderData.ProviderType; }
            set { this.adRolesProviderData.ProviderType = value; }
        }

        /// <summary>
        /// See <see cref="AdRolesProviderData.Server"/>.
        /// </summary>
        /// <value>See <see cref="AdRolesProviderData.Server"/>.</value>
        [SRDescription(SR.Keys.ServerPropertyDescription)]
        [Required]
        public string Server
        {
            get { return this.adRolesProviderData.Server; }
            set { this.adRolesProviderData.Server = value; }
        }

        /// <summary>
        /// See <see cref="AdRolesProviderData.UserPartition"/>.
        /// </summary>
        /// <value>See <see cref="AdRolesProviderData.UserPartition"/>.</value>
        [Required]
        [SRDescription(SR.Keys.UserPartitionPropertyDescription)]
        public string UserPartition
        {
            get { return this.adRolesProviderData.UserPartition; }
            set { this.adRolesProviderData.UserPartition = value; }
        }

		[SRDescription(SR.Keys.AccountNamePropertyDescription)]
		public string AccountName
		{
			get { return this.adRolesProviderData.AccountName; }
			set { this.adRolesProviderData.AccountName = value; }
		}

        /// <summary>
        /// See <see cref="RolesProviderNode.TypeName"/>.
        /// </summary>
        /// <value>A type name.</value>
        [Browsable(false)]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }
    }
}