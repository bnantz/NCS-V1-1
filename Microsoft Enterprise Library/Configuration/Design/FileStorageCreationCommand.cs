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
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{

    /// <summary>
    /// <para>Creates configuraiton stored in a file.</para>
    /// </summary>
	public abstract class FileStorageCreationCommand : StorageCreationCommand
	{
	    private string fileName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileStorageCreationCommand"/> class with a file name and <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="fileName"><para>The name of the file to create.</para></param>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
	    protected FileStorageCreationCommand(string fileName, IServiceProvider serviceProvider) : base(fileName, serviceProvider)
		{
            IStorageTable storageTable = ServiceHelper.GetCurrentStorageTable(ServiceProvider);
	        this.fileName = GetAbsolutePathFromMeatConfigPath(storageTable.MetaConfigurationFile, fileName);
		}

        /// <summary>
        /// <para>Gets or sets the name of the file to create.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the file to create.</para>
        /// </value>
	    protected string FileName
	    {
	        get { return fileName; }
            set { fileName = value; }
	    }

        /// <summary>
        /// <para>Determines if the current <see cref="FileName"/> can be overwrriten. It will prompt the user throuth the user interface if the user wants to overwrite the file.</para>
        /// </summary>
        /// <returns>
        /// <para><see langword="true"/> if the file can be overwritten; otherwise, <see langword="false"/>.</para>
        /// </returns>
	    protected bool CanOverwriteFile()
        {
            if (FileHelper.IsFileReadOnly(fileName))
            {
                IUIService uiService = ServiceHelper.GetUIService(ServiceProvider);            
                DialogResult result = uiService.ShowMessage(SR.OverwriteFileMessage(fileName), SR.OverwriteFileCaption, System.Windows.Forms.MessageBoxButtons.YesNo);
                if (DialogResult.Yes == result)
                {
                    FileHelper.ChangeFileAttributesToWritable(fileName);
                }
                else
                {
                    IConfigurationErrorLogService logService = ServiceHelper.GetConfigurationErrorService(ServiceProvider);
                    logService.LogError(new ConfigurationError(null, SR.ExceptionFilesNotSaved));
                    return false;
                }
            }
            return true;
        }

        private string GetAbsolutePathFromMeatConfigPath(string metaConfigurationFile, string filePath)
        {
            if (filePath == null || filePath.Length == 0) return filePath;
            if (!Path.IsPathRooted(filePath))
            {
                if (metaConfigurationFile == null || metaConfigurationFile.Length == 0) return filePath;
                filePath = Path.Combine(Path.GetDirectoryName(metaConfigurationFile), filePath);
            }
            return filePath.ToLower(CultureInfo.InvariantCulture);
        }
	}
}
