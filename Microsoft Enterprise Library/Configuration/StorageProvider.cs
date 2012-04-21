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
    /// <para>Represents a provider that reads and writes configuration data to storage.</para>
    /// </summary>
	public abstract class StorageProvider : ConfigurationProvider, IStorageProviderReader
	{
	    private string currentSectionName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="StorageProvider"/> class.</para>
        /// </summary>
	    protected StorageProvider()
		{
            currentSectionName = string.Empty;
		}

        /// <summary>
        /// <para>Gets the name of the configuration section.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the configuration section.</para>
        /// </value>
	    public string CurrentSectionName
	    {
	        get { return currentSectionName; }
	        set { currentSectionName = value; }
	    }

        /// <summary>
        /// <para>When overriden by a derived clas, reads the configuration data from storage.</para>
        /// </summary>
        /// <returns>
        /// <para>The configuration data.</para>
        /// </returns>
	    public abstract object Read();
        
        /// <summary>
        /// <para>When overriden by a derived class, creates an <see cref="IConfigurationChangeWatcher"/> for the storage.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="IConfigurationChangeWatcher"/> for the storage.</para>
        /// </returns>
	    public abstract IConfigurationChangeWatcher CreateConfigurationChangeWatcher();
	}
}
