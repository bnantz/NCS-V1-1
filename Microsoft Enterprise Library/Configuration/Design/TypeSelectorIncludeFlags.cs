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
    /// <para>Provides attributes for the filter of types.</para>
    /// </summary>
    [Flags]
    public enum TypeSelectorIncludeFlags
    {
        /// <summary>
        /// <para>No filter are applied to types.</para>
        /// </summary>
        Default = 0x00,
        /// <summary>
        /// <para>Inclue abstract types in the filter.</para>
        /// </summary>
        AbstractTypes = 0x01,
        /// <summary>
        /// <para>Inclue interfaces in the filter.</para>
        /// </summary>
        Interfaces = 0x02,
        /// <summary>
        /// <para>Inclue base types in the filter.</para>
        /// </summary>
        BaseType = 0x04,
        /// <summary>
        /// <para>Inclue non public types in the filter.</para>
        /// </summary>
        NonpublicTypes = 0x08
    }
}