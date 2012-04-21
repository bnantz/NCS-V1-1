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
    /// <see cref="ISecurityCacheProvider"/> objects.
    /// </summary>
    public class SecurityCacheProviderFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SecurityCacheProviderFactory"/> class.</para>
        /// </summary>
        public SecurityCacheProviderFactory() : this(ConfigurationManager.GetCurrentContext())
        {
        }

        /// <summary>
        /// Creates an instance of the factory.
        /// </summary>
        /// <param name="context">The current configuration context.</param>
        public SecurityCacheProviderFactory(ConfigurationContext context) : base(SR.SecurityCache, context, typeof(ISecurityCacheProvider))
        {
        }

        /// <summary>
        /// Gets the default security cache provider as specified via configuration.
        /// </summary>
        /// <returns>An <see cref="ISecurityCacheProvider"/> object.</returns>
        public ISecurityCacheProvider GetSecurityCacheProvider()
        {
            return (ISecurityCacheProvider)base.CreateDefaultInstance();
        }

        /// <summary>
        /// Gets the security cache provider with the specified name.
        /// </summary>
        /// <param name="name">The configured name of an security cache provider</param>
        /// <returns>An <see cref="ISecurityCacheProvider"/> object.</returns>
        public ISecurityCacheProvider GetSecurityCacheProvider(string name)
        {
            return (ISecurityCacheProvider)base.CreateInstance(name);
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
        /// <para>Gets the configuration object <see cref="Type"/> for the factory to create given the <paramref name="securityCacheProviderName"/>.</para>
        /// </summary>
        /// <param name="securityCacheProviderName">
        /// <para>The name of the <see cref="ISecurityCacheProvider"/> to create.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="ISecurityCacheProvider"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string securityCacheProviderName)
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            SecurityCacheProviderData securityCacheProviderData = securityConfigurationView.GetSecurityCacheProviderData(securityCacheProviderName);
            return GetType(securityCacheProviderData.TypeName);
        }

        /// <summary>
        /// <para>Gets the default provider name.</para>
        /// </summary>
        /// <returns>The default provider name.</returns>
        protected override string GetDefaultInstanceName()
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            return securityConfigurationView.GetDefaultSecurityCacheProviderName();
        }
    }
}