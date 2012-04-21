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
    /// <para>
    /// This interface specifies the contract that providers created through configuration
    /// must implement to allow them to be properly created and initialized through reflection.
    /// </para>
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// <para>Gets or sets the name of the provider.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the provider.</para>
        /// </value>
        string ConfigurationName { get; set; }

        /// <summary>
        /// <para>Initializes the provider with a <see cref="ConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="ConfigurationView"/> object.</para>
        /// </param>
        void Initialize(ConfigurationView configurationView);
    }
}