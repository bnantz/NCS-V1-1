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
    /// <para>Represents location information for saving and loading an <see cref="ApplicationConfigurationNode"/> object.</para>
    /// </summary>
    [Serializable]
    public class ApplicationData
    {
        private string name;
        private string baseDirectory;
        private string configurationFilePath;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ApplicationData"/> class.</para>
        /// </summary>
        public ApplicationData()
        {
            name = SR.DefaultApplicationName;
            baseDirectory = String.Empty;
            configurationFilePath = String.Empty;
        }

        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="ApplicationData"/> class with the specified name, base directory and configuration file path.
        /// </para>
        /// </summary>
        /// <param name="name"><para>The name for the application.</para></param>
        /// <param name="baseDirectory"><para>A base directory for the application.</para></param>
        /// <param name="configurationFilePath"><para>A path to the configuration file.</para></param>
        public ApplicationData(string name, string baseDirectory, string configurationFilePath)
        {
            this.name = name;
            this.baseDirectory = baseDirectory;
            this.configurationFilePath = configurationFilePath;
        }

        /// <summary>
        /// <para>Gets or sets the name of the application.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the application.</para>
        /// </value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// <para>Gets or sets the base directory of the application.</para>
        /// </summary>
        /// <value>
        /// <para>The base directory of the application.</para>
        /// </value>
        public string BaseDirectory
        {
            get { return baseDirectory; }
            set { baseDirectory = value; }
        }

        /// <summary>
        /// <para>Gets or sets the path to the application configuration file.</para>
        /// </summary>
        /// <value>
        /// <para>The path to the application configuration file.</para>
        /// </value>
        public string ConfigurationFilePath
        {
            get { return configurationFilePath; }
            set { configurationFilePath = value; }
        }

        /// <summary>
        /// <para>
        /// Creates an <see cref="ApplicationData"/> object using the properties of the current <see cref="AppDomain"/>.
        /// </para>
        /// </summary>
        /// <returns><para>An <see cref="ApplicationData"/> object.</para></returns>
        public static ApplicationData FromCurrentAppDomain()
        {
            ApplicationData appData = new ApplicationData();
            AppDomain current = AppDomain.CurrentDomain;
            appData.Name = current.FriendlyName;
            appData.BaseDirectory = current.BaseDirectory;
            appData.ConfigurationFilePath = current.SetupInformation.ConfigurationFile;
            return appData;
        }

    }

}