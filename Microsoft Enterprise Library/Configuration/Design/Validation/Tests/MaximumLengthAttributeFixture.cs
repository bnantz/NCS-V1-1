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
using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestFixture]
    public class MaximumLengthAttributeFixture : ConfigurationDesignHostTestBase
    {
        private MyMaxLengthTestNode maxLengthNode;
        private PropertyInfo valueInfo1;
        private PropertyInfo valueInfo2;

        public override void SetUp()
        {
            base.SetUp();
            maxLengthNode = new MyMaxLengthTestNode();
            valueInfo1 = maxLengthNode.GetType().GetProperty("Value1");
            valueInfo2 = maxLengthNode.GetType().GetProperty("Value2");
            CreateHierarchyAndAddToHierarchyService(maxLengthNode, CreateDefaultConfiguration());
        }

        [Test]
        public void MaxLengthViolationTest()
        {
            MaximumLengthAttribute attribute = new MaximumLengthAttribute(3);
            maxLengthNode.Value1 = "aaaa";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(maxLengthNode, valueInfo1, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void MaxLengthTest()
        {
            MaximumLengthAttribute attribute = new MaximumLengthAttribute(8);
            maxLengthNode.Value2 = "aaaa";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(maxLengthNode, valueInfo2, errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void MaxLengthViolationTestWithCommand()
        {
            maxLengthNode.Value1 = "MyTest";
            maxLengthNode.Value2 = "MyTestPassword";
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(maxLengthNode);
            Assert.IsNotNull(maxLengthNode.Site);
            Assert.AreEqual(2, ValidationErrorsCount);
        }

        [Test]
        public void MaxLengthTestWithCommand()
        {
            maxLengthNode.Value1 = "aaa";
            maxLengthNode.Value2 = "aaaaaa";
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(maxLengthNode);
            Assert.IsNotNull(maxLengthNode.Site);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        private class MyMaxLengthTestNode : ConfigurationNode
        {
            private string value1;
            private string value2;

            public MyMaxLengthTestNode() : base()
            {
            }

            [MaximumLength(3)]
            public string Value1
            {
                get { return value1; }
                set { value1 = value; }
            }

            [MaximumLength(8)]
            public string Value2
            {
                get { return value2; }
                set { value2 = value; }
            }
        }
    }
}

#endif