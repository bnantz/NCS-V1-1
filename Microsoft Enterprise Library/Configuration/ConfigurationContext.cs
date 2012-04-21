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
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// Represents a read-only set of configuration data
    /// indexed by configuration section name.
    /// </summary>
    public class ConfigurationContext : IDisposable
    {
        internal readonly IDisposableWrapper disposableWrapper;

        /// <summary>
        /// <para>Occurs before configuration is changed.</para>
        /// </summary>
        public event ConfigurationChangingEventHandler ConfigurationChanging;

        /// <summary>
        /// <para>Occurs after configuration is changed.</para>
        /// </summary>
        public event ConfigurationChangedEventHandler ConfigurationChanged;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationContext"/> class with the specified <see cref="ConfigurationManager"/></para>
        /// </summary>
        /// <param name="disposableWrapper">
        /// <para>The <see cref="IDisposableWrapper"/> used to create the context.</para>
        /// </param>
        internal ConfigurationContext(IDisposableWrapper disposableWrapper)
        {
            ArgumentValidation.CheckForNullReference(disposableWrapper, "disposableWrapper");

            this.disposableWrapper = disposableWrapper;
            this.disposableWrapper.ConfigurationBuilder.ConfigurationChanged += new ConfigurationChangedEventHandler(OneConfigurationBuilderConfigurationChanged);
            this.disposableWrapper.ConfigurationBuilder.ConfigurationChanging += new ConfigurationChangingEventHandler(OnConfigurationBuilderConfigurationChanging);
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationContext"/> class.</para>
        /// </summary>
        public ConfigurationContext() : this(new DisposingWrapper(new ConfigurationBuilder()))
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationContext"/> class from a configuration file.</para>
        /// </summary>
        /// <param name="configurationFile">
        /// <para>The configuration file containing the meta data.</para>
        /// </param>
        public ConfigurationContext(string configurationFile) : this(new DisposingWrapper(new ConfigurationBuilder(configurationFile)))
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationContext"/> class with the specified <see cref="ConfigurationDictionary"/>.</para>
        /// </summary>
        /// <param name="dictionary">
        /// <para>A <see cref="ConfigurationDictionary"/> to intialize the context.</para>
        /// </param>
        public ConfigurationContext(ConfigurationDictionary dictionary)
        {
            this.disposableWrapper = new DisposingWrapper(new ConfigurationBuilder(dictionary));
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.disposableWrapper.ConfigurationBuilder.ConfigurationChanged -= new ConfigurationChangedEventHandler(OneConfigurationBuilderConfigurationChanged);
                this.disposableWrapper.ConfigurationBuilder.ConfigurationChanging -= new ConfigurationChangingEventHandler(OnConfigurationBuilderConfigurationChanging);
                disposableWrapper.Dispose();
            }
        }

        /// <summary>
        /// <para>Gets the configuration file used by the current context where the meta data is stored.</para>
        /// </summary>
        /// <value>
        /// <para>The configuration file used by the current context where the meta data is stored.</para>
        /// </value>
        public string ConfigurationFile
        {
            get { return disposableWrapper.ConfigurationBuilder.currentConfigFileName; }
        }

        /// <summary>
        /// <para>
        /// Gets the configuration settings for a configuration section.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The configuration section to read.</para>
        /// </param>
        /// <returns>
        /// <para>The configuration settings for <paramref name="sectionName"/>.</para>
        /// </returns>
        public object GetConfiguration(string sectionName)
        {
            return disposableWrapper.ConfigurationBuilder.ReadConfiguration(sectionName);
        }

        /// <summary>
        /// <para>
        /// Gets the meta configuration for the configuration manager.
        /// </para>
        /// </summary>
        /// <returns>
        /// <para>
        /// The meta configuration for the configuration manager or null if no entries exists.
        /// </para>
        /// </returns>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured while reading the meta configuration.</para>
        /// </exception>
        public ConfigurationSettings GetMetaConfiguration()
        {
            return disposableWrapper.ConfigurationBuilder.ConfigurationSettings;
        }


        /// <summary>
        /// <para>
        /// Gets the meta configuration for the configuration manager.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the configuration section to retrieve.    </para>
        /// </param>
        /// <returns>
        /// <para>
        /// The meta configuration for the configuration manager or null if no entries exists.
        /// </para>
        /// </returns>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured while reading the meta configuration.</para>
        /// </exception>
        public ConfigurationSectionData GetMetaConfiguration(string sectionName)
        {
            return disposableWrapper.ConfigurationBuilder.ReadMetaConfiguration(sectionName);
        }

        /// <summary>
        /// <para>
        /// Write the meta configuration for the configuration manager to the configuration file.
        /// </para>
        /// </summary>
        /// <param name="configurationSettings">
        /// The meta configuration to write to configuration.
        /// </param>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured while reading the configuration to save the data.</para>
        /// </exception>
        public void WriteMetaConfiguration(ConfigurationSettings configurationSettings)
        {
            disposableWrapper.ConfigurationBuilder.WriteMetaConfiguration(configurationSettings);
        }

        /// <summary>
        /// <para>
        /// Write the meta configuration for the configuration manager to the configuration file.
        /// </para>
        /// </summary>
        /// <param name="configurationSectionData">
        /// The meta configuration to write to configuration.
        /// </param>
        /// <exception cref="ConfigurationException">
        /// <para>An error occured while reading the configuration to save the data.</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para>The configuration section already exists.</para>
        /// </exception>
        public void WriteMetaConfiguration(ConfigurationSectionData configurationSectionData)
        {
            disposableWrapper.ConfigurationBuilder.WriteMetaConfig(configurationSectionData);
        }

        /// <summary>
        /// <para>
        /// Write configuration for a section to storage.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section for the configuration data.</para>
        /// </param>
        /// <param name="configValue">
        /// <para>The configuration value to store.</para>
        /// </param>
        /// <exception cref="ConfigurationException">
        /// <para><paramref name="sectionName"/> is not valid section for this configuration.</para>
        /// <para>- or -</para>
        /// <para>The section data is read only.</para>
        /// <para>- or -</para>
        /// <para>An error occured while reading the configuration to save the data.</para>
        /// </exception>
        public void WriteConfiguration(string sectionName, object configValue)
        {
            disposableWrapper.ConfigurationBuilder.WriteConfiguration(sectionName, configValue);
        }

        /// <summary>
        /// <para>Removes all sections from the internal cache.</para>
        /// </summary>
        public void ClearSectionCache()
        {
            disposableWrapper.ConfigurationBuilder.ClearSectionCache();
        }

        /// <summary>
        /// <para>Removes a section from the internal cache.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The section name to remove.</para>
        /// </param>
        public void ClearSectionCache(string sectionName)
        {
            disposableWrapper.ConfigurationBuilder.ClearSectionCache(sectionName);
        }

        /// <summary>
        /// <para>Determines if a section name is valid in the current configuration file.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section to validate.</para>
        /// </param>
        /// <returns><see langword="true"/> if the section name is valid; otherwise, <see langword="false"/></returns>
        public bool IsValidSection(string sectionName)
        {
            return disposableWrapper.ConfigurationBuilder.IsValidSection(sectionName);
        }

        /// <summary>
        /// <para>
        /// Gets a value indicating whether a section in configuration is read-only.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>
        /// The section in the configuration.
        /// </para>
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the configuration section is read-only; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> can not be <see langword="null"/>.</para>
        /// </exception>		
        public bool IsReadOnly(string sectionName)
        {
            return disposableWrapper.ConfigurationBuilder.IsReadOnly(sectionName);
        }

        private void OneConfigurationBuilderConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, e);
            }
        }

        private void OnConfigurationBuilderConfigurationChanging(object sender, ConfigurationChangingEventArgs e)
        {
            if (ConfigurationChanging != null)
            {
                ConfigurationChanging(this, e);
            }
        }
    }
}