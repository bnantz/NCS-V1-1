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
    /// Represents a strongly-typed collection of 
    /// <see cref="RolesProviderData"/> objects.
    /// </summary>
    [Serializable]
    public class RolesProviderDataCollection : ProviderDataCollection
    {
        /// <summary>
        /// Returns a <see cref="RolesProviderData"/> by its index.
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="RolesProviderData"/> to get or set.</para>
        /// </param>
        public RolesProviderData this[int index]
        {
            get { return (RolesProviderData)GetProvider(index); }
            set { SetProvider(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="RolesProviderData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="RolesProviderData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public RolesProviderData this[string name]
        {
            get { return (RolesProviderData)GetProvider(name); }
            set { SetProvider(name, value); }
        }

        /// <summary>
        /// <para>Adds an <see cref="RolesProviderData"/> into the collection.</para>
        /// </summary>
        /// <param name="providerData">
        /// <para>The <see cref="RolesProviderData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="ProviderData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="providerData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><seealso cref="ProviderData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(RolesProviderData providerData)
        {
            AddProvider(providerData);
        }
    }
}