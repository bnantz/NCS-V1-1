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
using System.ComponentModel.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class IDictionaryServiceFixture
    {
        private IDictionaryService service;
        private ConfigurationDesignHost host;
        private static readonly string myKey = "key";
        private static readonly string myValue = "value";

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            host = new ConfigurationDesignHost();
            service = host.GetService(typeof(IDictionaryService)) as IDictionaryService;
        }

        [Test]
        public void SetValueAndMakeSureCanGet()
        {
            service.SetValue(myKey, myValue);
            object value = service.GetValue(myKey);
            Assert.AreSame(myValue, value);
            object key = service.GetKey(myValue);
            Assert.AreSame(myKey, key);
        }
    }
}

#endif