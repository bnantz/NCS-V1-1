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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>
    /// Provides a facade to configuration settings from defined storage in a specified configuration section. 
    /// </para>
    /// </summary> 
    public sealed class ConfigurationManager : IDisposable
    {
        private static ConfigurationManager instance;
        private ConfigurationBuilder builder;
        private static object syncObject = new object();

        /// <summary>
        /// <para>Occurs before configuration is changed.</para>
        /// </summary>
        public static event ConfigurationChangingEventHandler ConfigurationChanging;

        /// <summary>
        /// <para>Occurs after configuration is changed.</para>
        /// </summary>
        public static event ConfigurationChangedEventHandler ConfigurationChanged;

        private ConfigurationManager()
        {
            this.builder = new ConfigurationBuilder();
            this.builder.ConfigurationChanging += new ConfigurationChangingEventHandler(OnConfigurationChanging);
            this.builder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);
        }

        /// <summary>
        /// Finalizer for ConfigurationManager
        /// </summary>
        ~ConfigurationManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose method for ConfigurationManager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of unmanaged resources held by this class
        /// </summary>
        /// <param name="disposing">True if called from Dispose() method. False otherwise </param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (builder != null)
                {
                    builder.Dispose();
                    builder = null;
                }
            }
        }

        /// <devdoc>
        /// The singleton instance to use with static methods.
        /// </devdoc>
        internal static ConfigurationManager Current
        {
            get
            {
                lock (syncObject)
                {
                    if (instance == null)
                    {
                        instance = new ConfigurationManager();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// <para>
        /// Returns configuration settings for a user-defined configuration section.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The configuration section to read.</para>
        /// </param>
        /// <returns>
        /// <para>The configuration settings for <paramref name="sectionName"/>.</para>
        /// </returns>
        /// <remarks>
        /// <para>Once a section is read, the data for that section is cached and it will always return the same data.</para>
        /// </remarks>
        public static object GetConfiguration(string sectionName)
        {
            return Current.builder.ReadConfiguration(sectionName);
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
        /// <exception cref="System.Configuration.ConfigurationException">
        /// <para><paramref name="sectionName"/> is not valid section for this configuration.</para>
        /// <para>- or -</para>
        /// <para>The section data is read only.</para>
        /// <para>- or -</para>
        /// <para>An error occured while reading the configuration to save the data.</para>
        /// </exception>
        public static void WriteConfiguration(string sectionName, object configValue)
        {
            Current.builder.WriteConfiguration(sectionName, configValue);
        }

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationContext"/> for the current <see cref="ConfigurationManager"/>.</para>
        /// </summary>
        /// <returns><para>A <see cref="ConfigurationContext"/> object.</para></returns>
        public static ConfigurationContext GetCurrentContext()
        {
            return new ConfigurationContext(new NonDisposingWrapper(Current.builder));
        }

        /// <summary>
        /// <para>Creates a new instance of the <see cref="ConfigurationContext"/> class with the specified <see cref="ConfigurationDictionary"/>.</para>
        /// </summary>
        /// <param name="dictionary"><para>A <see cref="ConfigurationDictionary"/>.</para></param>
        /// <returns>
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </returns>
        public static ConfigurationContext CreateContext(ConfigurationDictionary dictionary)
        {
            return new ConfigurationContext(dictionary);
        }

        /// <summary>
        /// <para>Creates a new instance of a <see cref="ConfigurationContext"/> object.</para>
        /// </summary>
        /// <returns>
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </returns>
        public static ConfigurationContext CreateContext()
        {
            return new ConfigurationContext();
        }

        /// <summary>
        /// <para>Creates a new instance of a <see cref="ConfigurationContext"/> object for the given <paraname ref="configurationFile"/>.</para>
        /// </summary>
        /// <param name="configurationFile">
        /// <para>The configuration file that contains the meta configuration.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </returns>
        public static ConfigurationContext CreateContext(string configurationFile)
        {
            return new ConfigurationContext(configurationFile);
        }

        /// <summary>
        /// <para>Removes a section from the internal cache.</para>
        /// </summary>
        /// <param name="section">
        /// <para>The section name to remove.</para>
        /// </param>
        public static void ClearSingletonSectionCache(string section)
        {
            Current.builder.ClearSectionCache(section);
        }

        /// <summary>
        /// <para>Removes all sections from the internal cache.</para>
        /// </summary>
        public static void ClearSingletonSectionCache()
        {
            Current.builder.ClearSectionCache();
        }

        /// <devdoc>
        /// Raises the ConfigurationChanged event.
        /// </devdoc>
        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            if(ConfigurationChanged != null)
            {
                ConfigurationChanged(sender, args);
            }
        }

        /// <devdoc>
        /// Raises the ConfigurationChanging event.
        /// </devdoc>
        private void OnConfigurationChanging(object sender, ConfigurationChangingEventArgs args)
        {
            if (ConfigurationChanging != null)
            {
                ConfigurationChanging(sender, args);
            }
        }
    }
}