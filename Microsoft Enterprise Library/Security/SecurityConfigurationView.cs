//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================0

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// <para>Represents a view for navigating the <see cref="SecuritySettings"/> configuration data.</para>
    /// </summary>
    public class SecurityConfigurationView : ConfigurationView
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SecurityConfigurationView"/> class with a <see cref="ConfigurationContext"/> object.</para>
        /// </summary>
        /// <param name="configurationContext">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        public SecurityConfigurationView(ConfigurationContext configurationContext) : base(configurationContext)
        {
        }

        /// <summary>
        /// <para>Gets the <see cref="SecuritySettings"/> configuration data.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="SecuritySettings"/> configuration data.</para>
        /// </returns>
        public virtual SecuritySettings GetSecuritySettings()
        {
            return (SecuritySettings)ConfigurationContext.GetConfiguration(SecuritySettings.SectionName);
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="SecurityCacheProviderData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="SecurityCacheProviderData"/>.</para>
        /// </returns>
        public virtual string GetDefaultSecurityCacheProviderName()
        {
            SecuritySettings settings = GetSecuritySettings();
            return settings.DefaultSecurityCacheProviderName;
        }

        /// <summary>
        /// <para>Gets the named <see cref="SecurityCacheProviderData"/> from configuration.</para>
        /// </summary>
        /// <param name="securityCacheProviderName">
        /// <para>The name of the <see cref="SecurityCacheProviderData"/> to get the data.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="SecurityCacheProviderData"/> from configuration.</para>
        /// </returns>
        public virtual SecurityCacheProviderData GetSecurityCacheProviderData(string securityCacheProviderName)
        {
            SecuritySettings settings = GetSecuritySettings();
            SecurityCacheProviderData data = settings.SecurityCacheProviders[securityCacheProviderName];
            if (data == null)
            {
                throw new ConfigurationException(SR.ExceptionSecurityCacheProviderNotFound(securityCacheProviderName));
            }
            return data;
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="RolesProviderData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="RolesProviderData"/>.</para>
        /// </returns>
        public virtual string GetDefaultRoleProviderName()
        {
            SecuritySettings settings = GetSecuritySettings();
            return settings.DefaultRolesProviderName;
        }

        /// <summary>
        /// <para>Gets the named <see cref="RolesProviderData"/> from configuration.</para>
        /// </summary>
        /// <param name="roleProviderName">
        /// <para>The name of the <see cref="RolesProviderData"/> to get the data.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="RolesProviderData"/> from configuration.</para>
        /// </returns>
        public virtual RolesProviderData GetRolesProviderData(string roleProviderName)
        {
            SecuritySettings settings = GetSecuritySettings();
            RolesProviderData data = settings.RolesProviders[roleProviderName];
            if (data == null)
            {
                throw new ConfigurationException(SR.ExceptionRolesProviderNotFound(roleProviderName));
            }
            return data;
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="ProfileProviderData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="ProfileProviderData"/>.</para>
        /// </returns>
        public virtual string GetDefaultProfileProviderName()
        {
            SecuritySettings settings = GetSecuritySettings();
            return settings.DefaultProfileProviderName;
        }

        /// <summary>
        /// <para>Gets the named <see cref="ProfileProviderData"/> from configuration.</para>
        /// </summary>
        /// <param name="profileProviderName">
        /// <para>The name of the <see cref="ProfileProviderData"/> to get the data.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="ProfileProviderData"/> from configuration.</para>
        /// </returns>
        public virtual ProfileProviderData GetProfileProviderData(string profileProviderName)
        {
            SecuritySettings settings = GetSecuritySettings();
            ProfileProviderData data = settings.ProfileProviders[profileProviderName];
            if (data == null)
            {
                throw new ConfigurationException(SR.ExceptionProfileProviderNotFound(profileProviderName));
            }
            return data;
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="AuthorizationProviderData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="AuthorizationProviderData"/>.</para>
        /// </returns>
        public virtual string GetDefaultAuthorizationProviderName()
        {
            SecuritySettings settings = GetSecuritySettings();
            return settings.DefaultAuthorizationProviderName;
        }

        /// <summary>
        /// <para>Gets the named <see cref="AuthorizationProviderData"/> from configuration.</para>
        /// </summary>
        /// <param name="authorizationProviderName">
        /// <para>The name of the <see cref="AuthorizationProviderData"/> to get the data.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="AuthorizationProviderData"/> from configuration.</para>
        /// </returns>
        public virtual AuthorizationProviderData GetAuthorizationProviderData(string authorizationProviderName)
        {
            SecuritySettings settings = GetSecuritySettings();
            AuthorizationProviderData data = settings.AuthorizationProviders[authorizationProviderName];
            if (data == null)
            {
                throw new ConfigurationException(SR.ExceptionAuthorizationProviderNotFound(authorizationProviderName));
            }
            return data;
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="AuthenticationProviderData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="AuthenticationProviderData"/>.</para>
        /// </returns>
        public virtual string GetDefaultAuthenticationProviderName()
        {
            SecuritySettings settings = GetSecuritySettings();
            return settings.DefaultAuthenticationProviderName;
        }

        /// <summary>
        /// <para>Gets the named <see cref="AuthenticationProviderData"/> from configuration.</para>
        /// </summary>
        /// <param name="authenticationProviderName">
        /// <para>The name of the <see cref="AuthenticationProviderData"/> to get the data.</para>
        /// </param>
        /// <returns>
        /// <para>The named <see cref="AuthenticationProviderData"/> from configuration.</para>
        /// </returns>
        public virtual AuthenticationProviderData GetAuthenticationProviderData(string authenticationProviderName)
        {
            SecuritySettings settings = GetSecuritySettings();
            AuthenticationProviderData data = settings.AuthenticationProviders[authenticationProviderName];
            if (data == null)
            {
                throw new ConfigurationException(SR.ExceptionAuthenticationProviderNotFound(authenticationProviderName));
            }
            return data;
        }
    }
}