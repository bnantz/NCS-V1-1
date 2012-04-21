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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>A read-only collection of <see cref="ValidationError"/> objects.</para>
    /// </summary>	
    public sealed class ReadOnlyValidationErrorCollection : ICollection
    {
        private readonly Validation.ValidationErrorCollection validationErrorCollection;

        internal ReadOnlyValidationErrorCollection(Validation.ValidationErrorCollection validationErrorCollection)
        {
            this.validationErrorCollection = validationErrorCollection;
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
        public void CopyTo(Validation.ValidationError[] array, int index)
        {
            validationErrorCollection.CopyTo(array, index);
        }

        /// <summary>
        /// <para>Copies the elements of the collection to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="Array"/> array that is the destination of the elements copied from <see cref="Array"/>. The array must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        void System.Collections.ICollection.CopyTo(System.Array array, int index)
        {
            CopyTo((Validation.ValidationError[])array, index);
        }

        /// <summary>
        /// <para>Gets the number of elements contained in the collection.</para>
        /// </summary>
        /// <value>
        /// <para>The number of elements contained in the collection.</para>
        /// </value>
        public int Count
        {
            get { return validationErrorCollection.Count; }
        }

        /// <summary>
        /// <para>Gets an object that can be used to synchronize access to the collection.</para>
        /// </summary>
        /// <value>
        /// <para>An object that can be used to synchronize access to the collection.</para>
        /// </value>
        public object SyncRoot
        {
            get { return ((System.Collections.ICollection)validationErrorCollection).SyncRoot; }
        }

        /// <summary>
        /// <para>Gets a value indicating whether access to the ICollection is synchronized (thread-safe).</para>
        /// </summary>
        /// <value>
        /// <para>A value indicating whether access to the ICollection is synchronized (thread-safe).</para>
        /// </value>
        public bool IsSynchronized
        {
            get { return ((System.Collections.ICollection)validationErrorCollection).IsSynchronized; }
        }

        /// <summary>
        /// <para>Returns an enumerator that can iterate through a collection.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</para>
        /// </returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return validationErrorCollection.GetEnumerator();
        }
    }
}
