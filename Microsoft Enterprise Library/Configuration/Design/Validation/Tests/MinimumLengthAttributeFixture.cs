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
    public class MinimumLengthAttributeFixture : ConfigurationDesignHostTestBase
    {
        private MyMinLengthTestNode minLengthNode;
        private PropertyInfo valueInfo1;
        private PropertyInfo valueInfo2;

        [TestFixtureSetUp]
        public void Init()
        {
        }

        public override void SetUp()
        {
            base.SetUp();
            minLengthNode = new MyMinLengthTestNode();
            valueInfo1 = minLengthNode.GetType().GetProperty("Value1");
            valueInfo2 = minLengthNode.GetType().GetProperty("Value2");
            CreateHierarchyAndAddToHierarchyService(minLengthNode, CreateDefaultConfiguration());
        }

        [Test]
        public void MinLengthViolationTest()
        {
            MinimumLengthAttribute attribute = new MinimumLengthAttribute(8);
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(minLengthNode, valueInfo1, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void MinLengthTest()
        {
            MinimumLengthAttribute attribute = new MinimumLengthAttribute(8);
            minLengthNode.Value2 = "MyTestPassword";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(minLengthNode, valueInfo2, errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void MinLengthViolationTestWithVistor()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            Assert.IsNotNull(minLengthNode.Site);
            IConfigurationErrorLogService configurationErrorLogService = minLengthNode.Site.GetService(typeof(IConfigurationErrorLogService)) as IConfigurationErrorLogService;
            Assert.IsNotNull(configurationErrorLogService);
            cmd.Execute(minLengthNode);
            Assert.AreEqual(2, ValidationErrorsCount);
        }

        [Test]
        public void MinLengthTestWithVistor()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            Assert.IsNotNull(minLengthNode.Site);
            IConfigurationErrorLogService configurationErrorLogService = minLengthNode.Site.GetService(typeof(IConfigurationErrorLogService)) as IConfigurationErrorLogService;
            Assert.IsNotNull(configurationErrorLogService);
            minLengthNode.Value1 = "MyTest";
            minLengthNode.Value2 = "MyTestPassword";
            cmd.Execute(minLengthNode);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        private class MyMinLengthTestNode : ConfigurationNode
        {
            private string value1;
            private string value2;

            public MyMinLengthTestNode() : base()
            {
            }

            [MinimumLength(3)]
            public string Value1
            {
                get { return value1; }
                set { value1 = value; }
            }

            [MinimumLength(8)]
            public string Value2
            {
                get { return value2; }
                set { value2 = value; }
            }
        }
    }
}

#endif