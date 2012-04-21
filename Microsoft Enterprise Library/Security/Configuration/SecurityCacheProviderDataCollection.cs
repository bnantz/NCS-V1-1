//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="SecurityCacheProviderData"/> settings.
    /// </summary>
    [Serializable]
    public class SecurityCacheProviderDataCollection : ProviderDataCollection
    {
        /// <summary>
        /// Returns a <see cref="SecurityCacheProviderData"/> by its index.
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="SecurityCacheProviderData"/> to get or set.</para>
        /// </param>
        public SecurityCacheProviderData this[int index]
        {
            get { return (SecurityCacheProviderData)GetProvider(index); }
            set { SetProvider(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="SecurityCacheProviderData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="SecurityCacheProviderData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public SecurityCacheProviderData this[string name]
        {
            get { return (SecurityCacheProviderData)GetProvider(name); }
            set { SetProvider(name, value); }
        }

        /// <summary>
        /// <para>Adds an <see cref="SecurityCacheProviderData"/> into the collection.</para>
        /// </summary>
        /// <param name="providerData">
        /// <para>The <see cref="SecurityCacheProviderData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="ProviderData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="providerData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><seealso cref="ProviderData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(SecurityCacheProviderData providerData)
        {
            AddProvider(providerData);
        }
    }
}