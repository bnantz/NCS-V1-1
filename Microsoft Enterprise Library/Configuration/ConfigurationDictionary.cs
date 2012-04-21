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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a strongly-typed dictionary of section name and configuration data entries.</para>
    /// </summary>
    public class ConfigurationDictionary : DictionaryBase
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationDictionary"/> class.</para>
        /// </summary>
        public ConfigurationDictionary() : base()
        {
        }

        /// <summary>
        /// <para>Gets an <see cref="ICollection"/> containing the configuraiton section names.</para>
        /// </summary>
        /// <value>
        /// <para>An <see cref="ICollection"/> containing the configuraiton section names.</para>
        /// </value>
        public ICollection Keys
        {
            get { return base.Dictionary.Keys; }
        }

        /// <summary>
        /// <para>Gets an <see cref="ICollection"/> containing the configuration section values.</para>
        /// </summary>
        /// <value>
        /// <para>An <see cref="ICollection"/> containing the configuration section values.</para>
        /// </value>
        public ICollection Values
        {
            get { return base.Dictionary.Values; }
        }

        /// <summary>
        /// <para>Gets or sets the configuration data for the specified configuration section name.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the configuration section.</para>
        /// </param>
        /// <value>
        /// <para>The configuration data for the specified configuration section name.</para>
        /// </value>
        public object this[string sectionName]
        {
            get { return base.Dictionary[sectionName]; }
            set { base.Dictionary[sectionName] = value; }
        }

        /// <summary>
        /// <para>Adds the specified configuration data for the specified configuration section name.</para>
        /// </summary>
        /// <param name="sectionName"><para>The name of a configuration section.</para></param>
        /// <param name="configurationData"><para>A configuration data object.</para></param>
        public void Add(string sectionName, object configurationData)
        {
            base.Dictionary.Add(sectionName, configurationData);
        }

        /// <summary>
        /// <para>determines whether the section already exists..</para>
        /// </summary>
        /// <param name="sectionName"><para>The configuration section to locate.</para></param>
        /// <returns><para><see langword="true"/> if the element with the key exists; otherwise, <see langword="false"/>.</para></returns>
        public bool Contains(string sectionName)
        {
            return base.Dictionary.Contains(sectionName);
        }

        /// <summary>
        /// <para>Removes the configuration data from the dictionary forthe specified configuration section.</para>
        /// </summary>
        /// <param name="sectionName"><para>The configuration section name to remove.</para></param>
        public void Remove(string sectionName)
        {
            base.Dictionary.Remove(sectionName);
        }
    }
}