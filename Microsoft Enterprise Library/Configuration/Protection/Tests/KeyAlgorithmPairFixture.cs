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

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection.Tests
{
    [TestFixture]
    public class KeyAlgorithmPairFixture
    {
        [Test]
        public void TestConstruction()
        {
            byte[] bytes = new byte[] {1, 2, 3, 4};
            using (KeyAlgorithmPair pair = new KeyAlgorithmPair(bytes, "foo"))
            {
                Assert.AreEqual("foo", pair.AlgorithmTypeName);
                Assert.IsTrue(CryptographyUtility.CompareBytes(bytes, pair.Key));
            }
        }
    }
}

#endif