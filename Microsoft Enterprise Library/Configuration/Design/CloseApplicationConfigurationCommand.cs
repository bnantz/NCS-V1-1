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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a command that will close an <see cref="ApplicationConfigurationNode"/> and remove the <see cref="IUIHierarchy"/> that it represents.</para>
    /// </summary>
    public class CloseApplicationConfigurationCommand : ConfigurationNodeCommand
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CloseApplicationConfigurationCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public CloseApplicationConfigurationCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CloseApplicationConfigurationCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The service provider to get service objects.</para>
        /// </param>
        /// <param name="clearErrorService">
        /// <para>Determines if the <see cref="IConfigurationErrorLogService"/> when the command has executed.</para>
        /// </param>
        public CloseApplicationConfigurationCommand(IServiceProvider serviceProvider, bool clearErrorService) : base(serviceProvider, clearErrorService)
        {
        }

        /// <summary>
        /// <para>Closes the application configuration.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to execute the command upon.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                if (UIService.IsDirty(node.Hierarchy))
                {
                    DialogResult result = UIService.ShowMessage(SR.SaveApplicationRequest, SR.SaveApplicationCaption, MessageBoxButtons.YesNo);
                    if (DialogResult.Yes == result)
                    {
                        if (!TryAndSaveApplication(node))
                        {
                            return;
                        }
                    }
                }
                if (ConfigurationErrorLogService.ConfigurationErrors.Count > 0)
                {
                    UIService.DisplayErrorLog(ConfigurationErrorLogService);
                    DialogResult result = UIService.ShowMessage(SR.SaveApplicationErrorRequestMessage, SR.SaveApplicationCaption, MessageBoxButtons.YesNo);
                    if (result == DialogResult.No) return;
                }
                UIHierarchyService.RemoveHierarchy(node.Hierarchy.Id);
            }
            finally
            {
                UIService.EndUpdate();
            }
        }

        private bool TryAndSaveApplication(ConfigurationNode node)
        {
            SaveApplicationConfigurationNodeCommand cmd = new SaveApplicationConfigurationNodeCommand(ServiceProvider);
            cmd.Execute(node);
            return cmd.SaveSucceeded;
        }
    }
}