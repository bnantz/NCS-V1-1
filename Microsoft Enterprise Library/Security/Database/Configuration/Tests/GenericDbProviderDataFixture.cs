//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Configuration.Tests
{
    [TestFixture]
    public class GenericDbProviderDataFixture
    {
        [Test]
        public void Properties()
        {
            DbProfileProviderData data = new DbProfileProviderData();
            data.Database = "test";

            Assert.AreEqual("test", data.Database);
        }

        [Test]
        public void Constructor()
        {
            DbProfileProviderData data = new DbProfileProviderData("name", "test");
            Assert.AreEqual("test", data.Database);
        }
    }
}

#endif