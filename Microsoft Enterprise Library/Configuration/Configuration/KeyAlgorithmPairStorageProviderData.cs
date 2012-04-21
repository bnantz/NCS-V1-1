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
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a storage provider to read and write data for a <see cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Protection.KeyAlgorithmPair"/>.</para>
    /// </summary>
    /// <remarks>
    /// <para>The class maps to the <c>keyAlgorithmStorageProvider</c> element in configuration.</para>
    /// </remarks>
    [XmlRoot("keyAlgorithmStorageProvider", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    [XmlInclude(typeof(FileKeyAlgorithmPairStorageProviderData))]
    [XmlInclude(typeof(CustomKeyAlgorithmPairStorageProviderData))]
    [Serializable]
    public abstract class KeyAlgorithmPairStorageProviderData : ProviderData, ICloneable
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="KeyAlgorithmPairStorageProviderData"/> class..</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the storage provider.</para>
        /// </param>
        protected KeyAlgorithmPairStorageProviderData(string name) : base(name)
        {
        }

        /// <summary>
        /// <para>Creates a new object that is a copy of the current instance.</para>
        /// </summary>
        /// <returns>
        /// <para>A new object that is a copy of this instance.</para>
        /// </returns>
        public abstract object Clone();
    }
}