//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Common
{
    /// <summary>
    /// <para>Represents a collection of configuration data items.</para>
    /// </summary>  
    [XmlType(IncludeInSchema=false)]
    [Serializable]
    public abstract class DataCollection : ICollection, ISerializable, IDeserializationCallback
    {
        private const int defaultCapacity = 8;

        private bool readOnly;
        private ArrayList entries;
        private IHashCodeProvider hashCodeProvider;
        private IComparer comparer;
        private Hashtable entriesTable;
        private DataObjectEntry nullKeyEntry;
        private KeysCollection keys;
        private SerializationInfo serializationInfo;
        private int version;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DataCollection"/> class that is empty, has the default initial capacity and uses the default case-insensitive hash code provider and the default case-insensitive comparer.</para>
        /// </summary>
        protected DataCollection() : this(defaultCapacity)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DataCollection"/> class that is empty, has the specified initial capacity and uses the default case-insensitive hash code provider and the default case-insensitive comparer.</para>
        /// </summary>
        /// <param name="capacity">
        /// <para>The initial number of entries that the <see cref="DataCollection"/> can contain.</para>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="capacity"/> is less than zero.</para>
        /// </exception>
        protected DataCollection(int capacity) : this(capacity, new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture), new CaseInsensitiveComparer(CultureInfo.InvariantCulture))
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DataCollection"/> class that is empty, has the default initial capacity and uses the specified hash code provider and the specified comparer.</para>
        /// </summary>
        /// <param name="hashCodeProvider">
        /// <para>The <see cref="IHashCodeProvider"/> that will supply the hash codes for all keys in the <see cref="DataCollection"/>.</para>
        /// </param>
        /// <param name="comparer">
        /// <para>The <see cref="IComparer"/> to use to determine whether two keys are equal.</para>
        /// </param>
        protected DataCollection(IHashCodeProvider hashCodeProvider, IComparer comparer) : this(defaultCapacity, hashCodeProvider, comparer)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DataCollection"/> class that is empty, and uses the specified capacity, the specified hash code provider and the specified comparer.</para>
        /// </summary>
        /// <param name="capacity">
        /// <para>The initial number of entries that the <see cref="DataCollection"/> can contain.</para>
        /// </param>
        /// <param name="hashCodeProvider">
        /// <para>The <see cref="IHashCodeProvider"/> that will supply the hash codes for all keys in the <see cref="DataCollection"/>.</para>
        /// </param>
        /// <param name="comparer">
        /// <para>The <see cref="IComparer"/> to use to determine whether two keys are equal.</para>
        /// </param>
        protected DataCollection(int capacity, IHashCodeProvider hashCodeProvider, IComparer comparer)
        {
            this.hashCodeProvider = hashCodeProvider;
            this.comparer = comparer;
            Reset(capacity);
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DataCollection"/> class that is serializable and uses the specified <see cref="SerializationInfo"/> and <see cref="StreamingContext"/>.</para>
        /// </summary>
        /// <param name="info">
        /// <para>A <see cref="SerializationInfo"/> object that contains the information required to serialize the new <see cref="DataCollection"/> instance. </para>
        /// </param>
        /// <param name="context">
        /// <para>A <see cref="StreamingContext"/> object that contains the source and destination of the serialized stream associated with the new <see cref="DataCollection"/> instance.</para>
        /// </param>
        protected DataCollection(SerializationInfo info, StreamingContext context)
        {
            serializationInfo = info;
        }

        /// <summary>
        /// <para>Gets a <see cref="DataCollection.KeysCollection"/> instance that contains all the keys in the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <value>
        /// <para>A <see cref="DataCollection.KeysCollection"/> instance that contains all the keys in the <see cref="DataCollection"/> instance.</para>
        /// </value>
        public KeysCollection Keys
        {
            get
            {
                if (keys == null)
                {
                    keys = new KeysCollection(this);
                }
                return keys;
            }
        }

        /// <summary>
        /// Gets the number of key-and-value pairs contained in the <see cref="DataCollection"/> instance.
        /// </summary>
        /// <value>
        /// <para>The number of key-and-value pairs contained in the <see cref="DataCollection"/> instance.</para>
        /// </value>
        public int Count
        {
            get { return entries.Count; }
        }

        /// <summary>
        /// <para>Gets an object that can be used to synchronize access to the <see cref="DataCollection"/>.</para>
        /// </summary>
        /// <value>
        /// <para>An object that can be used to synchronize access to the <see cref="DataCollection"/>.</para>
        /// </value>
        public virtual object SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// <para>Gets a value indicating whether access to the <see cref="DataCollection"/> is synchronized (thread-safe).</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if access to the <see cref="DataCollection"/> is synchronized (thread-safe); otherwise, <see langword="false"/>. The default is <see langword="false"/>.</para>
        /// </value>
        public virtual bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// <para>Implements the <see cref="ISerializable"/> interface and returns the data needed to serialize the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="StreamingContext"></see>) for this serialization.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ArgumentValidation.CheckForNullReference(info, "info");

            info.AddValue("ReadOnly", readOnly);
            info.AddValue("HashProvider", hashCodeProvider, typeof (IHashCodeProvider));
            info.AddValue("Comparer", comparer, typeof (IComparer));

            int count = entries.Count;
            info.AddValue("Count", count);

            String[] keys = new String[count];
            Object[] values = new Object[count];

            for (int i = 0; i < count; i++)
            {
                DataObjectEntry entry = (DataObjectEntry) entries[i];
                keys[i] = entry.Key;
                values[i] = entry.Value;
            }

            info.AddValue("Keys", keys, typeof (String[]));
            info.AddValue("Values", values, typeof (Object[]));
        }

        /// <summary>
        /// <para>Implements the <see cref="ISerializable"/> interface and raises the deserialization event when the deserialization is complete.</para>
        /// </summary>
        /// <param name="sender">
        /// <para>The source of the deserialization event.</para>
        /// </param>
        public virtual void OnDeserialization(Object sender)
        {
            if (hashCodeProvider != null)
            {
                return;
            }

            if (serializationInfo == null)
            {
                throw new SerializationException();
            }

            SerializationInfo info = serializationInfo;
            serializationInfo = null;

            bool readOnly = info.GetBoolean("ReadOnly");
            hashCodeProvider = (IHashCodeProvider) info.GetValue("HashProvider", typeof (IHashCodeProvider));
            comparer = (IComparer) info.GetValue("Comparer", typeof (IComparer));
            int count = info.GetInt32("Count");

            String[] keys = (String[]) info.GetValue("Keys", typeof (String[]));
            Object[] values = (Object[]) info.GetValue("Values", typeof (Object[]));

            if (hashCodeProvider == null || comparer == null || keys == null || values == null)
            {
                throw new SerializationException();
            }

            Reset(count);

            for (int i = 0; i < count; i++)
            {
                BaseAdd(keys[i], values[i]);
            }

            this.readOnly = readOnly; // after collection populated
            version++;
        }

        /// <summary>
        /// <para>Removes the entry with the specified <paramref name="name"/> from the collection.</para>
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
            BaseRemove(name);
        }

        /// <summary>
        /// <para>Removes the entry at the specified index of the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index of the entry to remove.</para>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only. </para>
        /// <para>- or -</para>
        /// <para>The collection has a fixed size.</para>
        /// </exception>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Determines whether the <see cref="DataCollection"/> contains a specific key.
        /// </summary>
        /// <param name="name">
        /// <para>The key to locate in the <see cref="DataCollection"/>.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the <see cref="DataCollection"/> contains an element with the specified key; otherwise, <see langword="false"/>.</para>
        /// </returns>
        public bool Contains(string name)
        {
            return entriesTable.Contains(name);
        }

        /// <summary>
        /// <para>Removes all entries from the <see cref="DataCollection"/>.</para>
        /// </summary>
        /// <remarks>
        /// <para><seealso cref="Count"/> is set to zero.</para>
        /// </remarks>
        /// <seealso cref="IList.Clear"/>
        public void Clear()
        {
            BaseClear();
        }

        /// <summary>
        /// <para>Returns an enumerator that can iterate through the <see cref="DataCollection"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="IEnumerator"/> for the <see cref="DataCollection"/> instance.</para>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return new DataCollectionValuesEnumerator(this);
        }

        /// <summary>
        /// <para>Copies the entire <see cref="DataCollection"/> to a compatible one-dimensional Array, starting at the specified index of the target array.</para>
        /// </summary>
        /// <param name="array">
        /// <para>The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="DataCollection"/>. The <see cref="Array"/> must have zero-based indexing.</para>
        /// </param>
        /// <param name="index">
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </param>
        public void CopyTo(Array array, int index)
        {
            for (IEnumerator e = this.GetEnumerator(); e.MoveNext(); )
            {
                array.SetValue(e.Current, index++);
            }
        }

        /// <summary>
        /// <para>Gets or sets a value indicating whether the <see cref="DataCollection"/> instance is read-only.</para>
        /// </summary>
        /// <value><see langword="true"/> if the <see cref="DataCollection"/> instance is read-only; otherwise, <see langword="false"/>.</value>
        protected bool IsReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        /// <summary>
        /// <para>Gets a value indicating whether the <see cref="DataCollection"/> instance contains entries whose keys are not a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
        /// </summary>
        /// <returns>
        /// <para><see langword="true"/> if the <see cref="DataCollection"/> instance contains entries whose keys are not a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic); otherwise, <see lagword="false"/>.</para>
        /// </returns>
        protected bool BaseHasKeys()
        {
            return (entriesTable.Count > 0); // any entries with keys?
        }

        /// <summary>
        /// <para>Adds an entry with the specified key and value into the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The string key of the entry to add. The key can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic)</para>.
        /// </param>
        /// <param name="value">
        /// <para>The object value of the entry to add. The value can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para>The collection already contains an entry for <paramref name="name"/>.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only.</para>
        /// <para>- or -</para>
        /// <para>The collection has a fixed size.</para>
        /// </exception>
        protected void BaseAdd(String name, Object value)
        {
            if (readOnly)
            {
                throw new NotSupportedException(SR.ExceptionCollectionReadOnly);
            }

            DataObjectEntry entry = new DataObjectEntry(name, value);

            // insert entry into hashtable
            if (name != null)
            {
                if (entriesTable[name] == null)
                {
                    entriesTable.Add(name, entry);
                }
                else
                {
                    throw new InvalidOperationException(SR.ExceptionDuplicateEntry(name));
                }
            }
            else
            { // null key -- special case -- hashtable doesn't like null keys
                if (nullKeyEntry == null)
                {
                    nullKeyEntry = entry;
                }
            }

            // add entry to the list
            entries.Add(entry);

            version++;
        }

        /// <summary>
        /// <para>Removes the entries with the specified key from the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The string key of the entry to add. The key can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic)</para>.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only.</para>
        /// <para>- or -</para>
        /// <para>The collection has a fixed size.</para>
        /// </exception>
        protected void BaseRemove(String name)
        {
            CheckIfReadOnly();

            if (name != null)
            {
                // remove from hashtable
                entriesTable.Remove(name);

                // remove from array
                for (int i = entries.Count - 1; i >= 0; i--)
                {
                    if (comparer.Compare(name, BaseGetKey(i)) == 0)
                    {
                        entries.RemoveAt(i);
                    }
                }
            }
            else
            { // null key -- special case
                // null out special 'null key' entry
                nullKeyEntry = null;

                // remove from array
                for (int i = entries.Count - 1; i >= 0; i--)
                {
                    if (BaseGetKey(i) == null)
                    {
                        entries.RemoveAt(i);
                    }
                }
            }

            version++;
        }

        /// <summary>
        /// <para>Removes the entry at the specified index of the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index of the entry to remove.</para>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only.</para>
        /// <para>- or -</para>
        /// <para>The collection has a fixed size.</para>
        /// </exception>
        protected void BaseRemoveAt(int index)
        {
            CheckIfReadOnly();

            String key = BaseGetKey(index);

            if (key != null)
            {
                // remove from hashtable
                entriesTable.Remove(key);
            }
            else
            { // null key -- special case
                // null out special 'null key' entry
                nullKeyEntry = null;
            }

            // remove from array
            entries.RemoveAt(index);

            version++;
        }

        /// <summary>
        /// Removes all entries from the <see cref="DataCollection"/> instance.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only.</para>
        /// </exception>
        protected void BaseClear()
        {
            CheckIfReadOnly();

            Reset();
        }

        /// <summary>
        /// <para>Gets the value of the entry at the specified index of the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index of the value to get.</para>
        /// </param>
        /// <returns>
        /// <para>An object that represents the value of the entry at the specified index.</para>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
        /// </exception>
        protected Object BaseGet(int index)
        {
            DataObjectEntry entry = (DataObjectEntry) entries[index];
            return entry.Value;
        }

        /// <summary>
        /// <para>Gets the value of the first entry with the specified key from the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The string key of the entry to add. The key can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic)</para>.
        /// </param>
        /// <returns>
        /// <para>An object that represents the value of the first entry with the specified key, if found; otherwise, a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
        /// </returns>
        protected Object BaseGet(String name)
        {
            DataObjectEntry e = FindEntry(name);
            return (e != null) ? e.Value : null;
        }

        /// <summary>
        /// <para>Sets the value of the entry at the specified index of the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index of the entry to set.</para>
        /// </param>
        /// <param name="value">
        /// <para>The object that represents the new value of the entry to set. The value can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
        /// </param>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
        /// </exception>
        protected void BaseSet(int index, Object value)
        {
            CheckIfReadOnly();

            DataObjectEntry entry = (DataObjectEntry) entries[index];
            entry.Value = value;
            version++;
        }

        /// <summary>
        /// <para>Sets the value of the first entry with the specified key in the <see cref="DataCollection"/> instance, if found; otherwise, adds an entry with the specified key and value into the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The string key of the entry to set. The key can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic)</para>.
        /// </param>
        /// <param name="value">
        /// <para>The object that represents the new value of the entry to set. The value can be a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <para>The collection already contains an entry for <paramref name="name"/>.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <para>The collection is read-only.</para>
        /// </exception>
        protected void BaseSet(String name, Object value)
        {
            CheckIfReadOnly();

            DataObjectEntry entry = FindEntry(name);
            if (entry != null)
            {
                entry.Value = value;
                version++;
            }
            else
            {
                BaseAdd(name, value);
            }
        }

        /// <summary>
        /// <para>Gets the key of the entry at the specified index of the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The zero-based index of the key to get.</para>
        /// </param>
        /// <returns>
        /// <para>A string that represents the key of the entry at the specified index.</para>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
        /// </exception>
        protected String BaseGetKey(int index)
        {
            DataObjectEntry entry = (DataObjectEntry) entries[index];
            return entry.Key;
        }

        /// <summary>
        /// <para>Returns a String array that contains all the keys in the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A string array that contains all the keys in the <see cref="DataCollection"/> instance.</para>
        /// </returns>
        protected String[] BaseGetAllKeys()
        {
            int n = entries.Count;
            String[] allKeys = new String[n];

            for (int i = 0; i < n; i++)
            {
                allKeys[i] = BaseGetKey(i);
            }

            return allKeys;
        }

        /// <summary>
        /// <para>Returns an Object array that contains all the values in the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <returns>
        /// <para>An object array that contains all the values in the <see cref="DataCollection"/> instance.</para>
        /// </returns>
        protected Object[] BaseGetAllValues()
        {
            int n = entries.Count;
            Object[] allValues = new Object[n];

            for (int i = 0; i < n; i++)
            {
                allValues[i] = BaseGet(i);
            }

            return allValues;
        }

        /// <summary>
        /// <para>Returns an array of the specified type that contains all the values in the <see cref="DataCollection"/> instance.</para>
        /// </summary>
        /// <param name="type">
        /// <para>A <see cref="Type"/> that represents the type of array to return.</para>
        /// </param>
        /// <returns>
        /// <para>An array of the specified type that contains all the values in the <see cref="DataCollection"/> instance.</para>
        /// </returns>
        protected object[] BaseGetAllValues(Type type)
        {
            int n = entries.Count;
            object[] allValues = (object[]) Array.CreateInstance(type, n);

            for (int i = 0; i < n; i++)
            {
                allValues[i] = BaseGet(i);
            }

            return allValues;
        }

        private void Reset()
        {
            entries = new ArrayList();
            entriesTable = new Hashtable(hashCodeProvider, comparer);
            nullKeyEntry = null;
            version++;
        }

        private void Reset(int capacity)
        {
            entries = new ArrayList(capacity);
            entriesTable = new Hashtable(capacity, hashCodeProvider, comparer);
            nullKeyEntry = null;
            version++;
        }

        private DataObjectEntry FindEntry(String key)
        {
            if (key != null)
            {
                return (DataObjectEntry) entriesTable[key];
            }
            else
            {
                return nullKeyEntry;
            }
        }

        private void CheckIfReadOnly()
        {
            if (readOnly)
            {
                throw new NotSupportedException(SR.ExceptionCollectionReadOnly);
            }
        }

        internal class DataObjectEntry
        {
            internal DataObjectEntry(String name, object value)
            {
                Key = name;
                Value = value;
            }

            internal String Key;
            internal object Value;
        }

        [Serializable()]
        internal class DataCollectionValuesEnumerator : IEnumerator
        {
            private int position;
            private DataCollection collection;
            private int version;

            internal DataCollectionValuesEnumerator(DataCollection collection)
            {
                this.collection = collection;
                version = collection.version;
                position = -1;
            }

            public object Current
            {
                get
                {
                    if (!collection.readOnly && version != collection.version)
                    {
                        throw new InvalidOperationException(SR.ExceptionCollectionModified);
                    }
                    else if (position >= 0 && position < collection.Count)
                    {
                        return collection.BaseGet(position);
                    }
                    else
                    {
                        throw new InvalidOperationException(SR.ExceptionCollectionModified);
                    }
                }
            }

            public bool MoveNext()
            {
                if (!collection.readOnly && version != collection.version)
                {
                    throw new InvalidOperationException(SR.ExceptionCollectionModified);
                }
                if (position < collection.Count - 1)
                {
                    ++position;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                if (!collection.readOnly && version != collection.version)
                {
                    throw new InvalidOperationException(SR.ExceptionCollectionModified);
                }
                position = -1;
            }
        }

        /// <summary>
        /// <para>Represents a collection of the String keys of a collection.</para>
        /// </summary>
        [Serializable()]
        public sealed class KeysCollection : ICollection
        {
            private DataCollection collection;

            internal KeysCollection(DataCollection collection)
            {
                this.collection = collection;
            }

            /// <summary>
            /// <para>Gets the key at the specified index of the collection.</para>
            /// </summary>
            /// <param name="index"><para>The zero-based index of the key to get from the collection.</para></param>
            /// <returns><para>A string that contains the key at the specified index of the collection.</para></returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
            /// </exception>
            public String Get(int index)
            {
                return collection.BaseGetKey(index);
            }

            /// <summary>
            /// <para>Gets the entry at the specified index of the collection.</para>
            /// </summary>
            /// <param name="index">
            /// <para>The zero-based index of the entry to locate in the collection.</para>
            /// </param>
            /// <value>
            /// <para>The string key of the entry at the specified index of the collection.</para>
            /// </value>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <para><paramref name="index"/> is outside the valid range of indexes for the collection.</para>
            /// </exception>
            public string this[int index]
            {
                get { return Get(index); }
            }

            /// <summary>
            /// <para>Returns an enumerator that can iterate through the <see cref="DataCollection.KeysCollection"/>.</para>
            /// </summary>
            /// <returns>
            /// <para>An <para>IEnumerator</para> for the <see cref="DataCollection.KeysCollection"/>.</para>
            /// </returns>
            public IEnumerator GetEnumerator()
            {
                return new DataCollectionValuesEnumerator(collection);
            }

            /// <summary>
            /// <para>Gets the number of keys in the <see cref="DataCollection.KeysCollection"/>.</para>
            /// </summary>
            /// <value>
            /// The number of keys in the <see cref="DataCollection.KeysCollection"/>.
            /// </value>
            public int Count
            {
                get { return collection.Count; }
            }

            /// <summary>
            /// <para>Copies the entire <see cref="DataCollection.KeysCollection"/> to a compatible one-dimensional Array, starting at the specified index of the target array.</para>
            /// </summary>
            /// <param name="array">
            /// <para>The one-dimensional Array that is the destination of the elements copied from <see cref="DataCollection.KeysCollection"/>. The Array must have zero-based indexing.</para>
            /// </param>
            /// <param name="index">
            /// <para>The zero-based index in array at which copying begins. </para>
            /// </param>
            /// <excetion cref="ArgumentNullException">
            /// <para><paramref name="array"/> is a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
            /// </excetion>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <para><paramref name="index"/> is less than zero.</para>
            /// </exception>
            /// <exception cref="ArgumentException">
            /// <para><paramref name="array"/> is multidimensional</para>
            /// <para>-or-</para>
            /// <para><paramref name="index"/> is equal to or greater than the length of array.</para>
            /// <para>-or-</para>
            /// <para>The number of elements in the source <see cref="DataCollection.KeysCollection"/> is greater than the available space from index to the end of the destination array.</para>
            /// </exception>
            /// <exception cref="InvalidCastException">
            /// <para>The type of the source <see cref="DataCollection.KeysCollection"/> cannot be cast automatically to the type of the destination array.</para>
            /// </exception>
            void ICollection.CopyTo(Array array, int index)
            {
                string[] stringArray = array as string[];
                if (stringArray == null)
                {
                    throw new ArgumentException(SR.ExceptionStringArray, "array");
                }
                CopyTo(stringArray, index);
            }

            /// <summary>
            /// <para>Copies the entire <see cref="DataCollection.KeysCollection"/> to a compatible one-dimensional Array, starting at the specified index of the target array.</para>
            /// </summary>
            /// <param name="array">
            /// <para>The one-dimensional Array that is the destination of the elements copied from <see cref="DataCollection.KeysCollection"/>. The Array must have zero-based indexing.</para>
            /// </param>
            /// <param name="index">
            /// <para>The zero-based index in array at which copying begins. </para>
            /// </param>
            /// <excetion cref="ArgumentNullException">
            /// <para><paramref name="array"/> is a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic).</para>
            /// </excetion>
            /// <exception cref="ArgumentOutOfRangeException">
            /// <para><paramref name="index"/> is less than zero.</para>
            /// </exception>
            /// <exception cref="ArgumentException">
            /// <para><paramref name="array"/> is multidimensional</para>
            /// <para>-or-</para>
            /// <para><paramref name="index"/> is equal to or greater than the length of array.</para>
            /// <para>-or-</para>
            /// <para>The number of elements in the source <see cref="DataCollection.KeysCollection"/> is greater than the available space from index to the end of the destination array.</para>
            /// </exception>
            /// <exception cref="InvalidCastException">
            /// <para>The type of the source <see cref="DataCollection.KeysCollection"/> cannot be cast automatically to the type of the destination array.</para>
            /// </exception>
            public void CopyTo(string[] array, int index)
            {
                for (IEnumerator e = this.GetEnumerator(); e.MoveNext(); )
                {
                    array.SetValue(e.Current, index++);
                }
            }

            /// <summary>
            /// <para>Gets an object that can be used to synchronize access to the <see cref="DataCollection.KeysCollection"/>.</para>
            /// </summary>
            /// <value>
            /// <para>An object that can be used to synchronize access to the <see cref="DataCollection.KeysCollection"/>.</para>
            /// </value>
            object ICollection.SyncRoot
            {
                get { return collection; }
            }

            /// <summary>
            /// <para>Gets an object that can be used to synchronize access to the <see cref="DataCollection.KeysCollection"/>.</para>
            /// </summary>
            /// <value>
            /// <para>an object that can be used to synchronize access to the <see cref="DataCollection.KeysCollection"/>.</para>
            /// </value>
            bool ICollection.IsSynchronized
            {
                get { return false; }
            }
        }
    }
}