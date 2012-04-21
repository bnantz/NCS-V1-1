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

using System;
using System.Diagnostics;
using System.Globalization;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    [InstrumentationClass(InstrumentationType.Instance)]
    public class ApplicationData : Instance
    {
        /// <summary/>
        /// <exclude/>
        public ApplicationData()
        {
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The id of the process.
        /// </devdoc>
        public string ProcessIdentifier
        {
            get { return Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The name of the process.
        /// </devdoc>
        public string ProcessName
        {
            get { return Process.GetCurrentProcess().ProcessName; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The base directory.
        /// </devdoc>
        public string BaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The configuration file.
        /// </devdoc>
        public string ConfigFile
        {
            get { return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The name of the performance counter instance for the purpose of cross-referencing the performance counters.
        /// </devdoc>
        public string PerformanceCounterInstanceName
        {
            get { return PerformanceCounterInstances.CurrentInstanceName; }
        }

        /// <devdoc>
        /// This method wraps the BaseEvent.Published property so that it can have a ConditionalAttribute that can be used to turn off WMI.
        /// </devdoc>
        [Conditional("USEWMI")]
        internal void Publish()
        {
            this.Published = true;
        }
    }
}