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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// <para>A collection of <see cref="ValidationError"/> objects.</para>
    /// </summary>	
    [Serializable]
    public class ValidationErrorCollection : CollectionBase
    {
        /// <summary>
        /// <para>Initialize a new instance of the new <see cref="ValidationErrorCollection"/>.</para>
        /// </summary>
        public ValidationErrorCollection() : base()
        {
        }

        /// <summary>
        /// <para>Gets the <see cref="ValidationError"/> at the appropriate index.</para>
        /// </summary>
        /// <param name="index"><para>The zero-based index of the parameter to retrieve.</para></param>
        /// <value>
        /// <para>The index to the <see cref="ValidationError"/>.</para>
        /// </value>
        public ValidationError this[int index]
        {
            get { return (ValidationError)InnerList[index]; }
        }

        /// <summary>
        /// <para>Create and append a <see cref="ValidationError"/> to the end of the collection.</para>
        /// </summary>
        /// <param name="invalidItem">
        /// <para>The object that did not validate.</para>
        /// </param>
        /// <param name="propertyName">
        /// <para>The name of the property that is invalid.</para>
        /// </param>
        /// <param name="errorMessage">
        /// <para>The message that describes the error.</para>
        /// </param>
        public void Add(object invalidItem, string propertyName, string errorMessage)
        {
            Add(new ValidationError(invalidItem, propertyName, errorMessage));
        }

        /// <summary>
        /// <para>Appends a <see cref="ValidationError"/> to the end of the collection.</para>
        /// </summary>
        /// <param name="validationError">
        /// <para>The <see cref="ValidationError"/> to add.</para>
        /// </param>
        public void Add(ValidationError validationError)
        {
            InnerList.Add(validationError);
        }

        /// <summary>
        /// <para>Inserts an element into the <see cref="ValidationErrorCollection"/> at the specified index.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index at which value should be inserted.</para>
        /// </param>
        /// <param name="validationError">
        /// <para>The Object to insert.</para>
        /// </param>
        public void Insert(int index, ValidationError validationError)
        {
            InnerList.Insert(index, validationError);
        }

        /// <summary>
        /// <para>Searches for the specified <see cref="ValidationError"/> and returns the zero-based index of the first occurrence within the entire <see cref="ValidationErrorCollection"/>.</para>
        /// </summary>
        /// <param name="validationError">
        /// <para>The <see cref="ValidationError"/> to locate in the <see cref="ValidationErrorCollection"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The zero-based index of the first occurrence of value within the entire <see cref="ValidationErrorCollection"/>, if found; otherwise, -1.</para>
        /// </returns>
        public int IndexOf(ValidationError validationError)
        {
            return InnerList.IndexOf(validationError);
        }

        /// <summary>
        /// <para>Determines whether the <see cref="ValidationErrorCollection"/> contains a specific element.</para>
        /// </summary>
        /// <param name="validationError">
        /// <para>The <see cref="ValidationError"/> to locate in the <see cref="ValidationErrorCollection"/>.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the CollectionBase contains the specified value; otherwise, <see langword="false"/>.</para>
        /// </returns>
        public bool Contains(ValidationError validationError)
        {
            return InnerList.Contains(validationError);
        }

        /// <summary>
        /// <para>Removes the first occurrence of a specific object from the <see cref="ValidationErrorCollection"/>.</para>
        /// </summary>
        /// <param name="validationError">
        /// <para>The <see cref="ValidationError"/> to remove from the <see cref="ValidationErrorCollection"/>.</para>
        /// </param>
        public void Remove(ValidationError validationError)
        {
            InnerList.Remove(validationError);
        }

        /// <summary>
        /// <para>Adds a collection of <see cref="ValidationError"/> objects to the collection.</para>
        /// </summary>
        /// <param name="validationErrors">
        /// <para>The collection of <see cref="ValidationError"/> objects to the collection.</para>
        /// </param>
        public void AddRange(ValidationErrorCollection validationErrors)
        {
            InnerList.AddRange(validationErrors);
        }

        /// <summary>
        /// <para>Copies the entire <see cref="ValidationErrorCollection"/> to a compatible one-dimensional Array, starting at the specified index of the target array.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="ValidationError"/> array that is the destination of the elements copied from <see cref="ValidationErrorCollection"/>. The <see cref="ValidationError"/> array must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        public void CopyTo(ValidationError[] array, int index)
        {
            InnerList.CopyTo(array, index);
        }
    }
}