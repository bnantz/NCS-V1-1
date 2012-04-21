//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestFixture]
    public class ExceptionHandlerNodeFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void CustomHandlerPropertiesTest()
        {
            string typeName = "testType";
            NameValueItemCollection attributes = new NameValueItemCollection();
            attributes.Add(new NameValueItem("NAME", "VALUE"));

            CustomHandlerNode node = new CustomHandlerNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            node.TypeName = typeName;
            node.Attributes.Add(attributes[0]);

            Assert.AreEqual(typeName, node.TypeName);
            Assert.AreEqual(attributes["NAME"], node.Attributes["NAME"]);
        }

        [Test]
        public void CustomHandlerGetDataTest()
        {
            CustomHandlerData data = new CustomHandlerData();
            string typeName = "TestType";
            string name = "TestName";
            NameValueItemCollection attributes = new NameValueItemCollection();
            attributes.Add(new NameValueItem("NAME", "VALUE"));

            data.TypeName = typeName;
            data.Name = name;
            data.Attributes.Add(attributes[0]);

            CustomHandlerNode node = new CustomHandlerNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            CustomHandlerData nodeData = (CustomHandlerData) node.ExceptionHandlerData;

            Assert.AreEqual(typeName, nodeData.TypeName);
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(attributes["NAME"], nodeData.Attributes["NAME"]);
        }

        [Test]
        public void WrapHandlerPropertiesTest()
        {
            WrapHandlerNode node = new WrapHandlerNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            string exceptionMessage = "test message";
            string wrapExceptionTypeName = "System.ArgumentException, mscorlib, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

            node.ExceptionMessage = exceptionMessage;
            node.WrapExceptionTypeName = wrapExceptionTypeName;

            Assert.AreEqual(exceptionMessage, node.ExceptionMessage);
            Assert.AreEqual(wrapExceptionTypeName, node.WrapExceptionTypeName);
        }

        [Test]
        public void WrapHandlerGetDataTest()
        {
            WrapHandlerData data = new WrapHandlerData();
            string wrapExceptionTypeName = "System.ArgumentException, mscorlib, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            string name = "WrapHandler";

            data.WrapExceptionTypeName = wrapExceptionTypeName;
            data.Name = name;

            WrapHandlerNode node = new WrapHandlerNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            WrapHandlerData nodeData = (WrapHandlerData) node.ExceptionHandlerData;

            Assert.IsNull(nodeData.ExceptionMessage);
            Assert.AreEqual(wrapExceptionTypeName, nodeData.WrapExceptionTypeName);
            Assert.AreEqual(name, nodeData.Name);
        }

        [Test]
        public void ReplaceHandlerPropertiesTest()
        {
            ReplaceHandlerNode node = new ReplaceHandlerNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            string exceptionMessage = "testMessage";
            string replaceExceptionTypeName = "TestType";

            node.ExceptionMessage = exceptionMessage;
            node.ReplaceExceptionTypeName = replaceExceptionTypeName;

            Assert.AreEqual(exceptionMessage, node.ExceptionMessage);
            Assert.AreEqual(replaceExceptionTypeName, node.ReplaceExceptionTypeName);
        }

        [Test]
        public void ReplaceHandlerGetDataTest()
        {
            ReplaceHandlerData data = new ReplaceHandlerData();
            string replaceExceptionTypeName = "TestType";
            string name = "ReplaceHandler";

            data.ReplaceExceptionTypeName = replaceExceptionTypeName;
            data.Name = name;

            ReplaceHandlerNode node = new ReplaceHandlerNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            ReplaceHandlerData nodeData = (ReplaceHandlerData) node.ExceptionHandlerData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(replaceExceptionTypeName, nodeData.ReplaceExceptionTypeName);
        }
    }
}

#endif