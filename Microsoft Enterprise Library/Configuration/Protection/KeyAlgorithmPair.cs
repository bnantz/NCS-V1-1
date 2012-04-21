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
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection
{
    /// <summary>
    /// <para>Represents the key algrithm pair for encrypting configuration sections.</para>
    /// </summary>
    [Serializable]
    public class KeyAlgorithmPair : IDisposable
    {
        private byte[] key;
        private string algorithmTypeName;

        /// <summary>
        /// <para>Instantiate a new instance of the <see cref="KeyAlgorithmPair"/> class.</para>
        /// </summary>
        public KeyAlgorithmPair() : this(null, string.Empty)
        {
        }

        /// <summary>
        /// <para>Instantiate a new instance of the <see cref="KeyAlgorithmPair"/> class with a key and algorithm type name.</para>
        /// </summary>
        /// <param name="key">
        /// <para>The key for the algorithm.</para>
        /// </param>
        /// <param name="algorithmTypeName">
        /// <para>The fully qualified type name for the algorithm.</para>
        /// </param>
        public KeyAlgorithmPair(byte[] key, string algorithmTypeName)
        {
            this.key = key;
            this.algorithmTypeName = algorithmTypeName;
        }

        /// <summary>
        /// <para>Allows an the <see cref="KeyAlgorithmPair"/> to attempt to free resources and perform other cleanup operations before the <see cref="KeyAlgorithmPair"/> is reclaimed by garbage collection. </para>
        /// </summary>
        ~KeyAlgorithmPair()
        {
            Dispose(false);
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="KeyAlgorithmPair"/> and optionally releases the managed resources.</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="KeyAlgorithmPair"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.key != null)
                {
                    CryptographyUtility.ZeroOutBytes(this.key);
                }
            }
        }

        /// <summary>
        /// <para>Gets or sets the key for the pair.</para>
        /// </summary>
        /// <value>
        /// <para>The key for the pair.</para>
        /// </value>
        public byte[] Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// <para>Gets or sets the algorithm type name.</para>
        /// </summary>
        /// <value>
        /// <para>The algorithm type name.</para>
        /// </value>
        public string AlgorithmTypeName
        {
            get { return algorithmTypeName; }
            set { algorithmTypeName = value; }
        }
    }
}
