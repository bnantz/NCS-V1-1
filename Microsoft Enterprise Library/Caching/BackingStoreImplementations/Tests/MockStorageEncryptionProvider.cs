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

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests
{
    public class MockStorageEncryptionProvider : ConfigurationProvider, IStorageEncryptionProvider
    {
        public static bool Encrypted = false;
        public static bool Decrypted = false;

        public MockStorageEncryptionProvider()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
        }

        public byte[] Encrypt(byte[] plainText)
        {
            MockStorageEncryptionProvider.Encrypted = true;
            return plainText;
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            MockStorageEncryptionProvider.Decrypted = true;
            return cipherText;
        }
    }
}

#endif