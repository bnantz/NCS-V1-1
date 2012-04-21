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
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection.Tests
{
    [TestFixture]
    public class KeyInfoFixture
    {
        [Test]
        public void Properties()
        {
            KeySettings settings = new KeySettings(new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged).AssemblyQualifiedName));
            settings.Key = settings.KeyCreator.GenerateKey();
            Assert.AreEqual(32, settings.Key.Length);
        }

        [Test]
        public void Clone()
        {
            SymmetricAlgorithmKeyCreator keyCreator = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged).AssemblyQualifiedName);
            byte[] key = keyCreator.GenerateKey();

            KeySettings settings = new KeySettings(keyCreator);
            settings.Key = key;
            KeySettings settingsClone = settings.Clone();

            Assert.AreSame(keyCreator, settingsClone.KeyCreator);
            Assert.AreEqual(key, settingsClone.Key);
        }
    }
}

#endif