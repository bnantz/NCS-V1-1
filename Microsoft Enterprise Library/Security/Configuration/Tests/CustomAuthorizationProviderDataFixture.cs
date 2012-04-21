//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Tests
{
    [TestFixture]
    public class CustomAuthorizationProviderDataFixture
    {
        [Test]
        public void SerializationTest()
        {
            Utility.SerializationTest(
                new CustomAuthorizationProviderData());
            Utility.SerializationTest(
                typeof(AuthorizationProviderData),
                new CustomAuthorizationProviderData());
        }

        [Test]
        public void ExtensionsTest()
        {
            CustomAuthorizationProviderData data = new CustomAuthorizationProviderData();
            data.Extensions.Add(new NameValueItem("key", "value"));
            string value = data.Extensions["key"];
            Assert.AreEqual("value", value);
            Assert.AreEqual(1, data.Extensions.Count);
        }
    }
}

#endif