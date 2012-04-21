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
using System.IO;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a command that will open an application configuration.</para>
    /// </summary>
    public class OpenApplicationConfigurationCommand : ConfigurationNodeCommand
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="OpenApplicationConfigurationCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public OpenApplicationConfigurationCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="OpenApplicationConfigurationCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorLog">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        public OpenApplicationConfigurationCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }

        /// <summary>
        /// <para>Opens a previously saved configuration.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to execute the command upon.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                OpenFile();
                if (ConfigurationErrorLogService.ConfigurationErrors.Count > 0)
                {
                    UIService.DisplayErrorLog(ConfigurationErrorLogService);
                    UIService.ShowMessage(SR.OpenApplicationErrorMessage, SR.OpenApplicationCaption);
                }
            }
            finally
            {
                UIService.EndUpdate();
            }
        }

        private void OpenFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = SR.ConfigurationFileDialogFilter;
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = ".config";
            fileDialog.RestoreDirectory = true;
            DialogResult result = UIService.ShowOpenDialog(fileDialog);

            if (result == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                ApplicationData data = new ApplicationData(string.Empty, Path.GetDirectoryName(file), file);
                IUIHierarchy hierarchy = new UIHierarchy(new ApplicationConfigurationNode(data), ServiceProvider, new ConfigurationContext(file));
                UIHierarchyService.AddHierarchy(hierarchy);
                hierarchy.Open();
            }
        }
    }
}