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
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Tests
{
    [TestFixture]
    public class CryptographySettingsFixture 
    {

        [Test]
        public void Properties()
        {
            Guid appID = Guid.NewGuid();
            CryptographySettings settings = new CryptographySettings();

            KeyedHashAlgorithmProviderData hashData = new KeyedHashAlgorithmProviderData();
            hashData.Name = "name";
            settings.HashProviders.Add(hashData);
            Assert.AreEqual(1, settings.HashProviders.Count, "hash collection");

            DpapiSymmetricCryptoProviderData symmData = new DpapiSymmetricCryptoProviderData();
            symmData.Name = "name";
            settings.SymmetricCryptoProviders.Add(symmData);
            Assert.AreEqual(1, settings.SymmetricCryptoProviders.Count, "symmetric collection");
        }
    }
}

#endif