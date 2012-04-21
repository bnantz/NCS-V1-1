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

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// The design-time representation of the runtime
    /// <see cref="AuthorizationRuleProviderData"/> class.
    /// </summary>
    public class AuthorizationRuleProviderNode : AuthorizationProviderNode
    {
        private AuthorizationRuleProviderData authorizationRuleProviderData;

        /// <summary>
        /// Initializes a new instance of this node.
        /// </summary>
        public AuthorizationRuleProviderNode() : this(new AuthorizationRuleProviderData(SR.AuthorizationRuleProviderName))
        {
        }

        /// <summary>
        /// Initializes a new instance of this node from existing configuration data.
        /// </summary>
        /// <param name="data">Configuration data.</param>
        public AuthorizationRuleProviderNode(AuthorizationRuleProviderData data) : base(data)
        {
            this.authorizationRuleProviderData = data;
        }

        /// <summary>
        /// Overriden in order to make this property non-browsable.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }

        /// <summary>
        /// <para>Adds any configured <see cref="AuthorizationRuleNode"/> objects based on the supplied configuration data.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            foreach (AuthorizationRuleData ruleData in authorizationRuleProviderData.Rules)
            {
                AuthorizationRuleNode ruleNode = new AuthorizationRuleNode(ruleData);
                this.Nodes.Add(ruleNode);
            }
        }

        /// <summary>
        /// <para>Adds the base menu items and a menu item to create <see cref="AuthorizationRuleNode"/> objects.</para>
        /// </summary>
        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            ConfigurationMenuItem item = new ConfigurationMenuItem(SR.AuthorizationRuleCommandName,
                                                                   new AuthorizationRuleNodeCommandInterceptor(Site, typeof(AuthorizationRuleNode)),
                                                                   this,
                                                                   Shortcut.None,
                                                                   SR.GenericCreateStatusText(SR.AuthorizationRuleCommandName),
                                                                   InsertionPoint.New);
            AddMenuItem(item);
        }

        /// <summary>
        /// <para>Gets the <see cref="AuthorizationProviderData"/> configured through the user interface.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="AuthorizationProviderData"/> configured through the user interface.</para>
        /// </value>
        [Browsable(false)]
        public override AuthorizationProviderData AuthorizationProviderData
        {
            get
            {
                this.authorizationRuleProviderData.Rules.Clear();
                foreach (AuthorizationRuleNode ruleNode in this.Nodes)
                {
                    this.authorizationRuleProviderData.Rules.Add(ruleNode.AuthorizationRuleData);
                }
                return base.AuthorizationProviderData;
            }
        }
    }
}