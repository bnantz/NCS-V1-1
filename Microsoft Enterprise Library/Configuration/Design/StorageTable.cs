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

using System.Globalization;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Provides the list of StorageCommands to execute when saving
    /// </devdoc>
    internal class StorageTable : IStorageTable
    {
        internal StorageCreationCommandDictionary storageCommands;
        private string metaConfigurationFile;
        
        public StorageTable()
        {
            metaConfigurationFile = string.Empty;
            storageCommands = new StorageCreationCommandDictionary();
        }

        /// <devdoc>
        /// Gets or sets the meta configuration file.
        /// </devdoc>
        public string MetaConfigurationFile
        {
            get { return metaConfigurationFile; }
            set
            {
                if (value == null || value.Length == 0) return;

                if (!Path.IsPathRooted(value))
                {
                    value = Path.GetFullPath(value);
                }
                this.metaConfigurationFile = value.ToLower(CultureInfo.InvariantCulture);
            }
        }

        /// <devdoc>
        /// Gets the StorageCreationCommand objects in the table.
        /// </devdoc>
        public StorageCreationCommandDictionary GetStorageCreationCommands()
        {
            return storageCommands;
        }

        /// <devdoc>
        /// Adds a StorageCreationCommand to the table.
        /// </devdoc>
        public void Add(StorageCreationCommand storageCreationCommand)
        {
           storageCommands.Add(storageCreationCommand.Name.ToLower(CultureInfo.InvariantCulture),  storageCreationCommand);
        }

        /// <devdoc>
        /// Determines if a StorageCreationCommand exits table.
        /// </devdoc>
        public bool Contains(string name)
        {
            return storageCommands.Contains(name.ToLower(CultureInfo.InvariantCulture));
        }

        /// <devdoc>
        /// Removes a StorageCreationCommand from the table.
        /// </devdoc>
        public void Remove(string name)
        {
            storageCommands.Remove(name.ToLower(CultureInfo.InvariantCulture));
        }
    }
}