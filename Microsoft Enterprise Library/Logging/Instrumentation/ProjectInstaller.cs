//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Diagnostics;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
[assembly : Instrumented(@"root\EnterpriseLibrary")]

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary>
    /// Represents the installer for the instrumentation events. Not intended for direct use.
    /// </summary>
    public class ProjectInstaller : ProjectInstallerBase
    {
        /// <summary>
        /// Project installer for instrumentation events.
        /// </summary>
        public ProjectInstaller()
            : base(SR.InstrumentationEventSource,
                   SR.InstrumentationCounterCategory,
                   SR.InstrumentationCounterCategoryHelp,
                   LoggingServiceEvent.counters)
        {
            InstallEventSource(SR.DefaultLogDestinationSinkName, SR.ApplicationLogName);
            InstallEventSource(SR.DistributorEventLoggerDefaultApplicationName, SR.ApplicationLogName);
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