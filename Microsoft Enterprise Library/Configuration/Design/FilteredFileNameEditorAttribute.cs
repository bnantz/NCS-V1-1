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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Specifies the properties for the <see cref="FilteredFileNameEditor"/> to use to change a property.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FilteredFileNameEditorAttribute : Attribute
    {
        private string filter;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FilteredFileNameEditorAttribute"/> class.</para>
        /// </summary>
        /// <param name="filter">
        /// <para>The filter for the file names.</para>
        /// </param>
        public FilteredFileNameEditorAttribute(string filter)
        {
            this.filter = filter;
        }

        /// <summary>
        /// <para>Gets the filter for the dialog.</para>
        /// </summary>
        /// <value>
        /// <para>The filter for the dialog.</para>
        /// </value>
        public string Filter
        {
            get { return this.filter; }
        }
    }
}