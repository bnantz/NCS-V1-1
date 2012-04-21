//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="CategoryFilterData"/> settings.
    /// </summary>
    [Serializable]
    public class CategoryFilterDataCollection : DataCollection
    {
        /// <summary>
        /// <para>Gets or sets the <see cref="CategoryFilterData"/> at the specified <paramref name="index"/>.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="CategoryFilterData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="index"/>. If the specified <paramref name="index"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="index"/>.</para>
        /// </value>
        public CategoryFilterData this[int index]
        {
            get { return (CategoryFilterData)BaseGet(index); }
            set { BaseSet(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="CategoryFilterData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="CategoryFilterData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public CategoryFilterData this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                return BaseGet(name) as CategoryFilterData;
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
        /// <para>Adds an <see cref="CategoryFilterData"/> into the collection.</para>
        /// </summary>
        /// <param name="exceptionTypeData">
        /// <para>The <see cref="CategoryFilterData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="CategoryFilterData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="exceptionTypeData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para><seealso cref="CategoryFilterData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(CategoryFilterData exceptionTypeData)
        {
            if (exceptionTypeData == null)
            {
                throw new ArgumentNullException("configurationSection");
            }
            if (exceptionTypeData.Name == null)
            {
                throw new InvalidOperationException(SR.ExceptionCategoryFilterDataName);
            }
            BaseAdd(exceptionTypeData.Name, exceptionTypeData);
        }

        /// <summary>
        /// <para>Adds a value into the collection.</para>
        /// </summary>
        /// <param name="parameter">
        /// <para>The value to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>This method exists to support Xml Serialization.</para>
        /// </remarks>
        /// <exception cref="InvalidCastException">
        /// <para><paramref name="parameter"/> must be of type <see cref="CategoryFilterData"/>.</para>
        /// </exception>
        public void Add(object parameter)
        {
            Add((CategoryFilterData)parameter);
        }
    }
}