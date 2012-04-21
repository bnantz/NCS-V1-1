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
    /// Provides methods for the creation of <see cref="IRolesProvider"/> objects.
    /// </summary>
    public class RolesProviderFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="RolesProviderFactory"/> class.</para>
        /// </summary>
        public RolesProviderFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesProviderFactory"/> class with a specified <see cref="ConfigurationContext"/>.
        /// </summary>
        /// <param name="context">A <see cref="ConfigurationContext"/>.</param>
        public RolesProviderFactory(ConfigurationContext context) : base(SR.Roles, context, typeof(IRolesProvider))
        {
        }

        /// <summary>
        /// Gets the default roles provider as specified via configuration.
        /// </summary>
        /// <returns>An <see cref="IRolesProvider"/> object.</returns>
        public IRolesProvider GetRolesProvider()
        {
            return (IRolesProvider)base.CreateDefaultInstance();
        }

        /// <summary>
        /// Gets the roles provider with the specified name.
        /// </summary>
        /// <param name="roleProviderName">The configured name of an roles provider</param>
        /// <returns>An <see cref="IRolesProvider"/> object.</returns>
        public IRolesProvider GetRolesProvider(string roleProviderName)
        {
            return (IRolesProvider)base.CreateInstance(roleProviderName);
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
        /// <para>Gets the configuration object <see cref="Type"/> for the factory to create given the <paramref name="roleProviderName"/>.</para>
        /// </summary>
        /// <param name="roleProviderName">
        /// <para>The name of the <see cref="IRolesProvider"/> to create.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="IRolesProvider"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string roleProviderName)
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            RolesProviderData rolesProviderData = securityConfigurationView.GetRolesProviderData(roleProviderName);
            return GetType(rolesProviderData.TypeName);
        }

        /// <summary>
        /// <para>Gets the default provider name.</para>
        /// </summary>
        /// <returns>The default provider name.</returns>
        protected override string GetDefaultInstanceName()
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            return securityConfigurationView.GetDefaultRoleProviderName();
        }
    }
}