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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Configuration
{
    /// <summary>
    /// <para>Represents a collection of <see cref="StorageProviderData"/> objects in configuration.</para>
    /// </summary>       
    [Serializable]
    public class StorageProviderDataCollection : ProviderDataCollection
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="StorageProviderDataCollection"/> class.</para>
        /// </summary>
        public StorageProviderDataCollection()
        {
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="StorageProviderData"/> at the specified <paramref name="index"/>.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="StorageProviderData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="index"/>. If the specified <paramref name="index"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="index"/>.</para>
        /// </value>
        public StorageProviderData this[int index]
        {
            get { return (StorageProviderData)GetProvider(index); }
            set { SetProvider(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="StorageProviderData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="StorageProviderData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public StorageProviderData this[string name]
        {
            get { return (StorageProviderData)base.GetProvider(name); }
            set { base.SetProvider(name, value); }
        }

        /// <summary>
        /// <para>Adds an <see cref="StorageProviderData"/> into the collection.</para>
        /// </summary>
        /// <param name="storageProviderData">
        /// <para>The <see cref="StorageProviderData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="ProviderData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="storageProviderData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <para><seealso cref="ProviderData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(StorageProviderData storageProviderData)
        {
            base.AddProvider(storageProviderData);
        }

        /// <summary>
        /// <para>Copies the entire <see cref="StorageProviderDataCollection"/> to a compatible one-dimensional <see cref="StorageProviderData"/> array, starting at the specified index of the target array.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="StorageProviderData"/> array that is the destination of the elements copied from <see cref="StorageProviderDataCollection"/>. The <see cref="StorageProviderData"/> array must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        public void CopyTo(StorageProviderData[] array, int index)
        {
            for (IEnumerator e = this.GetEnumerator(); e.MoveNext(); )
            {
                array.SetValue(e.Current, index++);
            }
        }
    }
}