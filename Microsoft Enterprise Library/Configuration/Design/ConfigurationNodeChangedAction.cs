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
    /// <para>Specifies the type of node change.</para>
    /// </summary>
    public enum ConfigurationNodeChangedAction
    {
        /// <summary>
        /// <para>A node is being inserted.</para>
        /// </summary>
        Insert = 0,
        /// <summary>
        /// <para>A node is being removed from the tree.</para>
        /// </summary>
        Remove = 1,
        /// <summary>
        /// <para>A node is being renamed from the tree.</para>
        /// </summary>
        Rename = 2,
        /// <summary>
        /// <para>A node value is being changed.</para>
        /// </summary>
        Changed = 3,
        /// <summary>
        /// <para>A custom action.</para>
        /// </summary>
        Custom = 4
    }
}