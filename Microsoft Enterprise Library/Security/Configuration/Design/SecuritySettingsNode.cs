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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    [Image(typeof(SecuritySettingsNode))]
    [ServiceDependency(typeof(ILinkNodeService))]
    public class SecuritySettingsNode : ConfigurationNode
    {
        private SecuritySettings securitySettings;

        private AuthenticationProviderNode defaultAuthenticationProviderNode;
        private AuthorizationProviderNode defaultAuthorizationProviderNode;
        private ProfileProviderNode defaultProfileProviderNode;
        private RolesProviderNode defaultRolesProviderNode;
        private SecurityCacheProviderNode defaultSecurityCacheProviderNode;

        private ConfigurationNodeChangedEventHandler onAuthenticationProviderRemoved;
        private ConfigurationNodeChangedEventHandler onAuthorizationProviderRemoved;
        private ConfigurationNodeChangedEventHandler onProfileProviderRemoved;
        private ConfigurationNodeChangedEventHandler onRolesProviderRemoved;
        private ConfigurationNodeChangedEventHandler onSecurityCacheProviderRemoved;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SecuritySettingsNode"/> class.</para>
        /// </summary>
        public SecuritySettingsNode() : this(new SecuritySettings())
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SecuritySettingsNode"/> class with a <see cref="SecuritySettings"/> object.</para>
        /// </summary>
        /// <param name="securitySettings">
        /// <para>The <see cref="SecuritySettings"/> runtime configuration.</para>
        /// </param>
        public SecuritySettingsNode(SecuritySettings securitySettings) : base()
        {
            if (securitySettings == null)
            {
                throw new ArgumentNullException("securitySettings");
            }
            this.securitySettings = securitySettings;
            this.onAuthenticationProviderRemoved += new ConfigurationNodeChangedEventHandler(OnAuthenticationDefaultProviderRemoved);
            this.onAuthorizationProviderRemoved += new ConfigurationNodeChangedEventHandler(OnAuthorizationDefaultProviderRemoved);
            this.onProfileProviderRemoved += new ConfigurationNodeChangedEventHandler(OnProfileDefaultProviderRemoved);
            this.onRolesProviderRemoved += new ConfigurationNodeChangedEventHandler(OnRolesDefaultProviderRemoved);
            this.onSecurityCacheProviderRemoved += new ConfigurationNodeChangedEventHandler(OnSecurityCacheDefaultProviderRemoved);
        }

        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(AuthenticationProviderNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.DefaultProviderDescription)]
        public AuthenticationProviderNode DefaultAuthenticationInstance
        {
            get { return defaultAuthenticationProviderNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                defaultAuthenticationProviderNode = (AuthenticationProviderNode)service.CreateReference(defaultAuthenticationProviderNode,
                                                                                                   value,
                                                                                                   onAuthenticationProviderRemoved,
                                                                                                   null);
                this.securitySettings.DefaultAuthenticationProviderName = (defaultAuthenticationProviderNode != null) ? this.defaultAuthenticationProviderNode.Name : string.Empty;
            }
        }

        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(AuthorizationProviderNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.DefaultProviderDescription)]
        public AuthorizationProviderNode DefaultAuthorizationInstance
        {
            get { return defaultAuthorizationProviderNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                defaultAuthorizationProviderNode = (AuthorizationProviderNode)service.CreateReference(defaultAuthorizationProviderNode,
                                                                                                 value,
                                                                                                 onAuthorizationProviderRemoved,
                                                                                                 null);
                this.securitySettings.DefaultAuthorizationProviderName = (defaultAuthorizationProviderNode != null) ? this.defaultAuthorizationProviderNode.Name : string.Empty;
            }
        }

        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(ProfileProviderNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.DefaultProviderDescription)]
        public ProfileProviderNode DefaultProfileInstance
        {
            get { return defaultProfileProviderNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                defaultProfileProviderNode = (ProfileProviderNode)service.CreateReference(defaultProfileProviderNode,
                                                                                     value,
                                                                                     onProfileProviderRemoved,
                                                                                     null);
                this.securitySettings.DefaultProfileProviderName = (defaultProfileProviderNode  != null) ? this.defaultProfileProviderNode.Name : string.Empty;
            }
        }

        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(RolesProviderNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.DefaultProviderDescription)]
        public RolesProviderNode DefaultRolesInstance
        {
            get { return defaultRolesProviderNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                defaultRolesProviderNode = (RolesProviderNode)service.CreateReference(defaultRolesProviderNode,
                                                                                 value,
                                                                                 onRolesProviderRemoved,
                                                                                 null);
                this.securitySettings.DefaultRolesProviderName = (defaultRolesProviderNode   != null) ? this.defaultRolesProviderNode.Name : string.Empty;
            }
        }

        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(SecurityCacheProviderNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public SecurityCacheProviderNode DefaultSecurityCacheInstance
        {
            get { return defaultSecurityCacheProviderNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                defaultSecurityCacheProviderNode = (SecurityCacheProviderNode)service.CreateReference(defaultSecurityCacheProviderNode,
                                                                                                 value,
                                                                                                 onSecurityCacheProviderRemoved,
                                                                                                 null);
                this.securitySettings.DefaultSecurityCacheProviderName  = (defaultSecurityCacheProviderNode != null) ? this.defaultSecurityCacheProviderNode.Name : string.Empty;
            }
        }

        /// <summary>
        /// <para>Gets the runtime configuration data for the database configuration.</para>
        /// </summary>
        /// <returns>
        /// <para>A <see cref="SecuritySettings"/> reference.</para>
        /// </returns>
        [Browsable(false)]
        public virtual SecuritySettings SecuritySettings
        {
            get
            {
                GetAuthenticationProviders();
                GetRoleProviders();
                GetAuthorizationProviders();
                GetProfileProviders();
                GetSecurityCacheProviders();
                return this.securitySettings;
            }
        }

        private void GetSecurityCacheProviders()
        {
            SecurityCacheProviderCollectionNode securityCacheProviders = Hierarchy.FindNodeByType(typeof(SecurityCacheProviderCollectionNode)) as SecurityCacheProviderCollectionNode;
            if (securityCacheProviders == null) return;
            if (Object.ReferenceEquals(securitySettings.SecurityCacheProviders, securityCacheProviders.SecurityCacheProviderDataCollection)) return;

            securitySettings.SecurityCacheProviders.Clear();
            foreach (SecurityCacheProviderData securityCacheProviderData in securityCacheProviders.SecurityCacheProviderDataCollection)
            {
                this.securitySettings.SecurityCacheProviders[securityCacheProviderData.Name] = securityCacheProviderData;
            }
            
        }
        private void GetProfileProviders()
        {
            ProfileProviderCollectionNode profileProviders = Hierarchy.FindNodeByType(typeof(ProfileProviderCollectionNode)) as ProfileProviderCollectionNode;
            if (profileProviders == null) return;
            if (Object.ReferenceEquals(securitySettings.ProfileProviders, profileProviders.ProfileProviderDataCollection)) return;

            securitySettings.ProfileProviders.Clear();
            foreach (ProfileProviderData profileProviderData in profileProviders.ProfileProviderDataCollection)
            {
                securitySettings.ProfileProviders[profileProviderData.Name] = profileProviderData;
            }
        }

        private void GetAuthorizationProviders()
        {
            AuthorizationProviderCollectionNode authorizationProviders = Hierarchy.FindNodeByType(typeof(AuthorizationProviderCollectionNode)) as AuthorizationProviderCollectionNode;
            if (authorizationProviders == null) return;
            if (Object.ReferenceEquals(securitySettings.AuthorizationProviders, authorizationProviders.AuthorizationProviderDataCollection)) return;

            securitySettings.AuthorizationProviders.Clear();
            foreach (AuthorizationProviderData authorizationProviderData in authorizationProviders.AuthorizationProviderDataCollection)
            {
                securitySettings.AuthorizationProviders[authorizationProviderData.Name] = authorizationProviderData;
            }
        }

        private void GetRoleProviders()
        {
            RolesProviderCollectionNode rolesProviders = Hierarchy.FindNodeByType(typeof(RolesProviderCollectionNode)) as RolesProviderCollectionNode;
            if (rolesProviders == null) return;
            if (Object.ReferenceEquals(securitySettings.RolesProviders, rolesProviders.RolesProviderDataCollection)) return;

            securitySettings.RolesProviders.Clear();
            foreach (RolesProviderData rolesProviderData in rolesProviders.RolesProviderDataCollection)
            {
                this.securitySettings.RolesProviders[rolesProviderData.Name] = rolesProviderData;
            }
        }

        private void GetAuthenticationProviders()
        {
            AuthenticationProviderCollectionNode authenticationProviders = Hierarchy.FindNodeByType(typeof(AuthenticationProviderCollectionNode)) as AuthenticationProviderCollectionNode;
            if (authenticationProviders == null) return;
            if (Object.ReferenceEquals(this.securitySettings.AuthenticationProviders, authenticationProviders.AuthenticationProviderDataCollection)) return;

            securitySettings.AuthenticationProviders.Clear();
            foreach (AuthenticationProviderData authenticationProviderData in authenticationProviders.AuthenticationProviderDataCollection)
            {
                this.securitySettings.AuthenticationProviders[authenticationProviderData.Name] = authenticationProviderData;
            }
        }

        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            ResolveDefaultAuthenticationNode();
            ResolveDefaultAuthorizationNode();
            ResolveDefaultProfileNode();
            ResolveDefaultRolesNode();
            ResolveDefaultSecurityCacheNode();
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.SecuritySettingsNodeName;
            Nodes.Add(new AuthenticationProviderCollectionNode(this.securitySettings.AuthenticationProviders));
            Nodes.Add(new AuthorizationProviderCollectionNode(this.securitySettings.AuthorizationProviders));
            Nodes.Add(new RolesProviderCollectionNode(this.securitySettings.RolesProviders));
            Nodes.Add(new ProfileProviderCollectionNode(this.securitySettings.ProfileProviders));
            Nodes.Add(new SecurityCacheProviderCollectionNode(this.securitySettings.SecurityCacheProviders));
        }

        private void ResolveDefaultAuthenticationNode()
        {
            if ((securitySettings.DefaultAuthenticationProviderName == null) || (securitySettings.DefaultAuthenticationProviderName.Length == 0)) return;

            AuthenticationProviderCollectionNode nodes = this.Hierarchy.FindNodeByType(this, typeof(AuthenticationProviderCollectionNode)) as AuthenticationProviderCollectionNode;
            Debug.Assert(nodes != null, "Could not find the AuthencitationProviderCollectionNode");
            DefaultAuthenticationInstance = Hierarchy.FindNodeByName(nodes, securitySettings.DefaultAuthenticationProviderName) as AuthenticationProviderNode;
        }

        private void ResolveDefaultAuthorizationNode()
        {
            if ((securitySettings.DefaultAuthorizationProviderName == null) || (securitySettings.DefaultAuthorizationProviderName.Length == 0)) return;

            AuthorizationProviderCollectionNode nodes = this.Hierarchy.FindNodeByType(this, typeof(AuthorizationProviderCollectionNode)) as AuthorizationProviderCollectionNode;
            Debug.Assert(nodes != null, "Could not find the AuthorizationProviderCollectionNode");
            DefaultAuthorizationInstance = Hierarchy.FindNodeByName(nodes, securitySettings.DefaultAuthorizationProviderName) as AuthorizationProviderNode;            
        }

        private void ResolveDefaultProfileNode()
        {
            if ((securitySettings.DefaultProfileProviderName == null) || (securitySettings.DefaultProfileProviderName.Length == 0)) return;

            ProfileProviderCollectionNode nodes = this.Hierarchy.FindNodeByType(this, typeof(ProfileProviderCollectionNode)) as ProfileProviderCollectionNode;
            Debug.Assert(nodes != null, "Could not find the ProfileProviderCollectionNode");
            DefaultProfileInstance = Hierarchy.FindNodeByName(nodes, securitySettings.DefaultProfileProviderName) as ProfileProviderNode;
        }

        private void ResolveDefaultRolesNode()
        {
            if ((securitySettings.DefaultRolesProviderName == null) || (securitySettings.DefaultRolesProviderName.Length == 0)) return;

            RolesProviderCollectionNode nodes = this.Hierarchy.FindNodeByType(this, typeof(RolesProviderCollectionNode)) as RolesProviderCollectionNode;
            Debug.Assert(nodes != null, "Could not find the RolesProviderCollectionNode");
            DefaultRolesInstance = Hierarchy.FindNodeByName(nodes, securitySettings.DefaultRolesProviderName) as RolesProviderNode;
        }

        private void ResolveDefaultSecurityCacheNode()
        {
            if ((securitySettings.DefaultSecurityCacheProviderName == null) || (securitySettings.DefaultSecurityCacheProviderName.Length == 0)) return;

            SecurityCacheProviderCollectionNode nodes = this.Hierarchy.FindNodeByType(this, typeof(SecurityCacheProviderCollectionNode)) as SecurityCacheProviderCollectionNode;
            Debug.Assert(nodes != null, "Could not find the SecurityCacheProviderCollectionNode");
            DefaultSecurityCacheInstance = Hierarchy.FindNodeByName(nodes, securitySettings.DefaultSecurityCacheProviderName) as SecurityCacheProviderNode;            
        }

        private void OnAuthenticationDefaultProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.defaultAuthenticationProviderNode = null;
        }

        private void OnAuthorizationDefaultProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.defaultAuthorizationProviderNode = null;
        }

        private void OnProfileDefaultProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.defaultProfileProviderNode = null;
        }

        private void OnRolesDefaultProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.defaultRolesProviderNode = null;
        }

        private void OnSecurityCacheDefaultProviderRemoved(object sender, ConfigurationNodeChangedEventArgs args)
        {
            this.defaultSecurityCacheProviderNode = null;
        }
    }
}