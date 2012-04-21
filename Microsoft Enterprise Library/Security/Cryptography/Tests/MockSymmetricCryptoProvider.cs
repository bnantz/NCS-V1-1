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

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    public class MockSymmetricCryptoProvider : ConfigurationProvider, ISymmetricCryptoProvider
    {
        public bool Initialized = false;

        public MockSymmetricCryptoProvider()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
            this.Initialized = true;
        }

        public byte[] Encrypt(byte[] plainText)
        {
            return null;
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            return null;
        }
    }
}

#endif