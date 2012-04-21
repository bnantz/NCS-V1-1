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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization
{
    /// <summary>
    /// Represents an authorization provider that evaluates
    /// boolean expressions to determine whether 
    /// <see cref="System.Security.Principal.IPrincipal"/> objects
    /// are authorized.
    /// </summary>
    public class AuthorizationRuleProvider : ConfigurationProvider, IAuthorizationProvider
    {
        private SecurityConfigurationView securityConfigurationView;

        /// <summary>
        /// Initializes the state of the current object from
        /// the specified configuration data.
        /// </summary>
        /// <param name="configurationView">A <see cref="SecurityConfigurationView"></see> object</param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(SecurityConfigurationView));
            
            securityConfigurationView = (SecurityConfigurationView)configurationView;
        }

        /// <summary>
        /// Evaluates the specified authority against the specified context.
        /// </summary>
        /// <param name="principal">Must be an <see cref="IPrincipal"/> object.</param>
        /// <param name="ruleName">Must be a string that is the name of the rule to evaluate.</param>
        /// <returns><c>true</c> if the expression evaluates to true,
        /// otherwise <c>false</c>.</returns>
        public bool Authorize(IPrincipal principal, string ruleName)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            if (ruleName == null || ruleName.Length == 0)
            {
                throw new ArgumentNullException("ruleName");
            }

            SecurityAuthorizationCheckEvent.Fire(principal.Identity.Name, ruleName);

            BooleanExpression booleanExpression = GetParsedExpression(ruleName);
            if (booleanExpression == null)
            {
                throw new AuthorizationRuleNotFoundException(SR.AuthorizationRuleNotFoundMsg(ruleName));
            }

            bool result = booleanExpression.Evaluate(principal);

            if (result == false)
            {
                SecurityAuthorizationFailedEvent.Fire(principal.Identity.Name, ruleName);
            }
            return result;
        }

        private BooleanExpression GetParsedExpression(string ruleName)
        {
            AuthorizationRuleDataCollection authorizationRuleDataCollection = GetAuthorizationRules();
            AuthorizationRuleData authorizationRuleData = authorizationRuleDataCollection[ruleName];
            if (authorizationRuleData == null) return null;
            string expression = authorizationRuleData.Expression;
            Parser parser = new Parser();
            return parser.Parse(expression);
        }

        private AuthorizationRuleDataCollection GetAuthorizationRules()
        {
            AuthorizationProviderData authorizationProviderData = securityConfigurationView.GetAuthorizationProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(authorizationProviderData, typeof(AuthorizationRuleProviderData));

            AuthorizationRuleProviderData authorizationRuleProviderData = (AuthorizationRuleProviderData)authorizationProviderData;

            return authorizationRuleProviderData.Rules;
        }
    }
}