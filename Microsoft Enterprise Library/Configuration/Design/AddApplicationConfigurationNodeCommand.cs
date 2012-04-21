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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a command for adding a new <see cref="ApplicationConfigurationNode"/>.</para>
    /// </summary>
    public class AddApplicationConfigurationNodeCommand : ConfigurationNodeCommand
    {
        private ApplicationData applicationData;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="AddApplicationConfigurationNodeCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public AddApplicationConfigurationNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="AddApplicationConfigurationNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorService">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        public AddApplicationConfigurationNodeCommand(IServiceProvider serviceProvider, bool clearErrorService) : base(serviceProvider, clearErrorService)
        {
            this.applicationData = new ApplicationData();
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="ApplicationData"/> object that will be used to  initialize the <see cref="ApplicationConfigurationNode"/> object.
        /// </para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ApplicationData"/> object that will be used to initialize the <see cref="ApplicationConfigurationNode"/> object.</para>
        /// </value>
        public ApplicationData ApplicationData
        {
            get { return applicationData; }
            set { applicationData = value; }
        }

        /// <summary>
        /// <para>Creates a new <see cref="ApplicationConfigurationNode"/> object and adds it to the solution.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The <see cref="ApplicationConfigurationNode"/> is the root of an <see cref="IUIHierarchy"/> so the <paramref name="node"/> is not used, so passing <see langword="null"/> is expected.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(applicationData);
                IUIHierarchy hierarchy = ConfigurationUIHierarchyFactory.Create(appNode, ServiceProvider);
                UIHierarchyService.AddHierarchy(hierarchy);
                UIService.SetUIDirty(hierarchy);
            }
            finally
            {
                UIService.EndUpdate();
            }
        }
    }
}