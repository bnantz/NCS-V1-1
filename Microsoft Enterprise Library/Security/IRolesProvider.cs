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
    /// Defines the basic functionality of a role provider.
    /// </summary>
    public interface IRolesProvider : IConfigurationProvider
    {
        /// <summary>
        /// Creates a principal with the role information
        /// for the specified identity.
        /// </summary>
        /// <param name="identity">An <see cref="IIdentity"/></param>
        /// <returns>An <see cref="IPrincipal"/>.</returns>
        IPrincipal GetRoles(IIdentity identity);
    }
}