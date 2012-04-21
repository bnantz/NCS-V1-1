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
using Microsoft.Practices.EnterpriseLibrary.Security.Authorization;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the configuration data for an
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Authorization.AuthorizationRuleProvider"/>.
    /// </summary>
    [XmlRoot("authorizationProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    public class AuthorizationRuleProviderData : AuthorizationProviderData
    {
        private AuthorizationRuleDataCollection rules;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthorizationRuleProviderData"/> class.
        /// </summary>
        public AuthorizationRuleProviderData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthorizationRuleProviderData"/> class.
        /// </summary>
        /// <param name="name">The name of the rule provider.</param>
        public AuthorizationRuleProviderData(string name) : base(name)
        {
            this.rules = new AuthorizationRuleDataCollection();
        }

        /// <summary>
        /// Gets or sets the list of rules associated with
        /// the provider.
        /// </summary>
        /// <value>An <see cref="AuthorizationRuleDataCollection"/>.</value>
        [XmlArray("rules")]
        [XmlArrayItem(typeof(AuthorizationRuleData))]
        public AuthorizationRuleDataCollection Rules
        {
            get { return this.rules; }
        }

        /// <summary>
        /// Overriden so that the XmlSerializer will ignore this property. Gets a fixed value which is the type name of the <see cref="AuthorizationRuleProvider"/>.
        /// </summary>
        [XmlIgnore()]
        public override string TypeName
        {
            get { return typeof(AuthorizationRuleProvider).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}