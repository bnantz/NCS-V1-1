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
    /// <para>Represents a collection of <see cref="ConfigurationError"/> objects.</para>
    /// </summary>    
    public sealed class ConfigurationErrorCollection : CollectionBase
    {
        /// <summary>
        /// <para>Initializes a new instance of the  <see cref="ConfigurationErrorCollection"/> class.</para>
        /// </summary>
        public ConfigurationErrorCollection() : base()
        {
        }

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationError"/> at the given index.</para>
        /// </summary>
        /// <param name="index"><para>The zero-based index of the parameter to retrieve.</para></param>
        /// <value><para>The <see cref="ConfigurationError"/> at the specified index.</para></value>
        public ConfigurationError this[int index]
        {
            get { return (ConfigurationError)InnerList[index]; }
        }

        /// <summary>
        /// <para>Initializes a new <see cref="ConfigurationError"/> object with the specified <see cref="ConfigurationNode"/>  and error message and adds it to the collection.</para>
        /// </summary>
        /// <param name="node"><para>The <see cref="ConfigurationNode"/> object.</para></param>
        /// <param name="message"><para>The message for the error.</para></param>
        public void Add(ConfigurationNode node, string message)
        {
            Add(new ConfigurationError(node, message));
        }

        /// <summary>
        /// <para>Add a <see cref="ConfigurationError"/> to the collection.</para>
        /// </summary>
        /// <param name="configurationError"><para>The <see cref="ConfigurationError"/> to add to the collection.</para></param>
        public void Add(ConfigurationError configurationError)
        {
            InnerList.Add(configurationError);
        }

        /// <summary>
        /// <para>Inserts an element into the <see cref="ConfigurationErrorCollection"/> at the specified index.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index at which value should be inserted.</para>
        /// </param>
        /// <param name="configurationError">
        /// <para>The Object to insert.</para>
        /// </param>
        public void Insert(int index, ConfigurationError configurationError)
        {
            InnerList.Insert(index, configurationError);
        }

        /// <summary>
        /// <para>Searches for the specified <see cref="ConfigurationError"/> and returns the zero-based index of the first occurrence within the entire <see cref="ConfigurationErrorCollection"/>.</para>
        /// </summary>
        /// <param name="configurationError">
        /// <para>The <see cref="ConfigurationError"/> to locate in the <see cref="ConfigurationErrorCollection"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The zero-based index of the first occurrence of value within the entire <see cref="ConfigurationErrorCollection"/>, if found; otherwise, -1.</para>
        /// </returns>
        public int IndexOf(ConfigurationError configurationError)
        {
            return InnerList.IndexOf(configurationError);
        }

        /// <summary>
        /// <para>Determines whether the <see cref="ConfigurationErrorCollection"/> contains a specific element.</para>
        /// </summary>
        /// <param name="configurationError">
        /// <para>The <see cref="ConfigurationError"/> to locate in the <see cref="ConfigurationErrorCollection"/>.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the CollectionBase contains the specified value; otherwise, <see langword="false"/>.</para>
        /// </returns>
        public bool Contains(ConfigurationError configurationError)
        {
            return InnerList.Contains(configurationError);
        }

        /// <summary>
        /// <para>Removes the first occurrence of a specific object from the <see cref="ConfigurationErrorCollection"/>.</para>
        /// </summary>
        /// <param name="configurationError">
        /// <para>The <see cref="ConfigurationError"/> to remove from the <see cref="ConfigurationErrorCollection"/>.</para>
        /// </param>
        public void Remove(ConfigurationError configurationError)
        {
            InnerList.Remove(configurationError);
        }

        /// <summary>
        /// <para>Copies the entire <see cref="ConfigurationErrorCollection"/> to a compatible one-dimensional Array, starting at the specified index of the target array.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="ConfigurationError"/> array that is the destination of the elements copied from <see cref="ConfigurationErrorCollection"/>. The <see cref="ConfigurationError"/> array must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        public void CopyTo(ConfigurationError[] array, int index)
        {
            InnerList.CopyTo(array, index);
        }

        /// <summary>
        /// <para>Adds the <see cref="ConfigurationError"/>s of the <see cref="ConfigurationErrorCollection"/> to the end of the collection.</para>
        /// </summary>
        /// <param name="errors">
        /// <para>A <see cref="ConfigurationErrorCollection"/> object.</para>
        /// </param>
        public void AddRange(ConfigurationErrorCollection errors)
        {
            InnerList.AddRange(errors);
        }
    }
}