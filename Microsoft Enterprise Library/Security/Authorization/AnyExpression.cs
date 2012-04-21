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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization
{
    /// <summary>
    /// Represents an expression that evaluates to true
    /// for any specified principal.
    /// </summary>
    public class AnyExpression : WordExpression
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AnyExpression"/> class.
        /// </summary>
        public AnyExpression() : base(SR.AnonymousIdentityExpressionString)
        {
        }

        /// <summary>
        /// Evaluates the specified principal and returns
        /// true if the principal is not null.
        /// </summary>
        /// <param name="principal">The <see cref="System.Security.Principal.IPrincipal"/>
        /// that the current expression will be evaluated against.</param>
        /// <returns><strong>True</strong> if the principal
        /// is not null, otherwise <strong>false</strong>.</returns>
        public override bool Evaluate(IPrincipal principal)
        {
            if (principal != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Evaluates the specified 
        /// <see cref="System.Security.Principal.IIdentity"/>.
        /// </summary>
        /// <param name="identity">The <see cref="System.Security.Principal.IIdentity"/>
        /// that the current expression will be evaluated against.</param>
        /// <returns><strong>True</strong> if the identity not null, 
        /// otherwise <strong>false</strong>.</returns>
        public override bool Evaluate(IIdentity identity)
        {
            if (identity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a textual representation of the current expression
        /// </summary>
        /// <returns>An expression string.</returns>
        public override string ToString()
        {
            return SR.AnonymousIdentityExpressionString;
        }

    }
}