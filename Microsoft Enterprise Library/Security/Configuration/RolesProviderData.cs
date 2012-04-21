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
    /// all roles providers.
    /// </summary>
    [XmlRoot("rolesProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomRolesProviderData))]
    public abstract class RolesProviderData : ProviderData
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="RolesProviderData"/> class.
        /// </summary>
        protected RolesProviderData() : base()
        {
        }

        /// <summary>
        /// Intializes a new instance of the 
        /// <see cref="RolesProviderData"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        protected RolesProviderData(string name) : base(name)
        {
        }
    }
}