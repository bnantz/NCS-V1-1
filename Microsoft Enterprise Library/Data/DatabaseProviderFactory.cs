//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// <para>Represents a factory for creating named instances <see cref="Database"/> objects.</para>
    /// </summary>
    public class DatabaseProviderFactory : ProviderFactory
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DatabaseProviderFactory"/> class.</para>
        /// </summary>
        public DatabaseProviderFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="DatabaseProviderFactory"/> class with the specified <see cref="ConfigurationContext"/>.
        /// </para>
        /// </summary>
        /// <param name="configurationContext">
        /// <para>Configuration context to use when creating factory</para>
        /// </param>
        public DatabaseProviderFactory(ConfigurationContext configurationContext) : base(SR.FactoryName, configurationContext, typeof(Database))
        {
        }

        /// <summary>
        /// <para>Creates the <see cref="Database"/> object from the configuration data associated with the specified name.</para>
        /// </summary>
        /// <param name="instanceName">Instance name as defined in configuration</param>
        /// <returns><para>A <see cref="Database"/> object.</para></returns>
        public Database CreateDatabase(string instanceName)
        {
            return (Database)base.CreateInstance(instanceName);
        }

        /// <summary>
        /// <para>Creates the <see cref="Database"/> object from the configuration data associated with the default database instance.</para>
        /// </summary>
        /// <returns><para>A <see cref="Database"/> object.</para></returns>
        public Database CreateDefaultDatabase()
        {
            return (Database)CreateDefaultInstance();
        }

        /// <summary>
        /// <para>Gets the default database instance type.</para>
        /// </summary>
        /// <returns>
        /// <para>The default database instance type.</para>
        /// </returns>
        protected override string GetDefaultInstanceName()
        {
        	DatabaseConfigurationView view = (DatabaseConfigurationView)CreateConfigurationView();
            return view.GetDefaultInstanceName();
        }

        /// <summary>
        /// <para>Publish an instrumentation event that indicates there was an error while attempting to create a provider.</para>
        /// </summary>
        /// <param name="name"><para>The name of the configuration object.</para></param>
        /// <param name="e"><para>The <see cref="Exception"/> to publish.</para></param>
        protected override void PublishFailureEvent(string name, Exception e)
        {
            string errorMsg = SR.ExceptionMessageCreateServiceFailure(name);
            DataServiceFailureEvent.Fire(errorMsg, e);
        }

        /// <summary>
        /// <para>Creates the <see cref="DatabaseConfigurationView"/> object to navigate the <see cref="DatabaseSettings"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>A <see cref="DatabaseConfigurationView"/> object.</para>
        /// </returns>
        protected override ConfigurationView CreateConfigurationView()
        {
            return new DatabaseConfigurationView(ConfigurationContext);
        }

        /// <summary>
        /// <para>Gets the <see cref="Type"/> of <see cref="Database"/> to create based on the name of the <see cref="InstanceData"/>.</para>
        /// </summary>
        /// <param name="instanceName">
        /// <para>The name of the <see cref="InstanceData"/> to get the <see cref="Type"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="Type"/> of the <see cref="Database"/> to create.</para>
        /// </returns>
        protected override Type GetConfigurationType(string instanceName)
        {
        	DatabaseConfigurationView view = (DatabaseConfigurationView)CreateConfigurationView();
            DatabaseProviderData databaseTypeData = view.GetDatabaseProviderData(instanceName);
            return GetType(databaseTypeData.TypeName);
        }
    }
}