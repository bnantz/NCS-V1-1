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
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a strongly-typed collection of <see cref="ProviderData"/> objects.</para>
    /// </summary>
    [Serializable]
    public abstract class ProviderDataCollection : DataCollection
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ProviderDataCollection"/> class.</para>
        /// </summary>
        protected ProviderDataCollection()
        {
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="ProviderData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="ProviderData"/> to get or set.</para>
        /// </param>
        /// <returns>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        protected internal ProviderData GetProvider(string name)
        {
            ArgumentValidation.CheckForNullReference(name, "name");

            return (ProviderData)BaseGet(name);
        }

        /// <summary>
        /// <para>Gets the value of the entry at the specified index of the <see cref="ProviderDataCollection"/> instance.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index of the value to get.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="ProviderData"/> object that represents the value of the entry at the specified index.</para>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
        /// </exception>
        protected internal ProviderData GetProvider(int index)
        {
            return (ProviderData)BaseGet(index);
        }

        /// <summary>
        /// <para>Sets the value of the first entry with the specified key in the <see cref="ProviderDataCollection"/> instance, if found; otherwise, adds an entry with the specified key and value into the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The string key of the entry to set. The key can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic)</para>.
        /// </param>
        /// <param name="data">
        /// <para>The <see cref="ProviderData"/> object that represents the new value of the entry to set. The value can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para>The collection already contains an entry for <paramref name="name"/>.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only.</para>
        /// </exception>
        protected internal void SetProvider(string name, ProviderData data)
        {
            ArgumentValidation.CheckForNullReference(name, "name");

            BaseSet(name, data);
        }

        /// <summary>
        /// <para>Sets the value of the entry at the specified index of the <see cref="ProviderDataCollection"/> instance.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index of the entry to set.</para>
        /// </param>
        /// <param name="data">
        /// <para>The <see cref="ProviderData"/> object that represents the new value of the entry to set. The value can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
        /// </param>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
        /// </exception>
        protected internal void SetProvider(int index, ProviderData data)
        {
            BaseSet(index, data);
        }

        /// <summary>
        /// <para>Adds an <see cref="ProviderData"/> into the collection.</para>
        /// </summary>
        /// <param name="providerData">
        /// <para>The <see cref="ProviderData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="ProviderData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="providerData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para><seealso cref="ProviderData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        protected internal void AddProvider(ProviderData providerData)
        {
            ArgumentValidation.CheckForNullReference(providerData, "providerData");

            if (providerData.Name == null)
            {
                throw new InvalidOperationException(SR.ExceptionNullProviderDataName);
            }
            BaseAdd(providerData.Name, providerData);
        }

        /// <summary>
        /// <para>Adds an <see cref="ProviderData"/> into the collection.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the key for the <parmref name="providerData"/>. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <param name="providerData">
        /// <para>The <see cref="ProviderData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="ProviderData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="providerData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        protected internal void AddProvider(string name, ProviderData providerData)
        {
            ArgumentValidation.CheckForNullReference(name, "name");
            ArgumentValidation.CheckForNullReference(providerData, "providerData");

            BaseAdd(name, providerData);
        }

        /// <summary>
        /// Adds the items in the specified collection to the current collection.
        /// </summary>
        /// <param name="collection">A <see cref="ProviderDataCollection"/>.</param>
        protected internal void AddProviders(ProviderDataCollection collection)
        {
            foreach (ProviderData data in collection)
            {
                AddProvider(data);
            }
        }
    }
}