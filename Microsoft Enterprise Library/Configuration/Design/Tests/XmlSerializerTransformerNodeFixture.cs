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

#if       UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class XmlSerializerTransformerNodeFixture : ConfigurationDesignHostTestBase
    {
        private XmlSerializerTransformerNode node;
        private XmlSerializerTransformerData data;

        public override void SetUp()
        {
            base.SetUp();
            data = new XmlSerializerTransformerData("test");
            node = new XmlSerializerTransformerNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(data.Name, node.Name);
        }

        [Test]
        public void TypeNameTest()
        {
            Assert.AreEqual(data.TypeName, node.TypeName);
        }

        [Test]
        public void StorageProviderDataTest()
        {
            Assert.AreSame(data, node.TransformerData);
        }
    }
}

#endif