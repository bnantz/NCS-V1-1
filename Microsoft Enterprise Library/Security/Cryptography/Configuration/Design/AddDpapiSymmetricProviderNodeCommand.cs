//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Command interceptor for a <see cref="DpapiSymmetricCryptoProviderNode"/>.
    /// </summary>
    public class AddDpapiSymmetricProviderNodeCommand : AddChildNodeCommand
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AddChildNodeCommand"/> class with a name, the <see cref="ConfigurationNode"/> the node is for, and the <see cref="Type"/> to create.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The service provider to get service objects.</para>
        /// </param>
        /// <param name="childType">
        /// <para>
        /// The <see cref="Type"/> object for the configuration node to create and add to the node.
        /// </para>
        /// </param>
        public AddDpapiSymmetricProviderNodeCommand(IServiceProvider serviceProvider, Type childType) : base(serviceProvider, childType)
        {
        }

        protected override void ExecuteCore(ConfigurationNode node)
        {
            DpapiSettingsEditorUI dpapi = new DpapiSettingsEditorUI();
            DialogResult result = dpapi.ShowDialog();

            if (result == DialogResult.OK)
            {
                base.ExecuteCore(node);
                DpapiSymmetricCryptoProviderNode cryptoNode = (DpapiSymmetricCryptoProviderNode)ChildNode;
                DpapiSettings settings = new DpapiSettings(dpapi.DpapiSettingsData);
                cryptoNode.DataProtectionMode = settings;
            }
        }
    }
}