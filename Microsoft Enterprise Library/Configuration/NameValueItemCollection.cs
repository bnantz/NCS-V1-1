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
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a collection of <see cref="NameValueItem"/> objects.</para>
    /// </summary>
    [XmlRoot("extensions")]
    public class NameValueItemCollection : CollectionBase
    {
        private SortedList dictionary = new SortedList();

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="NameValueItemCollection"/>.</para>
        /// </summary>
        public NameValueItemCollection()
        {
        }

        internal NameValueItemCollection(NameValueItemCollection items)
        {
            foreach (NameValueItem item in items)
            {
                Add(item.Name, item.Value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="NameValueItem"/> associated with the specified <paramref name="index"/>.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="NameValueItem"/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="index"/>.</para>
        /// </value>              
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is equal to or greater than the length of <c>Count</c>.</para>
        /// </exception>
        public NameValueItem this[int index]
        {
            get { return (NameValueItem)this.List[index]; }
            set { this.List[index] = value; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="NameValueItem"/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="NameValueItem"/> value to get.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </value>              
        public string this[string name]
        {
            get
            {
                NameValueItem item = (NameValueItem)this.dictionary[name];
                if (item != null)
                {
                    return item.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="NameValueItem"/> that is
        /// keyed by the specified name.
        /// </summary>
        /// <param name="name">The name of an item in the list.</param>
        /// <returns>A <see cref="NameValueItem"/>.</returns>
        public NameValueItem GetNameValueItem(string name)
        {
            return (NameValueItem)this.dictionary[name];
        }

        /// <summary>
        /// Adds the specified name and value to the list.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="value">The value of the item.</param>
        public void Add(string name, string value)
        {
            this.Add(new NameValueItem(name, value));
        }

        /// <summary>
        /// <para>Adds a value into the collection.</para>
        /// </summary>
        /// <param name="item">
        /// <para>The value to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>This method exists to support Xml Serialization.</para>
        /// </remarks>
        /// <exception cref="InvalidCastException">
        /// <para><paramref name="item"/> could not be cast to the correct type.</para>
        /// </exception>        
        public void Add(object item)
        {
            this.List.Add(item);
        }

        /// <summary>
        /// <para>Adds an <see cref="NameValueItem"/> into the collection.</para>
        /// </summary>
        /// <param name="item">
        /// <para>The <see cref="NameValueItem"/> to add. The value can not be a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </param>
        /// <remarks>
        /// <para>If a reference already exists in the collection by <seealso cref="XmlIncludeTypeData.Name"/>, it will be replaced with the new reference.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="item"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><seealso cref="NameValueItem.Name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Add(NameValueItem item)
        {
            this.List.Add(item);
        }

        /// <summary>
        /// <para>Inserts an element into the <see cref="NameValueItemCollection"/> at the specified index.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index at which value should be inserted.</para>
        /// </param>
        /// <param name="item">
        /// <para>The <see cref="NameValueItem"/> to insert.</para>
        /// </param>
        /// <exception name="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than zero.</para>
        /// <para>- or -</para>
        /// <para><paramref name="index"/> is greater than <seealso cref="CollectionBase.Count"/>.</para>
        /// </exception>
        public void Insert(int index, NameValueItem item)
        {
            this.List.Insert(index, item);
        }

        /// <summary>
        /// Removes the entry with the specified <paramref name="name"/> from the collection.
        /// </summary>
        /// <param name="name">
        /// <para>The name of the item to remove.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void Remove(string name)
        {
            ArgumentValidation.CheckForNullReference(name, "name");

            NameValueItem item = (NameValueItem)this.dictionary[name];
            if (item != null)
            {
                this.List.RemoveAt(this.List.IndexOf(item));
            }
        }

        /// <summary>
        /// <para>Searches for the specified <see cref="NameValueItem"/> and returns the zero-based index of the first occurrence within the entire <see cref="NameValueItemCollection"/>.</para>
        /// </summary>
        /// <param name="item">
        /// <para>The <see cref="NameValueItem"/> to locate in the <see cref="NameValueItemCollection"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The zero-based index of the first occurrence of value within the entire <see cref="NameValueItemCollection"/>, if found; otherwise, -1.</para>
        /// </returns>
        public int IndexOf(NameValueItem item)
        {
            return List.IndexOf(item);
        }

        /// <summary>
        /// <para>Determines whether the <see cref="NameValueItemCollection"/> contains a specific element.</para>
        /// </summary>
        /// <param name="item">
        /// <para>The Object to locate in the <see cref="NameValueItemCollection"/>.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the <see cref="NameValueItemCollection"/> contains the specified value; otherwise, <see langword="false"/></para>
        /// </returns>
        public bool Contains(NameValueItem item)
        {
            return List.Contains(item);
        }

        /// <summary>
        /// Copies the entire NameValueItem list to a 
        /// compatible one-dimensional array of NameValueItem entries, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">An array of type NameValueItem to which 
        /// the access control list entries will be copied. The array 
        /// must have zero-based indexing. </param>
        /// <param name="index">The index in the array at which to 
        /// begin copying the NameValueItem list entries.</param>
        public void CopyTo(NameValueItem[] array, int index)
        {
            this.List.CopyTo(array, index);
        }

        internal void Set(NameValueItem item)
        {
            int index = this.List.IndexOf(item);
            if (index >= 0)
            {
                this[index] = item;
            }
            else
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Removes the collection reference from each
        /// NameValueItem in the list.
        /// </summary>
        protected override void OnClear()
        {
            foreach (NameValueItem item in this.List)
            {
                item.Owner = null;
            }
            base.OnClear();
        }

        /// <summary>
        /// Clears all items from the dictionary.
        /// </summary>
        protected override void OnClearComplete()
        {
            this.dictionary.Clear();
            base.OnClearComplete();
        }

        /// <summary>
        /// Checks if the item already exists in the collection.
        /// </summary>
        /// <param name="index">The zero-based index at which to insert <c>value</c>.</param>
        /// <param name="value">The new value of the element at <c>index</c>.</param>
        protected override void OnInsert(int index, object value)
        {
            if (this.List.Contains(value))
            {
                throw new ArgumentException(SR.ExceptionItemAlreadyAdded);
            }
            base.OnInsert(index, value);
        }

        /// <summary>
        /// Inserts the specified item into the dictionary.
        /// </summary>
        /// <param name="index">The index of the item in the list.</param>
        /// <param name="value">The value of the item to add to the list.</param>
        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            NameValueItem item = (NameValueItem)value;
            item.Owner = this;
            if ((item.Name != null) && (item.Name.Length != 0))
            {
                this.dictionary.Add(item.Name, item);
            }
        }

        /// <summary>
        /// Removes the item at the specified index 
        /// from the dictionary.
        /// </summary>
        /// <param name="index">The index of the item in the list.</param>
        /// <param name="value">The value of the item at the specified index.</param>
        protected override void OnRemoveComplete(int index, object value)
        {
            NameValueItem item = (NameValueItem)value;
            this.RemoveItem(item);
            item.Owner = null;
            base.OnRemoveComplete(index, value);
        }

        /// <summary>
        /// Checks that the argument is of type 
        /// <see cref="NameValueItem"/>.
        /// </summary>
        /// <param name="value">The value of the item to add.</param>
        protected override void OnValidate(object value)
        {
            base.OnValidate(value);

            if (!(value is NameValueItem))
            {
                throw new ArgumentException(SR.ExceptionInvalidArgumentType(typeof(NameValueItem).Name), "value");
            }
        }

        /// <summary>
        /// Updates the value dictionary with the new value.
        /// </summary>
        /// <param name="index">The index of the item in the list.</param>
        /// <param name="oldValue">The old value that is being replaced.</param>
        /// <param name="newValue">The new value that is being set.</param>
        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            base.OnSetComplete(index, oldValue, newValue);
            if (oldValue != null)
            {
                NameValueItem olditem = (NameValueItem)oldValue;
                this.RemoveItem(olditem);
            }
            NameValueItem newitem = (NameValueItem)newValue;
            newitem.Owner = this;
            this.dictionary[newitem.Name] = newValue;
        }

        private void RemoveItem(NameValueItem olditem)
        {
            olditem.Owner = null;
            int oldIndex = this.dictionary.IndexOfValue(olditem);
            if (oldIndex >= 0)
            {
                this.dictionary.RemoveAt(oldIndex);
            }
        }
    }
}