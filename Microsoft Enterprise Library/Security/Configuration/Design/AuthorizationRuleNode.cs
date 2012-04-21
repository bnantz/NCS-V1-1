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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// The design-time representation of the 
    /// <see cref="AuthorizationRuleData"/> class.
    /// </summary>
    [Image(typeof(AuthorizationRuleNode))]
    public class AuthorizationRuleNode : ConfigurationNode
    {
        private AuthorizationRuleData authorizationRuleData;

        /// <summary>
        /// Initializes a new instance of the node.
        /// </summary>
        public AuthorizationRuleNode() : this(new AuthorizationRuleData(SR.AuthorizationRuleNodeName, string.Empty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the node from existing configuration data.
        /// </summary>
        /// <param name="authorizationRuleData">Configuration data for a generic authorization provider</param>
        public AuthorizationRuleNode(AuthorizationRuleData authorizationRuleData) : base()
        {
            if (authorizationRuleData == null)
            {
                throw new ArgumentNullException("authorizationRuleData");
            }
            this.authorizationRuleData = authorizationRuleData;
        }

        /// <summary>
        /// Gets or sets the expression for the current rule.
        /// </summary>
        [Required]
        [Editor(typeof(ExpressionEditor), typeof(UITypeEditor))]
        [ValidExpression]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string Expression
        {
            get { return this.authorizationRuleData.Expression; }
            set { this.authorizationRuleData.Expression = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual AuthorizationRuleData AuthorizationRuleData
        {
            get { return this.authorizationRuleData; }
        }

        /// <summary>
        /// <para>Sets the name for the node based on the configuration name.</para>
        /// </summary>
        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = authorizationRuleData.Name;
        }

        /// <summary>
        /// <para>Raises the <see cref="ConfigurationNode.Renamed"/> event and sets the name of the configuration data.</para>
        /// </summary>
        /// <param name="e"><para>A <see cref="ConfigurationNodeChangedEventArgs"/> that contains the event data.</para></param>
        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed(e);
            authorizationRuleData.Name = e.Node.Name;
        }

    }
}