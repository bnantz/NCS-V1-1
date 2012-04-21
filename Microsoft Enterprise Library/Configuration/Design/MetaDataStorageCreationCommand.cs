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
using System.Xml;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Storage creation command to create the meta data configuration file.
    /// </devdoc>
	internal class MetaDataStorageCreationCommand : FileStorageCreationCommand
	{
        private bool creationCancled;

	    public MetaDataStorageCreationCommand(string fileName, IServiceProvider serviceProvider) : base(fileName, serviceProvider)
		{
		}

	    public bool CreationCancled
	    {
	        get { return creationCancled; }
	    }

	    public override void Execute()
	    {
            if (!EnsureConfigurationFileIsSet())
            {
                creationCancled = true;
                return;
            }
            
            if (File.Exists(FileName))
	        {
	            return;
	        }

	        CreateFile();
	    }

        private void CreateFile()
        {
            XmlDocument document = new XmlDocument();
            XmlElement element = document.CreateElement("configuration");
            document.AppendChild(element);
    
            if (CanOverwriteFile())
            {
                try
                {
                    document.Save(FileName);
                }
                catch (IOException e)
                {
                    LogError(e.Message);
                }
                catch (UnauthorizedAccessException e)
                {
                    LogError(e.Message);
                }
            }
        }

        private bool EnsureConfigurationFileIsSet()
        {
            if ((FileName == null) || (FileName.Length == 0))
            {
                if (DialogResult.OK != GetApplicationFileFromUI())
                {
                    return false;
                }
            }
            return true;
        }

        private DialogResult GetApplicationFileFromUI()
        {
            ApplicationConfigurationNode node = (ApplicationConfigurationNode)ServiceHelper.GetCurrentRootNode(ServiceProvider);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = SR.ConfigurationFileDialogFilter;
            dialog.Title = System.String.Concat(SR.SaveApplicationCaption, " : ", node.Name);
            IUIService uiService = ServiceHelper.GetUIService(ServiceProvider);
            System.Windows.Forms.DialogResult result = uiService.ShowSaveDialog(dialog);
            if (System.Windows.Forms.DialogResult.OK == result)
            {
                FileName = dialog.FileName;
                node.ConfigurationFile = FileName;
                node.Hierarchy.StorageTable.MetaConfigurationFile = FileName;
            }
            return result;
        }

        private void LogError(string message)
        {
            IConfigurationErrorLogService logService = ServiceHelper.GetConfigurationErrorService(ServiceProvider);
            IUIHierarchyService uiHierarchyService = ServiceHelper.GetUIHierarchyService(ServiceProvider);
            logService.LogError(new ConfigurationError(uiHierarchyService.SelectedHierarchy.RootNode, message));
        }
	}
}
