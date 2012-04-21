//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="CacheManagerData"/> settings.
    /// </summary>
    [Serializable]
    public class CacheManagerDataCollection : ProviderDataCollection
    {
        /// <summary>
        /// <para>Gets or sets the <see cref="CacheManagerData"/> at the specified <paramref name="index"/>.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index of the <see cref="CacheManagerData"/> to get or set.</para>
        /// </param>
        /// <para>The value associated with the specified <paramref name="index"/>. If the specified <paramref name="index"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="index"/>.</para>
        public CacheManagerData this[int index]
        {
            get { return (CacheManagerData)GetProvider(index); }
            set { SetProvider(index, value); }
        }

        /// <summary>
        /// Indexer to retrieve a named <see cref="CacheManagerData"/>.
        /// </summary>        
        /// <param name="name">Name of CacheManagerData to retrieve</param>
        public CacheManagerData this[string name]
        {
            get { return (CacheManagerData)base.GetProvider(name); }
            set { base.SetProvider(name, value); }
        }

        /// <summary>
        /// Add a new <see cref="CacheManagerData"/> to the collection.
        /// </summary>
        /// <param name="data">Distribution strategy to add.</param>
        public void Add(CacheManagerData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("cacheManager");
            }
            if (data.Name == null)
            {
                throw new InvalidOperationException(SR.ExceptionNullCacheManagerDataName);
            }
            base.AddProvider(data);
        }

        /// <summary>
        /// Adds the items in the specified collection to the current collection.
        /// </summary>
        /// <param name="collection">A <see cref="CacheManagerDataCollection"/>.</param>
        public void AddRange(CacheManagerDataCollection collection)
        {
            base.AddProviders(collection);
        }

        /// <summary>
        /// Copies the elements of the <see cref="CacheManagerDataCollection"/>
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="CacheManagerDataCollection"/>.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param>
        public void CopyTo(CacheManagerData[] array, int index)
        {
            base.CopyTo((Array)array, index);
        }
    }
}