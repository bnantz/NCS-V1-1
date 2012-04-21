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
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Tests
{
    [TestFixture]
    public class KeyHashAlgorithmProviderDataFixture
    {
        [Test]
        public void Properties()
        {
            byte[] key = new byte[] {0, 1, 2, 3, 4, 5, 6};
            KeyedHashAlgorithmProviderData data = new KeyedHashAlgorithmProviderData();
            data.Key = key;
            Assert.IsTrue(CryptographyUtility.CompareBytes(key, data.Key));
        }
    }
}

#endif