//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using System.Diagnostics;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	/// <summary>
    /// Installer for common event sources. This class is not meant to be used by any callers.
    /// </summary>
	[RunInstaller(true)]
	public class ProjectInstaller : DefaultManagementProjectInstaller
	{
        /// <summary>
        /// Initializes a new instance of <see cref="ProjectInstaller"></see>
        /// </summary>
        public ProjectInstaller()
        {
            InstallEventSource(SR.EnterpriseLibraryCryptography, SR.LogName);
            InstallEventSource(SR.FailureLogSource,  SR.LogName);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">True if called from Dispose, false if called from finalizer</param>
        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );
        }

        private void InstallEventSource(string sourceName, string logName)
        {
            EventLogInstaller defaultLogDestinationSinkNameInstaller = new EventLogInstaller();
            defaultLogDestinationSinkNameInstaller.Source = sourceName;
            defaultLogDestinationSinkNameInstaller.Log = logName;
            Installers.Add(defaultLogDestinationSinkNameInstaller);
        }
	}
}
