//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// <para>Represents a collection of <see cref="DatabaseProviderData"/> objects in configuration.</para>
    /// </summary>       
    [Serializable]
    public class DatabaseProviderDataCollection : ProviderDataCollection
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DatabaseProviderDataCollection"/> class.</para>
        /// </summary>
        public DatabaseProviderDataCollection()
        {
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="DatabaseProviderData"/> at the specified <paramref name="index"/>.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="DatabaseProviderData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="index"/>. If the specified <paramref name="index"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="index"/>.</para>
        /// </value>
        public DatabaseProviderData this[int index]
        {
            get { return (DatabaseProviderData)GetProvider(index); }
            set { SetProvider(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="DatabaseProviderData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="DatabaseProviderData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public DatabaseProviderData this[string name]
        {
            get { return (DatabaseProviderData)base.GetProvider(name); }
            set { base.SetProvider(name, value); }
        }

        /// <summary>
        /// <para>Adds an <see cref="DatabaseProviderData"/> into the collection.</para>
        /// </summary>
        /// <param name="databaseProviderData">
        /// <para>The <see cref="DatabaseProviderData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="ProviderData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="databaseProviderData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para><seealso cref="ProviderData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(DatabaseProviderData databaseProviderData)
        {
            base.AddProvider(databaseProviderData);
        }

        /// <summary>
        /// <para>Copies the entire <see cref="DatabaseProviderDataCollection"/> to a compatible one-dimensional <see cref="Array"/>, starting at the specified index of the target array.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="DatabaseProviderDataCollection"/> array that is the destination of the elements copied from <see cref="DatabaseProviderDataCollection"/>. The <see cref="DatabaseProviderData"/> array must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        public void CopyTo(DatabaseProviderData[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}