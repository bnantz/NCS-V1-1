//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// A hash provider for any hash algorithm which derives from <see cref="System.Security.Cryptography.KeyedHashAlgorithm"/>.
    /// </summary>
    public class KeyedHashAlgorithmProvider : HashAlgorithmProvider
    {
        /// <summary>
        /// Gets the cryptographer used for hashing.
        /// </summary>
        /// <returns>The cryptographer initialized with the configured key.</returns>
        protected override HashCryptographer GetHashCryptographer()
        {
            HashProviderData hashProviderData = CryptoConfigurationView.GetHashProviderData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(hashProviderData, typeof(KeyedHashAlgorithmProviderData));
            
            KeyedHashAlgorithmProviderData keyedHashAlgorithmProviderData = (KeyedHashAlgorithmProviderData)hashProviderData;
            return new HashCryptographer(keyedHashAlgorithmProviderData.AlgorithmType, keyedHashAlgorithmProviderData.Key);
        }
    }
}