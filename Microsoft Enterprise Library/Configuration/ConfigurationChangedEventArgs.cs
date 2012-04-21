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
    /// <para>Represents the method that will handle the <seealso cref="ConfigurationManager.ConfigurationChanged"/> and <see cref="ConfigurationContext.ConfigurationChanged"/> events.</para>
    /// </summary>
    /// <param name="sender">
    /// <para>The source of the event.</para>
    /// </param>
    /// <param name="e">
    /// <para>A <see cref="ConfigurationChangedEventArgs"/> that contains the event data.</para>
    /// </param>
    public delegate void ConfigurationChangedEventHandler(object sender, ConfigurationChangedEventArgs e);

    /// <summary>
    /// <para>Provides data for the <seealso cref="ConfigurationManager.ConfigurationChanged"/> and <see cref="ConfigurationContext.ConfigurationChanged"/> event which occur before configuration is changed and committed to storage.</para>
    /// </summary>
    [Serializable]
    public class ConfigurationChangedEventArgs : EventArgs
    {
        private readonly string configurationFile;
        private readonly string sectionName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationChangingEventArgs"/> class with the configuration file, the section name, the old value, and the new value of the changes.</para>
        /// </summary>
        /// <param name="configurationFile"><para>The configuration file where the change occured.</para></param>
        /// <param name="sectionName"><para>The section name of the changes.</para></param>
        public ConfigurationChangedEventArgs(string configurationFile, string sectionName)
        {
            this.configurationFile = configurationFile;
            this.sectionName = sectionName;
        }

        /// <summary>
        /// <para>Gets the configuration file of the data that is changing.</para>
        /// </summary>
        /// <value>
        /// <para>The configuration file of the data that is changing.</para>
        /// </value>
        public string ConfigurationFile
        {
            get { return configurationFile; }
        }

        /// <summary>
        /// <para>Gets the section name where the changes occurred.</para>
        /// </summary>
        /// <value>
        /// <para>The section name where the changes occurred.</para>
        /// </value>
        public string SectionName
        {
            get { return sectionName; }
        }
    }
}