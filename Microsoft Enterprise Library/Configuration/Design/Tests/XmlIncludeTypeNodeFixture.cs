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
    public class XmlIncludeTypeNodeFixture : ConfigurationDesignHostTestBase
    {
        private XmlIncludeTypeNode node;
        private XmlIncludeTypeData data;

        public override void SetUp()
        {
            base.SetUp();
            data = new XmlIncludeTypeData("string", "System.String, mscorlib");
            node = new XmlIncludeTypeNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
        }

        [Test]
        public void CreateDefaultTest()
        {
            XmlIncludeTypeNode xmlIncludeTypeNode = new XmlIncludeTypeNode();
            CreateHierarchyAndAddToHierarchyService(xmlIncludeTypeNode, CreateDefaultConfiguration());
            Assert.AreEqual(SR.DefaultXmlIncludeTypeNodeName, xmlIncludeTypeNode.Name);
            Assert.AreEqual(SR.DefaultXmlIncludeTypeNodeName, xmlIncludeTypeNode.XmlIncludeTypeData.Name);
        }

        [Test]
        public void CreateNameTest()
        {
            string name = "MyName";
            XmlIncludeTypeNode xmlIncludeTypeNode = new XmlIncludeTypeNode(new XmlIncludeTypeData(name));
            CreateHierarchyAndAddToHierarchyService(xmlIncludeTypeNode, CreateDefaultConfiguration());
            Assert.AreEqual(name, xmlIncludeTypeNode.Name);
            Assert.AreEqual(name, xmlIncludeTypeNode.XmlIncludeTypeData.Name);
        }

        [Test]
        public void TypeNameTest()
        {
            Assert.AreEqual(data.TypeName, node.TypeName);
        }

        [Test]
        public void XmlIncludeTypeDataTest()
        {
            Assert.AreSame(data, node.XmlIncludeTypeData);
        }

        [Test]
        public void ChangeDisplayNameShouldChangeNameTest()
        {
            string name = "Name";
            XmlIncludeTypeNode xmlIncludeTypeNode = new XmlIncludeTypeNode(new XmlIncludeTypeData(name));
            CreateHierarchyAndAddToHierarchyService(xmlIncludeTypeNode, CreateDefaultConfiguration());
            Assert.AreEqual(xmlIncludeTypeNode.Name, xmlIncludeTypeNode.Name);
            xmlIncludeTypeNode.Name = "newName1";
            Assert.AreEqual(xmlIncludeTypeNode.Name, xmlIncludeTypeNode.Name);
            xmlIncludeTypeNode.Name = "newName2";
            Assert.AreEqual(xmlIncludeTypeNode.Name, xmlIncludeTypeNode.Name);
        }
    }
}

#endif