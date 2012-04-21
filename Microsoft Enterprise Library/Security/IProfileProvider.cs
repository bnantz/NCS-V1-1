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

using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Defines the basic functionality of a profile provider.
    /// </summary>
    public interface IProfileProvider : IConfigurationProvider
    {
        /// <summary>
        /// Gets the profile for the specified identity.
        /// </summary>
        /// <param name="identity">An identity.</param>
        /// <returns>A profile object.</returns>
        object GetProfile(IIdentity identity);

        /// <summary>
        /// Sets the profile for the specified identity
        /// </summary>
        /// <param name="identity">An identity object.</param>
        /// <param name="profile">A profile object.</param>
        void SetProfile(IIdentity identity, object profile);
    }
}