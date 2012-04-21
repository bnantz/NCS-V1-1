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

using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>
    [RunInstaller(true)]
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class ProjectInstallerBase : DefaultManagementProjectInstaller
    {
        private string counterCategory;
        private string counterCategoryHelp;
        private CounterCreationData[] counterData;

        /// <summary/>
        /// <param name="eventSource"/>
        /// <param name="counterCategory"/>
        /// <param name="counterCategoryHelp"/>
        /// <param name="counterData"/>
        /// <exclude/>
        protected ProjectInstallerBase(string eventSource, string counterCategory,
                                       string counterCategoryHelp, CounterCreationData[] counterData)
        {
            this.counterCategory = counterCategory;
            this.counterCategoryHelp = counterCategoryHelp;
            this.counterData = counterData;

            EventLogInstaller eventlogInstaller = new EventLogInstaller();
            eventlogInstaller.Log = "Application";
            eventlogInstaller.Source = eventSource;
            Installers.Add(eventlogInstaller);
        }

        /// <summary/>
        /// <param name="stateSaver"/>
        /// <exclude/>
        public override void Install(IDictionary stateSaver)
        {
            // Delete the counter category if already exists
            if (PerformanceCounterCategory.Exists(this.counterCategory))
            {
                PerformanceCounterCategory.Delete(this.counterCategory);
            }
            AddPerformanceCountersToInstaller();
            base.Install(stateSaver);
        }

        /// <summary/>
        /// <param name="savedState"/>
        /// <exclude/>
        public override void Uninstall(IDictionary savedState)
        {
            if (PerformanceCounterCategory.Exists(this.counterCategory))
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
            categoryInstaller.CategoryName = this.counterCategory;
            categoryInstaller.CategoryHelp = this.counterCategoryHelp;
            Installers.Add(categoryInstaller);

            // adding all the counters in the category.
            foreach (CounterCreationData counterCreation in this.counterData)
            {
                categoryInstaller.Counters.Add(counterCreation);
            }
        }
    }
}