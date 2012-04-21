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
    /// Provides methods for the creation of <see cref="IAuthenticationProvider"/> objects.
    /// </summary>
    public class AuthenticationProviderFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="AuthenticationProviderFactory"/> class.</para>
        /// </summary>
        public AuthenticationProviderFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="AuthenticationProviderFactory"/> class with a <see cref="ConfigurationContext"/>.</para>
        /// </summary>
        /// <param name="context"><para>A <see cref="ConfigurationContext"/> containing the configuration data to use.</para></param>
        public AuthenticationProviderFactory(ConfigurationContext context) : base(SR.Authentication, context, typeof(IAuthenticationProvider))
        {
        }

        /// <summary>
        /// Gets the default authentication provider as specified via configuration.
        /// </summary>
        /// <returns>An <see cref="IAuthenticationProvider"/> object.</returns>
        public IAuthenticationProvider GetAuthenticationProvider()
        {
            return (IAuthenticationProvider)base.CreateDefaultInstance();
        }

        /// <summary>
        /// Gets the authentication provider with the specified name.
        /// </summary>
        /// <param name="authenticationProviderName">The configured name of an authentication provider</param>
        /// <returns>An <see cref="IAuthenticationProvider"/> object.</returns>
        public IAuthenticationProvider GetAuthenticationProvider(string authenticationProviderName)
        {
            return (IAuthenticationProvider)base.CreateInstance(authenticationProviderName);
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
        /// <para>Gets the configuration object <see cref="Type"/> for the factory to create given the <paramref name="authenticationProviderName"/>.</para>
        /// </summary>
        /// <param name="authenticationProviderName">
        /// <para>The name of the <see cref="IAuthenticationProvider"/> to create.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="IAuthenticationProvider"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string authenticationProviderName)
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            AuthenticationProviderData authenticationProviderData = securityConfigurationView.GetAuthenticationProviderData(authenticationProviderName);
            return GetType(authenticationProviderData.TypeName);
        }

        /// <summary>
        /// <para>Gets the default provider name.</para>
        /// </summary>
        /// <returns>The default provider name.</returns>
        protected override string GetDefaultInstanceName()
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            return securityConfigurationView.GetDefaultAuthenticationProviderName();
        }
    }
}