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
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the common configuration data for
    /// all authentication providers.
    /// </summary>
    [XmlRoot("authenticationProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomAuthenticationProviderData))]
    public abstract class AuthenticationProviderData : ProviderData
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AuthenticationProviderData"/> class.
        /// </summary>
        protected AuthenticationProviderData()
        {
        }

        /// <summary>
        /// Intializes a new instance of the 
        /// <see cref="AuthenticationProviderData"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        protected AuthenticationProviderData(string name) : base(name)
        {
        }
    }
}