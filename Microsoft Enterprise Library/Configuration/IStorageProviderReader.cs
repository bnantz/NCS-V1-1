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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>
    /// Represents a storage provider reader for configuration data.
    /// </para>
    /// </summary>
    public interface IStorageProviderReader : IConfigurationProvider, IConfigurationChangeWatcherFactory
    {
        /// <summary>
        /// <para>When implemented by a class, gets the name of the configuration section.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the configuration section.</para>
        /// </value>
        string CurrentSectionName { get; set; }

        /// <summary>
        /// <para>When implemented by a class, reads the configuration from storage</para>
        /// </summary>        
        /// <returns>
        /// <para>The configuration data for the sectionName.</para>
        /// </returns>
        object Read();
    }
}