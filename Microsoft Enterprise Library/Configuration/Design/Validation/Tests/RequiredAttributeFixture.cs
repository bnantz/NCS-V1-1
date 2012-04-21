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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestFixture]
    public class RequiredAttributeFixture : ConfigurationDesignHostTestBase
    {
        private MyRequiredTestNode requiredNode;
        private PropertyInfo valueInfo1;
        private PropertyInfo valueInfo2;

        public override void SetUp()
        {
            base.SetUp();
            requiredNode = new MyRequiredTestNode();
            valueInfo1 = requiredNode.GetType().GetProperty("Value1");
            valueInfo2 = requiredNode.GetType().GetProperty("Value2");
            CreateHierarchyAndAddToHierarchyService(requiredNode, CreateDefaultConfiguration());
        }

        [Test]
        public void RequiredNotThereTest()
        {
            RequiredAttribute attribute = new RequiredAttribute();
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(requiredNode, valueInfo1, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void RequiredValueTest()
        {
            RequiredAttribute attribute = new RequiredAttribute();
            requiredNode.Value2 = "MyTest";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(requiredNode, valueInfo2, errors);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void RequiredNotThereTestWithVistor()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            Assert.IsNotNull(requiredNode.Site);
            cmd.Execute(requiredNode);
            Assert.AreEqual(2, ValidationErrorsCount);
        }

        [Test]
        public void RequiredValueTestWithVistor()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            Assert.IsNotNull(requiredNode.Site);
            requiredNode.Value1 = "aaa";
            requiredNode.Value2 = "aaaaaa";
            cmd.Execute(requiredNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        private class MyRequiredTestNode : ConfigurationNode
        {
            private string value1;
            private string value2;

            public MyRequiredTestNode() : base()
            {
            }

            [Required]
            public string Value1
            {
                get { return value1; }
                set { value1 = value; }
            }

            [Required]
            public string Value2
            {
                get { return value2; }
                set { value2 = value; }
            }
        }
    }
}

#endif