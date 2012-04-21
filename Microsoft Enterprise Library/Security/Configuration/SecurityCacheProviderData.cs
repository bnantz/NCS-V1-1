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
    /// Configuration data for the Security Cache.
    /// </summary>
    [XmlRoot("securityCacheProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomSecurityCacheProviderData))]
    public abstract class SecurityCacheProviderData : ProviderData
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SecurityCacheProviderData"/> class.
        /// </summary>
        protected SecurityCacheProviderData() : base()
        {
        }

        /// <summary>
        /// Intializes a new instance of the 
        /// <see cref="SecurityCacheProviderData"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        protected SecurityCacheProviderData(string name) : base(name)
        {
        }
    }
}