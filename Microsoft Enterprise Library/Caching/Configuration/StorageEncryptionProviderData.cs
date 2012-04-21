//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Base class for configuration data defined for all types of StorageEncryptionProviders
    /// </summary>
    [XmlRoot("storageEncryption", Namespace=CacheManagerSettings.ConfigurationNamespace)]
    public abstract class StorageEncryptionProviderData : ProviderData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        protected StorageEncryptionProviderData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="StorageEncryptionProviderData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="StorageEncryptionProviderData"/>.
        /// </param>
        protected StorageEncryptionProviderData(string name) : base(name)
        {
        }
    }
}