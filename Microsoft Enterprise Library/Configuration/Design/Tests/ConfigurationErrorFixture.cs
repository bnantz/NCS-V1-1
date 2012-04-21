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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationErrorFixture
    {
        private ConfigurationError error;
        private ConfigurationNode node;
        private string message;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            node = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            message = "Test";
            error = new ConfigurationError(node, message);
        }

        [Test]
        public void NodeTest()
        {
            Assert.AreSame(node, error.ConfigurationNode);
        }

        [Test]
        public void MessageTest()
        {
            Assert.AreEqual(message, error.Message);
        }
    }
}

#endif