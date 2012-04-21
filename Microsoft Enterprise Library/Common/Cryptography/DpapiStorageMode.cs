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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Cryptography
{
    /// <summary>
    /// <para>Specifies the key store used by DPAPI.</para>
    /// </summary>
    public enum DpapiStorageMode
    {
        /// <summary>
        /// No storage mode specified. Should never happen or be used
        /// </summary>
        None = 0,

        /// <summary>
        /// <para>Store the key  using the machine account.</para>
        /// </summary>
        Machine = 1,

        /// <summary>
        /// <para>No storage selected. This value represents that we are not using DPAPI.</para>
        /// </summary>
        User = 2
    }
}