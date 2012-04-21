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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Provides methods for the creation of
    /// <see cref="IAuthorizationProvider"/> objects.
    /// </summary>
    public class AuthorizationProviderFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="AuthorizationFactory"/> class.</para>
        /// </summary>
        public AuthorizationProviderFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationProviderFactory"/>
        /// with the specified <see cref="ConfigurationContext"/>.
        /// </summary>
        /// <param name="context">A <see cref="ConfigurationContext"/>.</param>
        public AuthorizationProviderFactory(ConfigurationContext context) : base(SR.Authorization, context, typeof(IAuthorizationProvider))
        {
        }

        /// <summary>
        /// Gets the default authorization provider as specified via configuration.
        /// </summary>
        /// <returns>An <see cref="IAuthorizationProvider"/> object.</returns>
        public IAuthorizationProvider GetAuthorizationProvider()
        {
            return (IAuthorizationProvider)base.CreateDefaultInstance();
        }

        /// <summary>
        /// Gets the authorization provider with the specified name.
        /// </summary>
        /// <param name="authorizationProviderName">The configured name of an authorization provider</param>
        /// <returns>An <see cref="IAuthorizationProvider"/> object.</returns>
        public IAuthorizationProvider GetAuthorizationProvider(string authorizationProviderName)
        {
            return (IAuthorizationProvider)base.CreateInstance(authorizationProviderName);
        }

        /// <summary>
        /// <para>Creates the <see cref="SecurityConfigurationView"/> for the factory.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="SecurityConfigurationView"/> for the factory.</para>
        /// </returns>
        protected override ConfigurationView CreateConfigurationView()
        {
            return new SecurityConfigurationView(ConfigurationContext);
        }

        /// <summary>
        /// <para>Gets the configuration object <see cref="Type"/> for the factory to create given the <paramref name="authorizationProviderName"/>.</para>
        /// </summary>
        /// <param name="authorizationProviderName">
        /// <para>The name of the <see cref="IAuthorizationProvider"/> to create.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="IAuthorizationProvider"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string authorizationProviderName)
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            AuthorizationProviderData authorizationProviderData = securityConfigurationView.GetAuthorizationProviderData(authorizationProviderName);
            return GetType(authorizationProviderData.TypeName);
        }

        /// <summary>
        /// <para>Gets the default provider name.</para>
        /// </summary>
        /// <returns>The default provider name.</returns>
        protected override string GetDefaultInstanceName()
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            return securityConfigurationView.GetDefaultAuthorizationProviderName();
        }
    }
}