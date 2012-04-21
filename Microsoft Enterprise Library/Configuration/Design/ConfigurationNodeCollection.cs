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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a collection of <see cref="ConfigurationNode"/> objects.</para>
    /// </summary>
    [Serializable]
    public sealed class ConfigurationNodeCollection : IList
    {
        [NonSerialized] private ConfigurationNode owner;

        internal ConfigurationNodeCollection(ConfigurationNode owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// <para>Gets the total number of <see cref="ConfigurationNode"/> objects in the collection.</para>
        /// </summary>
        /// <value>
        /// <para>The total number of <see cref="ConfigurationNode"/> objects in the collection.</para>
        /// </value>
        public int Count
        {
            get { return owner.ChildCount; }
        }

        /// <summary>
        /// <para>Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.</para>
        /// </summary>        
        /// <returns>
        /// <para>An object that can be used to synchronize access to the <see cref="ICollection"/>.</para>
        /// </returns>
        object ICollection.SyncRoot
        {
            get { return this; }
        }

        /// <summary>
        /// <para>gets a value indicating whether access to the <see cref="ICollection"/> is synchronized (thread-safe).</para>
        /// </summary>        
        /// <returns>
        /// <para><see langword="true"/> if access to the <see cref="ICollection"/> is synchronized (thread-safe); otherwise, <see langword="false"/>.</para>
        /// </returns>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// <para>Gets a value indicating whether the <see cref="IList"/> has a fixed size.</para>
        /// </summary>        
        /// <returns>
        /// <para><see langword="true"/> if the <see cref="IList"/> has a fixed size; otherwise, <see langword="false"/></para>
        /// </returns>
        bool IList.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// <para>Gets a value indicating whether the collection is read-only.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the IList is read-only; otherwise, <see langword="false"/>.</para>
        /// </value>
        /// <seealso cref="IList.IsReadOnly"/>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// <para>Gets or sets a node at the given index.</para>
        /// </summary>        
        /// <param name="index"><para>The zero-based index of the parameter to retrieve.</para></param>
        /// <value><para>The <see cref="ConfigurationNode"/> at the specified index.</para></value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>The <paramref name="index"/> value is less than zero or is greater than the number of nodes in the collection..</para>
        /// </exception>
        public ConfigurationNode this[int index]
        {
            get
            {
                if (index < 0 || index >= owner.ChildCount)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                return owner.ChildNodes[index];
            }
            set
            {
                if (index < 0 || index >= owner.ChildCount)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                Add(value);
            }
        }

        /// <summary>
        /// <para>Gets or sets a element at the given index.</para>
        /// </summary>        
        /// <param name="index"><para>The zero-based index of the element to retrieve.</para></param>
        /// <value><para>The element at the specified index.</para></value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>The <paramref name="index"/> value is less than zero or is greater than the number of nodes in the collection..</para>
        /// </exception>
        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                if (value is ConfigurationNode)
                {
                    this[index] = (ConfigurationNode)value;
                }
                else
                {
                    throw new ArgumentException(SR.ExceptionConfigNodeExpected, "value");
                }

            }
        }

        /// <summary>
        /// <para>Adds an array of previously created nodes to the collection.</para>
        /// </summary>
        /// <param name="nodes">
        /// <para>An array of <see cref="ConfigurationNode"/> objects representing the tree nodes to add to the collection.</para>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="nodes"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="nodes"/> contains one or more <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public void AddRange(ConfigurationNode[] nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }
            foreach (ConfigurationNode node in nodes)
            {
                Add(node);
            }
        }

        /// <summary>
        /// <para>Adds a previously created node to the end of the collection.</para>
        /// </summary>
        /// <param name="node"><para>The <see cref="ConfigurationNode"/> to add to the collection.</para></param>
        /// <returns><para>The zero-based index value of the <see cref="ConfigurationNode"/> added to the tree node collection.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="node"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        /// <see cref="IList.Add"/>
        public int Add(ConfigurationNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            return owner.AddNode(node);
        }

        /// <summary>
        /// <para>Adds an item to the <see cref="IList"/>.</para>
        /// </summary>
        /// <param name="value">
        /// <para>The <see cref="Object"/> to add to the <see cref="IList"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The position into which the new element was inserted.</para>
        /// </returns>
        int IList.Add(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("node");
            }
            if (value is ConfigurationNode)
            {
                return Add((ConfigurationNode)value);
            }
            throw new ArgumentOutOfRangeException("node");
        }

        /// <summary>
        /// <para>Adds a previously created node to the end of the collection and adds any default.</para>
        /// </summary>
        /// <param name="node"><para>The <see cref="ConfigurationNode"/> to add to the collection.</para></param>
        /// <returns><para>The zero-based index value of the <see cref="ConfigurationNode"/> added to the tree node collection.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="node"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public int AddWithDefaultChildren(ConfigurationNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            return owner.AddNodeWithDefaultChildren(node);
        }

        /// <summary>
        /// <para>Determines whether the specified node is a member of the collection.</para>
        /// </summary>
        /// <param name="node"><para>The <see cref="ConfigurationNode"/> to locate in the collection.</para></param>
        /// <returns><see langword="true"/> if the <see cref="ConfigurationNode"/> is a member of the collection; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="IList.Contains"/>
        public bool Contains(ConfigurationNode node)
        {
            return IndexOf(node) != -1;
        }

        /// <summary>
        /// <para>Determines whether the specified node name is a member of the collection.</para>
        /// </summary>
        /// <param name="nodeName"><para>The node name to locate in the collection.</para></param>
        /// <returns><see langword="true"/> if the <see cref="ConfigurationNode"/> is a member of the collection; otherwise, <see langword="false"/>.</returns>
        public bool Contains(string nodeName)
        {
            return owner.childNodeLookup.ContainsKey(nodeName);
        }

        /// <summary>
        /// <para>Determines whether the <see cref="IList"/> contains a specific value.</para>
        /// </summary>
        /// <param name="value">
        /// <para>The <see cref="Object"/> to locate in the <see cref="IList"/>.</para>
        /// </param>
        /// <returns>
        /// <para><see langword="true"/> if the <see cref="Object"/> is found in the <see cref="IList"/>; otherwise, <see langword="false"/></para>
        /// </returns>
        bool IList.Contains(object value)
        {
            if (value is ConfigurationNode)
            {
                return Contains((ConfigurationNode)value);
            }
            return false;
        }

        /// <summary>
        /// <para>Determines the index of a specific node in the collection.</para>
        /// </summary>
        /// <param name="node"><para>Determines the index of a specific node in the collection.</para></param>
        /// <returns><para>The index of the node if found in the list; otherwise, -1.</para></returns>
        /// <seealso cref="IList.IndexOf"/>
        public int IndexOf(ConfigurationNode node)
        {
            for (int index = 0; index < Count; ++index)
            {
                if (this[index].CompareTo(node) == 0)
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// <para>Determines the index of a specific item in the <see cref="IList"/>.</para>
        /// </summary>
        /// <param name="value">
        /// <para>The <see cref="Object"/> to locate in the <see cref="IList"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The index of value if found in the list; otherwise, -1.</para>
        /// </returns>
        int IList.IndexOf(object value)
        {
            if (value is ConfigurationNode)
            {
                return IndexOf((ConfigurationNode)value);
            }
            return -1;
        }

        /// <summary>
        /// <para>Inserts a node into the collection at the specified index.</para>
        /// </summary>
        /// <param name="index"><para>The zero-based index location where the item is inserted.</para></param>
        /// <param name="node"><para>A <see cref="ConfigurationNode"/> to insert into the collection.</para></param>        
        /// <seealso cref="IList.Insert"/>
        public void Insert(int index, ConfigurationNode node)
        {
            if (index < 0)
            {
                index = 0;
            }
            if (index > owner.ChildCount)
            {
                index = owner.ChildCount;
            }
            owner.InsertNodeAt(index, node, true);
        }

        /// <summary>
        /// <para>Inserts an item to the <see cref="IList"/> at the specified position.</para>
        /// </summary>
        /// <param name="index"><para>The zero-based index at which value should be inserted.</para></param>
        /// <param name="value"><para>The <see cref="Object"/> to insert into the <see cref="IList"/>.</para></param>        
        /// <returns/>
        void IList.Insert(int index, object value)
        {
            if (value is ConfigurationNode)
            {
                Insert(index, (ConfigurationNode)value);
            }
            else
            {
                throw new ArgumentException(SR.ExceptionConfigNodeExpected, "value");
            }
        }

        /// <summary>
        /// <para>Removes all nodes from the collection.</para>
        /// </summary>
        public void Clear()
        {
            owner.Clear();
        }

        /// <summary>
        /// <para>Copies the entire collection into an existing array at a specified location within the array.</para>
        /// </summary>
        /// <param name="destination"><para>The destination array.</para></param>
        /// <param name="index"><para>The index in the destination array at which storing begins.</para></param>
        /// <seealso cref="ICollection.CopyTo"/>
        public void CopyTo(ConfigurationNode[] destination, int index)
        {
            if (owner.ChildCount > 0)
            {
                Array.Copy(owner.ChildNodes, 0, destination, index, owner.ChildCount);
            }
        }

        /// <summary>
        /// <para>copies the elements of the <see cref="ICollection"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.</para>
        /// </summary>
        /// <param name="destination"><para>The destination array.</para></param>
        /// <param name="index"><para>The index in the destination array at which storing begins.</para></param>  
        /// <returns>
        /// <para>The zero-based index in array at which copying begins.</para>
        /// </returns>
        void ICollection.CopyTo(Array destination, int index)
        {
            CopyTo((ConfigurationNode[])destination, index);
        }

        /// <summary>
        /// <para>Removes the specified node from the collection.</para>
        /// </summary>
        /// <param name="node"><para>The <see cref="ConfigurationNode"/> to remove.</para></param>
        /// <seealso cref="IList.Remove"/>
        public void Remove(ConfigurationNode node)
        {
            node.RemoveNode(true);
        }

        /// <summary>
        /// <para>Removes the first occurrence of a specific object from the <see cref="IList"/>.</para>
        /// </summary>
        /// <param name="value">
        /// <para>The <see cref="Object"/> to remove from the <see cref="IList"/>.</para>
        /// </param>
        void IList.Remove(object value)
        {
            if (value is ConfigurationNode)
            {
                Remove((ConfigurationNode)value);
            }
        }

        /// <summary>
        /// <para>Removes the <see cref="ConfigurationNode"/> at the specified index from the collection.</para>
        /// </summary>
        /// <param name="index"><para>The index of the <see cref="ConfigurationNode"/> to remove.</para></param>
        public void RemoveAt(int index)
        {
            this[index].Remove();
        }

        /// <summary>
        /// <para>Returns an enumerator that can iterate through a collection.</para>
        /// </summary>        
        /// <returns>
        /// <para>An enumerator that can iterate through a collection.</para>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return new ConfigurationNodeCollectionEnumerator(owner.ChildNodes, owner.ChildCount);
        }

        private class ConfigurationNodeCollectionEnumerator : IEnumerator
        {
            private ConfigurationNode[] array;
            private int total;
            private int current;

            public ConfigurationNodeCollectionEnumerator(ConfigurationNode[] array, int count)
            {
                this.array = array;
                this.total = count;
                current = -1;
            }

            public object Current
            {
                get
                {
                    if (current == -1)
                    {
                        return null;
                    }
                    else
                    {
                        return array[current];
                    }
                }
            }

            public bool MoveNext()
            {
                if (current < total - 1)
                {
                    current++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                current = -1;
            }
        }
    }

}