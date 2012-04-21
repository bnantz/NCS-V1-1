//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
    [TestFixture]
    public class SensitiveParameterNodeFixture
    {
        [Test]
        public void MaskedValue()
        {
            string maskedValue = "erotueur";
            SensitiveParameterNode node = new SensitiveParameterNode();
            node.MaskedValue = new Password(maskedValue);
            Assert.AreEqual(maskedValue, node.MaskedValue.PasswordText);
            ParameterData nodeData = node.Parameter;
            Assert.AreEqual(maskedValue, nodeData.Value);
        }

        [Test]
        public void MaskedValueData()
        {
            string maskedValue = "erotueur";
            ParameterData data = new ParameterData("testName", maskedValue);
            SensitiveParameterNode node = new SensitiveParameterNode(data);
            Assert.AreEqual(maskedValue, node.MaskedValue.PasswordText);
        }
    }
}

#endif