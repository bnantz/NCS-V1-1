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
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
	/// <summary>
	/// Command interceptor for a <see cref="AuthorizationRuleNode"/>.
	/// </summary>
	public class AuthorizationRuleNodeCommandInterceptor : AddChildNodeCommand
	{
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AuthorizationRuleNodeCommandInterceptor"/> class with a name, the <see cref="ConfigurationNode"/> the node is for, and the <see cref="Type"/> to create.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The service provider to get service objects.</para>
        /// </param>
        /// <param name="childType">
        /// <para>
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.
        /// </para>
        /// </param>
        public AuthorizationRuleNodeCommandInterceptor(IServiceProvider serviceProvider, Type childType) : base(serviceProvider, childType)
        {
        }

        /// <summary>
        /// Intercepts Command.Execute() and launches a <c>KeyManager</c> prompting the user to manage a key.
        /// </summary>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            ExpressionEditorFormUI formUI = new ExpressionEditorFormUI();
            formUI.RuleName = SR.AuthorizationRuleNodeName;
            DialogResult result = formUI.ShowDialog();

            if (result == DialogResult.OK) 
            {
                base.ExecuteCore(node);
                AuthorizationRuleNode ruleNode = (AuthorizationRuleNode)ChildNode;
                ruleNode.Expression = formUI.Expression;
            }
        }
       
	}
}
