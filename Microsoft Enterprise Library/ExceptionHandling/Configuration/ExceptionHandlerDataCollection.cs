//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="ExceptionHandlerData"/>s.
    /// </summary>
    [Serializable]
    public class ExceptionHandlerDataCollection : ProviderDataCollection
    {
        /// <summary>
        /// Gets an <see cref="ExceptionHandlerData"/> at the defined index.
        /// </summary>
        /// <param name="index">Index for given element in the collection</param>
        public ExceptionHandlerData this[int index]
        {
            get { return (ExceptionHandlerData)GetProvider(index); }
            set { SetProvider(index, value); }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="ExceptionHandlerData "/> associated with the specified <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="ExceptionHandlerData "/> to get or set.</para>
        /// </param>
        /// <value>
        /// <para>The value associated with the specified <paramref name="name"/>. If the specified <paramref name="name"/> is not found, attempting to get it returns a <see langword="null"/> reference (Nothing in Visual Basic), and attempting to set it creates a new entry using the specified <paramref name="name"/>.</para>
        /// </value>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="name"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public ExceptionHandlerData this[string name]
        {
            get
            {
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                return BaseGet(name) as ExceptionHandlerData;
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
        /// Adds an <see cref="ExceptionHandlerData"/> to the collection.
        /// </summary>
        /// <param name="exceptionHandlerData">The instance to add to the collection.</param>
        public void Add(ExceptionHandlerData exceptionHandlerData)
        {
            AddProvider(exceptionHandlerData);
        }
    }
}