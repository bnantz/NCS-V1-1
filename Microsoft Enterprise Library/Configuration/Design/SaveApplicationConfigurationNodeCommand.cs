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
using System.Diagnostics;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a command that will save an application configuration.</para>
    /// </summary>
    public class SaveApplicationConfigurationNodeCommand : ConfigurationNodeCommand
    {
        private bool saveSucceeded;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SaveApplicationConfigurationNodeCommand"/> class with an <see cref="System.IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public SaveApplicationConfigurationNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="SaveApplicationConfigurationNodeCommand"/> class with an <see cref="System.IServiceProvider"/> and if the error service should be cleared after the command executes.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorLog">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        public SaveApplicationConfigurationNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }

        /// <summary>
        /// <para>Saves the application configuration.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to execute the command upon.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            Debug.Assert(node.GetType().Equals(typeof(ApplicationConfigurationNode)));
            if (!DoValidationCommand(node))
            {
                return;
            }
            if (!DoApplicationSave(node))
            {
                return;
            }
        }

        /// <summary>
        /// <para>Determines if the saving of the application was suseccesful.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the saving of the application was suseccesful; otherwise, <see langword="false"/>.</para>
        /// </value>
        public bool SaveSucceeded
        {
            get { return saveSucceeded; }
        }

        private bool DoValidationCommand(ConfigurationNode node)
        {
            using(ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider, false, false))
            {
                cmd.Execute(node.Hierarchy.RootNode);
                if (!cmd.ValidationSucceeded)
                {
                    UIService.ShowMessage(SR.ValidationErrorsMessage, SR.SaveApplicationCaption, MessageBoxButtons.OK);
                    saveSucceeded = false;
                    return false;
                }
                return true;
            }
        }

        private bool DoApplicationSave(ConfigurationNode node)
        {
            // we need to create the meta data for everybody first
            if (!CreateMetaDataConfiguration(node)) return false;

            IStorageTable storageTable = CurrentHierarchy.StorageTable;
            CreateStorageEntries(storageTable);
            if (!CheckAndDisplayErrors())
            {
                return false;
            }
            CurrentHierarchy.ConfigurationContext = new ConfigurationContext(node.Hierarchy.StorageTable.MetaConfigurationFile);
            return SaveHierarchy();
        }

        private bool CreateMetaDataConfiguration(ConfigurationNode node)
        {
            MetaDataStorageCreationCommand cmd = new MetaDataStorageCreationCommand(node.Hierarchy.StorageTable.MetaConfigurationFile, ServiceProvider);
            cmd.Execute();
            if (cmd.CreationCancled)
            {
                return false;
            }
            return true;
        }

        private bool SaveHierarchy()
        {
            CurrentHierarchy.Save();
            return CheckAndDisplayErrors();
        }

        private bool CheckAndDisplayErrors()
        {
            saveSucceeded = true;
            if (ConfigurationErrorLogService.ConfigurationErrors.Count > 0)
            {
                UIService.DisplayErrorLog(ConfigurationErrorLogService);
                UIService.ShowError(SR.SaveApplicationErrorMessage, SR.SaveApplicationCaption);
                saveSucceeded = false;
            }
            return saveSucceeded;
        }

        private void CreateStorageEntries(IStorageTable storageTable)
        {
            StorageCreationCommandDictionary commands = storageTable.GetStorageCreationCommands();
            foreach (StorageCreationCommand cmd in commands.Values)
            {
                cmd.Execute();
            }
        }
    }
}