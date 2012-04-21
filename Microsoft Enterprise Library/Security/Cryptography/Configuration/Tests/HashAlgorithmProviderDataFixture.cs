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
    public class HashAlgorithmProviderDataFixture
    {
        [Test]
        public void Properties()
        {
            string algorithmType = "oeorihgr";

            HashAlgorithmProviderData data = new HashAlgorithmProviderData();
            data.AlgorithmType = algorithmType;
            data.SaltEnabled = true;

            Assert.AreEqual(algorithmType, data.AlgorithmType, "alg");
            Assert.AreEqual(true, data.SaltEnabled, "salt");

        }
    }
}

#endif