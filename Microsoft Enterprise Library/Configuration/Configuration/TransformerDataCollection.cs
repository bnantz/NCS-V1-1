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
    /// <para>Represents a collection of <see cref="TransformerData"/> objects in configuration.</para>
    /// </summary>       
    [Serializable]
    public class TransformerDataCollection : ProviderDataCollection
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="TransformerDataCollection"/> class.</para>
        /// </summary>
        public TransformerDataCollection()
        {
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="TransformerData"/> at the specified <paramref name="index"/>.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="TransformerData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="index"/>. If the specified <paramref name="index"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="index"/>.</para>
        /// </value>
        public TransformerData this[int index]
        {
            get { return (TransformerData)GetProvider(index); }
            set { SetProvider(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="TransformerData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="TransformerData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public TransformerData this[string name]
        {
            get { return (TransformerData)base.GetProvider(name); }
            set { base.SetProvider(name, value); }
        }

        /// <summary>
        /// <para>Adds an <see cref="TransformerData"/> into the collection.</para>
        /// </summary>
        /// <param name="transformerData">
        /// <para>The <see cref="TransformerData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="ProviderData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// <para><paramref name="transformerData"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <para><seealso cref="ProviderData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(TransformerData transformerData)
        {
            base.AddProvider(transformerData);
        }

        /// <summary>
        /// <para>Copies the entire <see cref="TransformerDataCollection"/> to a compatible one-dimensional <see cref="TransformerData"/> array, starting at the specified index of the target array.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="TransformerData"/> array that is the destination of the elements copied from <see cref="TransformerDataCollection"/>. The <see cref="TransformerData"/> array must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        public void CopyTo(TransformerData[] array, int index)
        {
            for (IEnumerator e = this.GetEnumerator(); e.MoveNext(); )
            {
                array.SetValue(e.Current, index++);
            }
        }
    }
}