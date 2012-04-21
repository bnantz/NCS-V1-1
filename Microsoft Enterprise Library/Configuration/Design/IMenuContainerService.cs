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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Provides support for adding menu items to the main menu.</para>.
    /// </summary>
    public interface IMenuContainerService
    {
        /// <summary>
        /// <para>When implemented by a class, gets the list of menu items for all external callers.</para>
        /// </summary>
        /// <value>
        /// <para>The list of menu items for all external callers.</para>
        /// </value>
        IList MenuItems { get; }
    }
}