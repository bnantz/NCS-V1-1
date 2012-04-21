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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents an object that consumes configuration. This class is abstract.</para>
    /// </summary>
    public abstract class ConfigurationProvider : IConfigurationProvider
    {
        private string configurationName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationProvider"/> class.</para>
        /// </summary>
        protected ConfigurationProvider()
        {
        }

        /// <summary>
        /// <para>Gets or sets the name of the provider.</para>
        /// </summary>
        /// <value><para>The name of the provider.</para></value>
        public string ConfigurationName
        {
            get { return configurationName; }
            set { configurationName = value; }
        }

        /// <summary>
        /// <para>When overridden by a class, initializes the provider with a <see cref="ConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="ConfigurationView"/> object.</para>
        /// </param>
        public abstract void Initialize(ConfigurationView configurationView);
    }
}