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
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a reaonly collection of <see cref="ConfigurationError"/> objects.</para>
    /// </summary>
    public sealed class ReadOnlyConfigurationErrorCollection : System.Collections.ICollection
    {
        private readonly ConfigurationErrorCollection configurationErrorCollection;

        internal ReadOnlyConfigurationErrorCollection(ConfigurationErrorCollection configurationErrorCollection)
        {
            this.configurationErrorCollection = configurationErrorCollection;
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
            configurationErrorCollection.CopyTo(array, index);
        }

        /// <summary>
        /// <para>copies the elements of the <see cref="ICollection"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index,</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <para></para>Array that is the destination of the elements copied from <see cref="ICollection"/>. The <see cref="Array"/> must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        void ICollection.CopyTo(System.Array array, int index)
        {
            CopyTo((ConfigurationError[])array, index);
        }

        /// <summary>
        /// <para>Gets the number of elements contained in the <see cref="ICollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The number of elements contained in the <see cref="ICollection"/>.</para>
        /// </value>
        public int Count
        {
            get { return configurationErrorCollection.Count; }
        }

        /// <summary>
        /// <para>Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>An object that can be used to synchronize access to the <see cref="ICollection"/>.</para>
        /// </value>
        public object SyncRoot
        {
            get { return ((System.Collections.ICollection)configurationErrorCollection).SyncRoot; }
        }

        /// <summary>
        /// <para>Gets a value indicating whether access to the ICollection is synchronized (thread-safe).</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if access to the ICollection is synchronized (thread-safe); otherwise, <see langword="false"/>.</para>
        /// </value>
        public bool IsSynchronized
        {
            get { return ((System.Collections.ICollection)configurationErrorCollection).IsSynchronized; }
        }


        /// <summary>
        /// <para>Returns an enumerator that can iterate through a collection.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</para>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return configurationErrorCollection.GetEnumerator();
        }
    }
}
