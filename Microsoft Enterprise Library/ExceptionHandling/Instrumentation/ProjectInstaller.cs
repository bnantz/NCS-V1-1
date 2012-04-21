//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Management.Instrumentation;
[assembly : Instrumented(@"root\EnterpriseLibrary")]

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    [RunInstaller(true)]
    [System.ComponentModel.DesignerCategory("Code")]
    public class ProjectInstaller : DefaultManagementProjectInstaller
    {
        /// <summary/>
        /// <exclude/>
        public ProjectInstaller()
        {
            EventLogInstaller eventlogInstaller = new EventLogInstaller();
            eventlogInstaller.Log = "Application";
            eventlogInstaller.Source = ExceptionHandlingEvent.EventSourceName;
            Installers.Add(eventlogInstaller);
            InstallEventSource(SR.EntLibExceptionHandlerSource, "Application");
        }

        /// <summary/>
        /// <param name="stateSaver"/>
        /// <exclude/>
        public override void Install(IDictionary stateSaver)
        {
            // Delete the counter category if already exists
            if (PerformanceCounterCategory.Exists(ExceptionHandledEvent.InstrumentationCounterCategory))
            {
                PerformanceCounterCategory.Delete(ExceptionHandledEvent.InstrumentationCounterCategory);
            }
            AddPerformanceCountersToInstaller();
            base.Install(stateSaver);
        }

        /// <summary/>
        /// <param name="savedState"/>
        /// <exclude/>
        public override void Uninstall(IDictionary savedState)
        {
            if (PerformanceCounterCategory.Exists(ExceptionHandledEvent.InstrumentationCounterCategory))
            {
                AddPerformanceCountersToInstaller();
            }
            base.Uninstall(savedState);
        }

        private void AddPerformanceCountersToInstaller()
        {
            // The categoryInstaller is for one performance category only.
            // We have only one category per assembly
            PerformanceCounterInstaller categoryInstaller = new PerformanceCounterInstaller();
            categoryInstaller.CategoryName = ExceptionHandledEvent.InstrumentationCounterCategory;
            categoryInstaller.CategoryHelp = SR.InstrumentationCounterCategoryHelp;
            Installers.Add(categoryInstaller);

            // adding all the counters in the category.
            foreach (CounterCreationData counterCreation in ExceptionHandlingEvent.counters)
            {
                categoryInstaller.Counters.Add(counterCreation);
            }
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