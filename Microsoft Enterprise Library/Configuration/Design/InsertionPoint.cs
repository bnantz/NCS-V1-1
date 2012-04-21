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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Points on the main menu where <see cref="ConfigurationMenuItem"/> objets can be sited.</para>
    /// </summary>
    public enum InsertionPoint
    {
        /// <summary>
        /// <para>The item will become a menu item of the main task menu.</para>
        /// </summary>
        Action = 0,
        /// <summary>
        /// <para>The item will become a menu item of the main help menu.</para>
        /// </summary>
        Help = 1,
        /// <summary>
        /// <para>Will create a new menu item in the action menu item and add to it's submenu.</para>
        /// </summary>
        New = 2
    }
}