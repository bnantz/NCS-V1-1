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

using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a strongly-typed dictionary of section name and configuration data entries.</para>
    /// </summary>
    public class StorageCreationCommandDictionary : DictionaryBase
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="StorageCreationCommandDictionary"/> class.</para>
        /// </summary>
        public StorageCreationCommandDictionary() : base()
        {
        }

        /// <summary>
        /// <para>Gets an <see cref="ICollection"/> containing the names of the <see cref="StorageCreationCommand"/> objects.</para>
        /// </summary>
        /// <value>
        /// <para>An <see cref="ICollection"/> containing the names of the <see cref="StorageCreationCommand"/> objects.</para>
        /// </value>
        public ICollection Keys
        {
            get { return base.Dictionary.Keys; }
        }

        /// <summary>
        /// <para>Gets an <see cref="ICollection"/> containing the <see cref="StorageCreationCommand"/> objects.</para>
        /// </summary>
        /// <value>
        /// <para>An <see cref="ICollection"/> containing the <see cref="StorageCreationCommand"/> objects.</para>
        /// </value>
        public ICollection Values
        {
            get { return base.Dictionary.Values; }
        }

        /// <summary>
        /// <para>Gets or sets <see cref="StorageCreationCommand"/> for the specified name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="StorageCreationCommand"/>.</para>
        /// </param>
        /// <value>
        /// <para>The <see cref="StorageCreationCommand"/> for the specified name.</para>
        /// </value>
        public StorageCreationCommand this[string name]
        {
            get { return (StorageCreationCommand)base.Dictionary[name]; }
            set { base.Dictionary[name] = value; }
        }

        /// <summary>
        /// <para>Adds the specified configuration data for the specified configuration section name.</para>
        /// </summary>
        /// <param name="name"><para>The name of a <see cref="StorageCreationCommand"/>.</para></param>
        /// <param name="storageCreationCommand"><para>A <see cref="StorageCreationCommand"/> object.</para></param>
        public void Add(string name, StorageCreationCommand storageCreationCommand)
        {
            base.Dictionary.Add(name, storageCreationCommand);
        }

        /// <summary>
        /// <para>Determines whether the <see cref="StorageCreationCommand"/> already exists..</para>
        /// </summary>
        /// <param name="name"><para>The <see cref="StorageCreationCommand"/> to locate.</para></param>
        /// <returns><para><see langword="true"/> if the <see cref="StorageCreationCommand"/> with the name exists; otherwise, <see langword="false"/>.</para></returns>
        public bool Contains(string name)
        {
            return base.Dictionary.Contains(name);
        }

        /// <summary>
        /// <para>Removes the <see cref="StorageCreationCommand"/> from the dictionary forthe specified configuration section.</para>
        /// </summary>
        /// <param name="name"><para>The <see cref="StorageCreationCommand"/> name to remove.</para></param>
        public void Remove(string name)
        {
            base.Dictionary.Remove(name);
        }
    }
}
