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

using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <devdoc>
    /// Represents a collection of provider objects in configuration. This is used internally when some one only has one provider.
    /// </devdoc>       
    internal class CustomProviderDataCollection : ProviderDataCollection
    {
        public CustomProviderDataCollection()
        {
        }

        /// <summary>
        /// <para>Gets or sets the ProviderData at the specified index.</para>
        /// </summary>
        public ProviderData this[int index]
        {
            get { return GetProvider(index); }
            set { SetProvider(index, value); }
        }

        /// <devdoc>
        /// Gets or sets the ProviderData associated with the specified name
        /// </devdoc>
        public ProviderData this[string name]
        {
            get { return base.GetProvider(name); }
            set { base.SetProvider(name, value); }
        }

        /// <devdoc>
        /// Adds an ProviderData into the collection.
        /// </devdoc>
        public void Add(ProviderData providerData)
        {
            base.AddProvider(providerData);
        }

        /// <devdoc>
        /// Adds an ProviderData into the collection with a specific name.
        /// </devdoc>
        public void Add(string name, ProviderData providerData)
        {
            base.AddProvider(name, providerData);
        }

        /// <summary>
        /// Copies the entire CustomProviderDataCollection to a compatible one-dimensional ProviderData array, starting at the specified index of the target array.
        /// </summary>
        public void CopyTo(ProviderData[] array, int index)
        {
            for (IEnumerator e = this.GetEnumerator(); e.MoveNext(); )
            {
                array.SetValue(e.Current, index++);
            }
        }
    }
}