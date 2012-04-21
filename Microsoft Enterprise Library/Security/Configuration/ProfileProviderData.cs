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
    /// all profile providers.
    /// </summary>
    [XmlRoot("profileProvider", Namespace=SecuritySettings.ConfigurationNamespace)]
    [XmlInclude(typeof(CustomProfileProviderData))]
    public abstract class ProfileProviderData : ProviderData
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ProfileProviderData"/> class.
        /// </summary>
        protected ProfileProviderData() : base()
        {
        }

        /// <summary>
        /// Intializes a new instance of the 
        /// <see cref="ProfileProviderData"/> class.
        /// </summary>
        /// <param name="name">The name of the provider.</param>
        protected ProfileProviderData(string name) : base(name)
        {
        }
    }
}