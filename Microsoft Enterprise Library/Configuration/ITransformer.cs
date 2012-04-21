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
    /// Represents a standard interface for transforming configuration data coming from and to storage.
    /// </para>
    /// </summary>
    public interface ITransformer : IConfigurationProvider
    {
        /// <summary>
        /// <para>Gets the name of the configuration section.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the configuration section.</para>
        /// </value>
        string CurrentSectionName { get; set; }

        /// <summary>
        /// <para>
        /// When implemented by a class, deserializes the configuration data coming from storage.
        /// </para>
        /// </summary>
        /// <param name="section">
        /// <para>The data that came from storage.</para>
        /// </param>
        /// <returns>
        /// <para>An object that can be consumed by the calling assembly that wants configuration data.</para>
        /// </returns>
        object Deserialize(object section);

        /// <summary>
        /// <para>
        /// When implemented by a class, serializes the configuration data coming from the calling assembly and maps it into something that the storage provider can understand.
        /// </para>
        /// </summary>
        /// <param name="value">
        /// <para>The data to serialize.</para>
        /// </param>
        /// <returns>
        /// <para>The object that can be consumed by the storage provider.</para>
        /// </returns>
        object Serialize(object value);
    }
}