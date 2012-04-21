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
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Command interceptor for a <see cref="SymmetricAlgorithmProviderNode"/>.
    /// </summary>
    public class AddSymmetricAlgorithmProviderNodeCommand : AddChildNodeCommand
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
        public AddSymmetricAlgorithmProviderNodeCommand(IServiceProvider serviceProvider, Type childType) : base(serviceProvider, childType)
        {
        }

        protected override void ExecuteCore(ConfigurationNode node)
        {
            TypeSelectorUI selector = new TypeSelectorUI(
                typeof(RijndaelManaged),
                typeof(SymmetricAlgorithm),
                TypeSelectorIncludeFlags.Default
                );
            DialogResult typeResult = selector.ShowDialog();
            if (typeResult == DialogResult.OK)
            {
                KeySettings keySettings = new KeySettings(new SymmetricAlgorithmKeyCreator(selector.SelectedType.AssemblyQualifiedName));
                KeyManagerEditorUI keyManager = new KeyManagerEditorUI(keySettings);
                DialogResult keyResult = keyManager.ShowDialog();

                if (keyResult == DialogResult.OK)
                {
                    INodeNameCreationService service = GetService(typeof(INodeNameCreationService)) as INodeNameCreationService;
                    Debug.Assert(service != null, "Could not find the INodeNameCreationService");
                    base.ExecuteCore(node);
                    SymmetricAlgorithmProviderNode providerNode = (SymmetricAlgorithmProviderNode)ChildNode;
                    providerNode.AlgorithmType = selector.SelectedType.AssemblyQualifiedName;
                    providerNode.Name = service.GetUniqueDisplayName(providerNode.Parent, selector.SelectedType.Name);
                    providerNode.Key = keyManager.KeySettings;
                }
            }
        }
    }
}