//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration.Tests
{
    [TestFixture]
    public class FormatterFixture
    {
        [Test]
        public void BasePropetiesTest()
        {
            CustomFormatterData data = new CustomFormatterData();
            string typeName = "Test Type";
            string name = "Test Name";

            data.TypeName = typeName;
            data.Name = name;

            Assert.AreEqual(typeName, data.TypeName);
            Assert.AreEqual(name, data.Name);
        }

        [Test]
        public void CustomFormatterDataPropertiesTest()
        {
            CustomFormatterData data = new CustomFormatterData();
            NameValueItemCollection attributes = new NameValueItemCollection();
            attributes.Add(new NameValueItem("TEST", "test"));

            data.Attributes.Add(attributes[0]);

            Assert.AreEqual(attributes[0], data.Attributes[0]);
        }
    }
}

#endif