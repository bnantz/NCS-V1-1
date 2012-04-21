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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Tests
{
    [TestFixture]
    public class DpapiSymmetricCryptoProviderDataFixture
    {
        [Test]
        public void Properties()
        {
            string name = "name";
            DpapiStorageMode mode = DpapiStorageMode.User;
            byte[] machineEntropy = new byte[16];
            CryptographyUtility.GetRandomBytes(machineEntropy);

            DpapiSymmetricCryptoProviderData data = new DpapiSymmetricCryptoProviderData();
            data.Name = name;

            Assert.AreEqual(name, data.Name, "name");
            Assert.AreEqual(typeof(DpapiSymmetricCryptoProvider).AssemblyQualifiedName, data.TypeName, "type");

            DpapiSettingsData dpapiData = new DpapiSettingsData();
            dpapiData.Mode = mode;
            dpapiData.Entropy = machineEntropy;
            data.DataProtectionMode = dpapiData;
            Assert.AreEqual(mode, data.DataProtectionMode.Mode, "Mode");
            Assert.IsTrue(CryptographyUtility.CompareBytes(machineEntropy, dpapiData.Entropy), "Entropy");
        }
    }
}

#endif