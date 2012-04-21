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
    public class DpapiSettingsDataFixture
    {
        [Test]
        public void Properties()
        {
            DpapiStorageMode mode = DpapiStorageMode.Machine;
            byte[] entropy = new byte[] {0, 1, 2, 3, 4};

            DpapiSettingsData data = new DpapiSettingsData();
            data.Mode = mode;
            data.Entropy = entropy;

            Assert.AreEqual(mode, data.Mode);
            Assert.AreEqual(entropy, data.Entropy);
        }
    }
}

#endif