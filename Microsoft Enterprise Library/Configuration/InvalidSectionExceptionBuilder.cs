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
using System.Configuration;
using System.Globalization;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
	
	internal class InvalidSectionExceptionBuilder
	{
	    private readonly string sectionName;
	    private readonly string fileName;

	    public InvalidSectionExceptionBuilder(string sectionName, string fileName)
		{
	        this.fileName = fileName;
	        this.sectionName = sectionName;
		}

        public ConfigurationException ThrowException()
        {
            string message = string.Empty;
            if (string.Compare(Path.GetFileName(fileName), Path.GetFileName(ConfigurationBuilder.MachineConfigurationFile), true, CultureInfo.InvariantCulture) == 0)
            {
                message = SR.ExceptionSectionNameInvalid(sectionName, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, fileName);
            }
            else
            {
                message = SR.ExceptionSectionNameInvalid(sectionName, fileName, ConfigurationBuilder.MachineConfigurationFile);
            }
            return new ConfigurationException(message);
        }

	}
}
