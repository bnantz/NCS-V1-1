//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <devdoc>
    /// Create an encryption settings node from the wizard.
    /// </devdoc>
    internal class AddFileKeyAlgorithmPairNodeCommand : AddChildNodeCommand
    {
        public AddFileKeyAlgorithmPairNodeCommand(IServiceProvider serviceProvider, Type childType) : base(serviceProvider, childType)
        {
        }

        protected override void ExecuteCore(ConfigurationNode node)
        {
            AddNewKeyAlgorithmWizard wizard = new AddNewKeyAlgorithmWizard();

            if (wizard.ShowDialog() == DialogResult.OK)
            {
                FileKeyAlgorithmStorageProviderWizard fileWizard = new FileKeyAlgorithmStorageProviderWizard(wizard.KeyAlgorithmPair, ServiceProvider);

                if (fileWizard.ShowDialog() == DialogResult.OK)
                {
                    base.ExecuteCore(node);
                    FileKeyAlgorithmPairStorageProviderNode pairStorageNode = (FileKeyAlgorithmPairStorageProviderNode)ChildNode;
                    pairStorageNode.DpapiSettings = fileWizard.DpapiSettings;
                    pairStorageNode.File = fileWizard.Path;
                }
            }
        }

    }
}
