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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Protection.Tests
{
    [TestFixture]
    public class SymmetricAlgorithmKeyCreatorFixture
    {
        [Test]
        public void CreateKeyWithAES()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged).AssemblyQualifiedName);
            Assert.AreEqual(32, keyCreator.KeyLength);
            Assert.AreEqual(32, keyCreator.GenerateKey().Length);
            Assert.IsFalse((keyCreator.GenerateKey() == keyCreator.GenerateKey()));
        }

        [Test]
        [ExpectedException(typeof(TypeLoadException))]
        public void CreateKeyWithBadAlgorithmType()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator("bad type");
        }

        [Test]
        public void ValidKeys()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged).AssemblyQualifiedName);
            Assert.IsTrue(keyCreator.KeyIsValid(new byte[16]));
            Assert.IsTrue(keyCreator.KeyIsValid(new byte[24]));
            Assert.IsTrue(keyCreator.KeyIsValid(new byte[32]));
        }

        [Test]
        public void InvalidRangeKeys()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged).AssemblyQualifiedName);
            Assert.IsFalse(keyCreator.KeyIsValid(new byte[1]));
            Assert.IsFalse(keyCreator.KeyIsValid(new byte[25]));
            Assert.IsFalse(keyCreator.KeyIsValid(new byte[129]));
        }
    }
}

#endif