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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// <para>Represents a view for navigating the <see cref="DatabaseSettings"/> configuration data.</para>
    /// </summary>
    public class DatabaseConfigurationView : ConfigurationView
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DatabaseConfigurationView"/> class with a <see cref="ConfigurationContext"/> object.</para>
        /// </summary>
        /// <param name="configurationContext">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        public DatabaseConfigurationView(ConfigurationContext configurationContext) : base(configurationContext)
        {
        }

        /// <summary>
        /// <para>Gets the <see cref="DatabaseSettings"/> configuration data.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="DatabaseSettings"/> configuration data.</para>
        /// </returns>
        public virtual DatabaseSettings GetDatabaseSettings()
        {
            return (DatabaseSettings)ConfigurationContext.GetConfiguration(DatabaseSettings.SectionName);
        }

        /// <summary>
        /// <para>Gets the name of the default <see cref="InstanceData"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The name of the default <see cref="InstanceData"/>.</para>
        /// </returns>
        public virtual string GetDefaultInstanceName()
        {
            DatabaseSettings settings = GetDatabaseSettings();
            string instanceName = settings.DefaultInstance;
            if (instanceName == null)
            {
                throw new ConfigurationException(SR.ExceptionMessageNoDefault);
            }
            return instanceName;
        }

        /// <summary>
        /// <para>Gets the <see cref="DatabaseProviderData"/> for the named <see cref="InstanceData"/>.</para>
        /// </summary>
        /// <param name="instanceName">
        /// <para>The name of the <see cref="InstanceData"/> to get the data.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="DatabaseProviderData"/> for the named <see cref="InstanceData"/>.</para>
        /// </returns>
        public virtual DatabaseProviderData GetDatabaseProviderData(string instanceName)
        {
            DatabaseSettings settings = GetDatabaseSettings();
            InstanceData instance = settings.Instances[instanceName];
            if (null == instance)
            {
                throw new ConfigurationException(SR.ExceptionNoInstance(instanceName));
            }

            ConnectionStringData connectionString = settings.ConnectionStrings[instance.ConnectionString];
            if (null == connectionString)
            {
                throw new ConfigurationException(SR.ExceptionNoConnectionStringType(instance.ConnectionString));
            }

            DatabaseTypeData databaseType = settings.DatabaseTypes[instance.DatabaseTypeName];
            if (null == databaseType)
            {
                throw new ConfigurationException(SR.ExceptionNoDatabaesType(instance.DatabaseTypeName));
            }

            return new DatabaseProviderData(instance, databaseType, connectionString);
        }
        
    }
}