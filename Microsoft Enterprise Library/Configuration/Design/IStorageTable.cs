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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides a list of documents in the current <see cref="IUIHierarchy"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>The table stores all the files in lower case and performs a lower case on all operations.</para>
    /// </remarks>
    public interface IStorageTable
    {
        /// <summary>
        /// <para>When implmented by a class, gets or sets the meta configuration file.</para>
        /// </summary>
        /// <value>
        /// <para>The meta configuration file.</para>
        /// </value>
        /// <remarks>
        /// <para>This file is used to store the meta data for your configuraiton describing the information around the configuration.</para>
        /// </remarks>
        string MetaConfigurationFile { get; set; }

        /// <summary>
        /// <para>When implemented by a class, gets the <see cref="StorageCreationCommand"/> objects in the table.</para>
        /// </summary>
        /// <returns>A <see cref="StorageCreationCommandDictionary"/> object containg the <see cref="StorageCreationCommand"/> objects indexed by name.</returns>
        StorageCreationCommandDictionary GetStorageCreationCommands();

        /// <summary>
        /// <para>When implmented by a class, adds a <see cref="StorageCreationCommand"/> to the table.</para>
        /// </summary>
        /// <param name="storageCreationCommand">
        /// <para>The <see cref="StorageCreationCommand"/> to add.</para>
        /// </param>
        void Add(StorageCreationCommand storageCreationCommand);

        /// <summary>
        /// <para>When implmented by a class, determines if a <see cref="StorageCreationCommand"/> exits table.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="StorageCreationCommand"/>.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the <see cref="StorageCreationCommand"/> exists; otherwise, <see langword="false"/>.</para>
        /// </returns>
        bool Contains(string name);

        /// <summary>
        /// <para>When implmented by a class, removes a <see cref="StorageCreationCommand"/> from the document</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="StorageCreationCommand"/>.</para>
        /// </param>
        /// <remarks>
        /// <para>If the file is not rooted it will be rooted to the meta configuration files root.</para>
        /// </remarks>
        void Remove(string name);
    }
}