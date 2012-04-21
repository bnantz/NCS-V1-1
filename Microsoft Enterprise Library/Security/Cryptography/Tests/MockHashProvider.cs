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
    public class MockHashProvider : ConfigurationProvider, IHashProvider
    {
        public bool Initialized = false;

        public MockHashProvider()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
            this.Initialized = true;
        }

        public byte[] CreateHash(byte[] plainText)
        {
            return null;
        }

        public bool CompareHash(byte[] plainText, byte[] hashedText)
        {
            return false;
        }
    }
}

#endif