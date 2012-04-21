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

using System;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Base class for RolesProviders in the security infrastructure. This class encapsulates the
    /// basic policy of how roles are retrieved from the underlying store, construction of a principal
    /// based on the roles and identity of the user, and calls instrumentation. Details of how this all
    /// happens are left to subclasses.
    /// </summary>
    public abstract class SecurityRolesProvider : ConfigurationProvider, IRolesProvider
    {
        /// <summary>
        /// Verifies the <see cref="IIdentity"/> object and wraps it inside 
        /// a new <see cref="GenericPrincipal"/> object along with any security
        /// roles the user is a member of.
        /// </summary>
        /// <param name="userIdentity">An <see cref="IIdentity"/> object 
        /// representing an authenticated user.
        /// </param>
        /// <returns>
        /// If successfull, returns the populated <see cref="IPrincipal"/> object.
        /// </returns>
        public IPrincipal GetRoles(IIdentity userIdentity)
        {
            ValidateParameters(userIdentity);

            string[] userRoles = CollectAllUserRoles(userIdentity);

            GenericPrincipal userPrincipal = new GenericPrincipal(userIdentity, userRoles);

            SecurityRoleLoadEvent.Fire(userIdentity.Name);

            return userPrincipal;
        }

        /// <summary>
        /// Subclasses must override this method to retrieve the associated list of
        /// user roes for the given identity. 
        /// </summary>
        /// <param name="userIdentity">Identity of user used for retrieval</param>
        /// <returns>String array of roles for the given user identity</returns>
        protected abstract string[] CollectAllUserRoles(IIdentity userIdentity);

        /// <summary>
        /// Validates the identity.
        /// </summary>
        /// <param name="userIdentity">The identity to validate.</param>
        protected void ValidateParameters(IIdentity userIdentity)
        {
            if (userIdentity == null)
            {
                throw new ArgumentNullException(SR.IdentityIsNull);
            }
            if (userIdentity.Name.Length == 0)
            {
                throw new ArgumentException(SR.IdentityInvalid);
            }
        }
    }
}