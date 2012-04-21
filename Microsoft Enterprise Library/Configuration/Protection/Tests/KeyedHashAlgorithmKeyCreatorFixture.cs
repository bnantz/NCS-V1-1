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
using System;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection.Tests
{
    [TestFixture]
    public class KeyedHashAlgorithmKeyCreatorFixture
    {
        [Test]
        public void CreateHMACSHA1Key()
        {
            KeyedHashAlgorithmKeyCreator keyCreator = new KeyedHashAlgorithmKeyCreator(typeof(HMACSHA1).AssemblyQualifiedName);
            Assert.AreEqual(64, keyCreator.KeyLength);
            Assert.AreEqual(64, keyCreator.GenerateKey().Length);
            Assert.IsFalse(CryptographyUtility.CompareBytes(keyCreator.GenerateKey(), keyCreator.GenerateKey()));
        }

        [Test]
        [ExpectedException(typeof(TypeLoadException))]
        public void CreateWithBadType()
        {
            new KeyedHashAlgorithmKeyCreator("Bad Type");
        }
    }
}

#endif