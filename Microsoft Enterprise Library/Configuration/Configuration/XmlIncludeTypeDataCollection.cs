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
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a collection of <see cref="XmlIncludeTypeData"/>s for the <see cref="XmlSerializerTransformerData"/> in configuration.</para>
    /// </summary>       
    [Serializable]
    public class XmlIncludeTypeDataCollection : DataCollection
    {
        /// <summary>
        /// <para>Gets or sets the <see cref="XmlIncludeTypeData"/> at the specified <paramref name="index"/>.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="XmlIncludeTypeData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="index"/>. If the specified <paramref name="index"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="index"/>.</para>
        /// </value>
        public XmlIncludeTypeData this[int index]
        {
            get { return (XmlIncludeTypeData)BaseGet(index); }
            set { BaseSet(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="XmlIncludeTypeData"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="XmlIncludeTypeData"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public XmlIncludeTypeData this[string name]
        {
            get
            {
                ArgumentValidation.CheckForNullReference(name, "name");

                return BaseGet(name) as XmlIncludeTypeData;
            }
            set
            {
                ArgumentValidation.CheckForNullReference(name, "name");

                BaseSet(name, value);
            }
        }

        /// <summary>
        /// <para>Adds an <see cref="XmlIncludeTypeData"/> into the collection.</para>
        /// </summary>
        /// <param name="xmlIncludeType">
        /// <para>The <see cref="XmlIncludeTypeData"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="XmlIncludeTypeData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="xmlIncludeType"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><seealso cref="XmlIncludeTypeData.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(XmlIncludeTypeData xmlIncludeType)
        {
            ArgumentValidation.CheckForNullReference(xmlIncludeType, "xmlIncludeType");
            ArgumentValidation.CheckForInvalidNullNameReference(xmlIncludeType.Name, "xmlIncludeType");

            BaseAdd(xmlIncludeType.Name, xmlIncludeType);
        }

        /// <summary>
        /// <para>Copies the entire <see cref="XmlIncludeTypeDataCollection"/> to a compatible one-dimensional Array, starting at the specified index of the target array.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="XmlIncludeTypeData"/> array that is the destination of the elements copied from <see cref="XmlIncludeTypeDataCollection"/>. The <see cref="XmlIncludeTypeData"/> array must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        public void CopyTo(XmlIncludeTypeData[] array, int index)
        {
            for (IEnumerator e = this.GetEnumerator(); e.MoveNext(); )
            {
                array.SetValue(e.Current, index++);
            }
        }

    }
}