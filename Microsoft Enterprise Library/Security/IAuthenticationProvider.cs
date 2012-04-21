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
    /// Defines the basic functionality of an authentication provider.
    /// </summary>
    public interface IAuthenticationProvider : IConfigurationProvider
    {
        /// <summary>
        /// Verifies the specified credentials and constructs an
        /// <see cref="IIdentity"/> object.
        /// </summary>
        /// <param name="credentials">A credential object such as a <see cref="NamePasswordCredential"/>.</param>
        /// <param name="identity">An <see cref="IIdentity"/> object.</param>
        /// <returns><strong>True</strong> if authentication was
        /// successful, otherwise false.</returns>
        bool Authenticate(object credentials, out IIdentity identity);
    }
}