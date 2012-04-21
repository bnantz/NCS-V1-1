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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Tests
{
    [TestFixture]
    public class SymmetricAlgorithmProviderDataFixture
    {
        [Test]
        public void Properties()
        {
            string algorithmType = "e59y8ue";
            byte[] key = new byte[] {1, 2, 3, 4};

            SymmetricAlgorithmProviderData data = new SymmetricAlgorithmProviderData();
            data.AlgorithmType = algorithmType;
            data.Key = key;

            Assert.AreEqual(key, data.Key);
            Assert.AreEqual(algorithmType, data.AlgorithmType, "algorithm");
        }

    }
}

#endif