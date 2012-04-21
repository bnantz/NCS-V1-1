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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection
{
    /// <summary>
    /// <para>Represents storage for a <see cref="KeyAlgorithmPair"/>.</para>
    /// </summary>
    public interface IKeyAlgorithmPairStorageProvider : IConfigurationProvider
    {
        /// <summary>
        /// <para>When implemented by a class, loads the <see cref="KeyAlgorithmPair"/> from storage.</para>
        /// </summary>
        /// <returns>
        /// <para>Loads the <see cref="KeyAlgorithmPair"/> from storage.</para>
        /// </returns>
        KeyAlgorithmPair Load();

        /// <summary>
        /// <para>When implemented by a class, saves the <see cref="KeyAlgorithmPair"/> to storage.</para>
        /// </summary>
        /// <param name="keyAlgorithmPair">
        /// <para>The <see cref="KeyAlgorithmPair"/> to store.</para>
        /// </param>
        void Save(KeyAlgorithmPair keyAlgorithmPair);
    }
}