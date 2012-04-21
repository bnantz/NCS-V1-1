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

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents the configuration data for the 
    /// security providers.
    /// </summary>
    [XmlRoot("enterpriseLibrary.securitySettings", Namespace=SecuritySettings.ConfigurationNamespace)]
    public class SecuritySettings
    {
        private RolesProviderDataCollection rolesProviders;
        private AuthorizationProviderDataCollection authorizationProviders;
        private ProfileProviderDataCollection profileProviders;
        private AuthenticationProviderDataCollection authenticationProviders;
        private SecurityCacheProviderDataCollection securityCacheProviders;

        private string defaultAuthorizationProviderName;
        private string defaultAuthenticationProviderName;
        private string defaultRolesProviderName;
        private string defaultProfileProviderName;
        private string defaultSecurityCacheProviderName;

        /// <summary>
        /// The name of the configuration section for the security providers.
        /// </summary>
        public const string SectionName = "securityConfiguration";

        /// <summary>
        /// <para>Gets the Xml namespace for this root node.</para>
        /// </summary>
        /// <value>
        /// <para>The Xml namespace for this root node.</para>
        /// </value>
        public const string ConfigurationNamespace = "http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/security";

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="SecuritySettings"/> class.
        /// </summary>
        public SecuritySettings()
        {
            this.rolesProviders = new RolesProviderDataCollection();
            this.authorizationProviders = new AuthorizationProviderDataCollection();
            this.profileProviders = new ProfileProviderDataCollection();
            this.authenticationProviders = new AuthenticationProviderDataCollection();
            this.securityCacheProviders = new SecurityCacheProviderDataCollection();
            defaultAuthorizationProviderName = string.Empty;
            defaultAuthenticationProviderName = string.Empty;
            defaultRolesProviderName = string.Empty;
            defaultProfileProviderName = string.Empty;
            defaultSecurityCacheProviderName = string.Empty;
        }

        /// <summary>
        /// The instance name of the default <see cref="IAuthorizationProvider"/> instance.
        /// </summary>
        [XmlAttribute("defaultAuthorizationInstance")]
        public string DefaultAuthorizationProviderName
        {
            get { return defaultAuthorizationProviderName; }
            set { defaultAuthorizationProviderName = value; }
        }

        /// <summary>
        /// The instance name of the default <see cref="IAuthenticationProvider"/> instance.
        /// </summary>
        [XmlAttribute("defaultAuthenticationInstance")]
        public string DefaultAuthenticationProviderName
        {
            get { return defaultAuthenticationProviderName; }
            set { defaultAuthenticationProviderName = value; }
        }

        /// <summary>
        /// The instance name of the default <see cref="IRolesProvider"/> instance.
        /// </summary>
        [XmlAttribute("defaultRolesInstance")]
        public string DefaultRolesProviderName
        {
            get { return defaultRolesProviderName; }
            set { defaultRolesProviderName = value; }
        }

        /// <summary>
        /// The instance name of the default <see cref="IProfileProvider"/> instance.
        /// </summary>
        [XmlAttribute("defaultProfileInstance")]
        public string DefaultProfileProviderName
        {
            get { return defaultProfileProviderName; }
            set { defaultProfileProviderName = value; }
        }

        /// <summary>
        /// The instance name of the default <see cref="ISecurityCacheProvider"/> instance.
        /// </summary>
        [XmlAttribute("defaultSecurityCacheInstance")]
        public string DefaultSecurityCacheProviderName
        {
            get { return defaultSecurityCacheProviderName; }
            set { defaultSecurityCacheProviderName = value; }
        }

        /// <summary>
        /// <para>Gets the <see cref="RolesProviderDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The roles providers available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>rolesProviders</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="rolesProviders", Namespace=SecuritySettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="rolesProvider", Type=typeof(RolesProviderData), Namespace=SecuritySettings.ConfigurationNamespace)]
        public RolesProviderDataCollection RolesProviders
        {
            get { return this.rolesProviders; }
        }

        /// <summary>
        /// <para>Gets the <see cref="AuthorizationProviderDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The authorization providers available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>authorizationProviders</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="authorizationProviders", Namespace=SecuritySettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="authorizationProvider", Type=typeof(AuthorizationProviderData), Namespace=SecuritySettings.ConfigurationNamespace)]
        public AuthorizationProviderDataCollection AuthorizationProviders
        {
            get { return this.authorizationProviders; }
        }

        /// <summary>
        /// <para>Gets the <see cref="AuthenticationProviderDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The authentication providers available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>authenticationProviders</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="authenticationProviders", Namespace=SecuritySettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="authenticationProvider", Type=typeof(AuthenticationProviderData), Namespace=SecuritySettings.ConfigurationNamespace)]
        public AuthenticationProviderDataCollection AuthenticationProviders
        {
            get { return this.authenticationProviders; }
        }

        /// <summary>
        /// <para>Gets the <see cref="ProfileProviderDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The profile providers available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>profileProviders</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="profileProviders", Namespace=SecuritySettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="profileProvider", Type=typeof(ProfileProviderData), Namespace=SecuritySettings.ConfigurationNamespace)]
        public ProfileProviderDataCollection ProfileProviders
        {
            get { return this.profileProviders; }
        }

        /// <summary>
        /// <para>Gets the <see cref="SecurityCacheProviderDataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The security cache providers available in configuration. The default is an empty collection.</para>
        /// </value>
        /// <remarks>
        /// <para>This property maps to the <c>securityCacheProviders</c> element in configuration.</para>
        /// </remarks>
        [XmlArray(ElementName="securityCacheProviders", Namespace=SecuritySettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="securityCacheProvider", Type=typeof(SecurityCacheProviderData), Namespace=SecuritySettings.ConfigurationNamespace)]
        public SecurityCacheProviderDataCollection SecurityCacheProviders
        {
            get { return this.securityCacheProviders; }
        }
    }
}