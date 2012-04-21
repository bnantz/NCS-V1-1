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
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// <para>Represents a collection of <see cref="ParameterData"/>s for a <see cref="ConnectionString"/> in configuration.</para>
    /// </summary>
    /// <remarks>
    /// <para>This class represents the <c>parameters</c> element in configuration.</para>
    /// </remarks>
    [Serializable]
    public class ParameterDataCollection : DataCollection
    {
        /// <summary>
        /// <para>Gets or sets the <see cref="ParameterData"/> at the specified <paramref name="index"/>.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="ParameterData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="index"/>. If the specified <paramref name="index"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="index"/>.</para>
        /// </value>
        public ParameterData this[int index]
        {
            get { return (ParameterData)BaseGet(index); }
            set { BaseSet(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="ParameterData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="ParameterData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public ParameterData this[string name]
        {
            get
            {
                ArgumentValidation.CheckForNullReference(name, "name");

                return BaseGet(name) as ParameterData;
            }
            set
            {
                ArgumentValidation.CheckForNullReference(name, "name");

                BaseSet(name, value);
            }
        }

        /// <summary>
        /// <para>Adds an <see cref="ParameterData"/> into the collection.</para>
        /// </summary>
        /// <param name="parameterData">
        /// <para>The <see cref="ParameterData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="ParameterData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="parameterData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <para><seealso cref="ParameterData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(ParameterData parameterData)
        {
            ArgumentValidation.CheckForNullReference(parameterData, "parameterData");
            ArgumentValidation.CheckForInvalidNullNameReference(parameterData.Name, typeof(ParameterData).FullName);

            BaseAdd(parameterData.Name, parameterData);
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
        /// <para><paramref name="parameter"/> must be of type <see cref="ParameterData"/>.</para>
        /// </exception>
        public void Add(object parameter)
        {
            Add((ParameterData)parameter);
        }

        /// <summary>
        /// <para>Copies the entire <see cref="ParameterDataCollection"/> to a compatible one-dimensional <see cref="Array"/>, starting at the specified index of the target array.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="ParameterDataCollection"/> array that is the destination of the elements copied from <see cref="ParameterDataCollection"/>. The <see cref="ParameterData"/> array must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        public void CopyTo(ParameterData[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}