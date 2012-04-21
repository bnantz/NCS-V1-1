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
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
    /// <summary>
    /// Represents a strongly-typed collection of 
    /// <see cref="AuthorizationRuleData"/> objects.
    /// </summary>
    [Serializable]
    public class AuthorizationRuleDataCollection : DataCollection
    {
        /// <summary>
        /// Gets a <see cref="AuthorizationRuleData"/> by its index.
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="AuthorizationRuleData"/> to get or set.</para>
        /// </param>
        public AuthorizationRuleData this[int index]
        {
            get { return (AuthorizationRuleData)BaseGet(index); }
            set { BaseSet(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="AuthorizationRuleData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="AuthorizationRuleData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public AuthorizationRuleData this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                return (AuthorizationRuleData)BaseGet(name);
            }
            set
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                BaseSet(name, value);
            }
        }

        /// <summary>
        /// <para>Adds an <see cref="AuthorizationRuleData"/> into the collection.</para>
        /// </summary>
        /// <param name="providerData">
        /// <para>The <see cref="AuthorizationRuleData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="AuthorizationRuleData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="providerData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><seealso cref="AuthorizationRuleData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(AuthorizationRuleData providerData)
        {
            if (providerData == null)
            {
                throw new ArgumentNullException("providerData");
            }
            if (providerData.Name == null)
            {
                throw new ArgumentNullException("providerData.Name");
            }
            BaseAdd(providerData.Name, providerData);
        }
    }
}