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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// The design time representation of the runtime configuration data
    /// for a profile provider.
    /// </summary>
    [Image(typeof(ProfileProviderCollectionNode))]
    public class ProfileProviderCollectionNode : ConfigurationNode
    {
        private ProfileProviderDataCollection profileProviderDataCollection;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ProfileProviderCollectionNode"/> class.</para>
        /// </summary>
        public ProfileProviderCollectionNode() : this(new ProfileProviderDataCollection())
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ProfileProviderCollectionNode"/> class with configuration data for the provider.</para>
        /// </summary>
        /// <param name="profileProviderDataCollection">
        /// <para>Configuration data to initialize the node.</para>
        /// </param>
        public ProfileProviderCollectionNode(ProfileProviderDataCollection profileProviderDataCollection) : base()
        {
            if (profileProviderDataCollection == null)
            {
                throw new ArgumentNullException("profileProviderDataCollection");
            }
            this.profileProviderDataCollection = profileProviderDataCollection;
        }

        /// <summary>
        /// <para>Gets or sets the name for the node.</para>
        /// </summary>
        /// <value>
        /// <para>The display name for the node.</para>
        /// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual ProfileProviderDataCollection ProfileProviderDataCollection
        {
            get
            {
                profileProviderDataCollection.Clear();
                foreach (ProfileProviderNode node in this.Nodes)
                {
                    profileProviderDataCollection[node.ProfileProviderData.Name] = node.ProfileProviderData;
                }
                return profileProviderDataCollection;
            }
        }

        protected override void OnAddMenuItems()
        {
            ConfigurationMenuItem.CreateValidateNodeCommand(Site, this);
            CreateDynamicMenuItems(typeof(ProfileProviderNode));
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = SR.ProfileProviderCollectionNodeName;
            CreateDynamicNodes(profileProviderDataCollection);
        }
    }
}