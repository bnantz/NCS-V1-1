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
    /// Provides methods for the creation of <see cref="IProfileProvider"/> objects.
    /// </summary>
    public class ProfileProviderFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ProfileProviderFactory"/> class.</para>
        /// </summary>
        public ProfileProviderFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileProviderFactory"/>
        /// class with a specified <see cref="ConfigurationContext"/>.
        /// </summary>
        /// <param name="context">A <see cref="ConfigurationContext"/>.</param>
        public ProfileProviderFactory(ConfigurationContext context) : base(SR.Profile, context, typeof(IProfileProvider))
        {
        }

        /// <summary>
        /// Gets the default profile provider as specified via configuration.
        /// </summary>
        /// <returns>An <see cref="IProfileProvider"/> object.</returns>
        public IProfileProvider GetProfileProvider()
        {
            return (IProfileProvider)base.CreateDefaultInstance();
        }

        /// <summary>
        /// Gets the profile provider with the specified name.
        /// </summary>
        /// <param name="name">The configured name of an profile provider</param>
        /// <returns>An <see cref="IProfileProvider"/> object.</returns>
        public IProfileProvider GetProfileProvider(string name)
        {
            return (IProfileProvider)base.CreateInstance(name);
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
        /// <para>Gets the configuration object <see cref="Type"/> for the factory to create given the <paramref name="profileProviderName"/>.</para>
        /// </summary>
        /// <param name="profileProviderName">
        /// <para>The name of the <see cref="IProfileProvider"/> to create.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="IProfileProvider"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string profileProviderName)
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            ProfileProviderData profileProviderData = securityConfigurationView.GetProfileProviderData(profileProviderName);
            return GetType(profileProviderData.TypeName);
        }

        /// <summary>
        /// <para>Gets the default provider name.</para>
        /// </summary>
        /// <returns>The default provider name.</returns>
        protected override string GetDefaultInstanceName()
        {
        	SecurityConfigurationView securityConfigurationView = (SecurityConfigurationView)CreateConfigurationView();
            return securityConfigurationView.GetDefaultProfileProviderName();
        }
    }
}