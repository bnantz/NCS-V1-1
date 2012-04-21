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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>
    /// Represents a storage provider writer for configuration data.
    /// </para>
    /// </summary>
    public interface IStorageProviderWriter : IStorageProviderReader
    {
        /// <summary>
        /// When implemented by a class, writes the configuration data to storage.
        /// </summary>
        /// <param name="value">
        /// <para>The value to write to storage.</para>
        /// </param>
        void Write(object value);
    }
}