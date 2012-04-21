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
    public class RegexAttributeFixture : ConfigurationDesignHostTestBase
    {
        private MyRegexTestNode regexNode;
        private PropertyInfo emailInfo;

        public override void SetUp()
        {
            base.SetUp();
            regexNode = new MyRegexTestNode();
            emailInfo = regexNode.GetType().GetProperty("Email");
            CreateHierarchyAndAddToHierarchyService(regexNode, CreateDefaultConfiguration());
        }

        [Test]
        public void RegexViolationTest()
        {
            RegexAttribute attribute = new RegexAttribute(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            regexNode.Email = "joeblow";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(regexNode, emailInfo, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void RegexTest()
        {
            RegexAttribute attribute = new RegexAttribute(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            regexNode.Email = "someone@microsoft.com";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(regexNode, emailInfo, errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void RegexViolationTestWithVistor()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            Assert.IsNotNull(regexNode.Site);
            regexNode.Email = "joeblow.com";
            cmd.Execute(regexNode);
            Assert.AreEqual(1, ValidationErrorsCount);
        }

        [Test]
        public void RegexTestWithVistor()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            Assert.IsNotNull(regexNode.Site);
            regexNode.Email = "someone@microsoft.com";
            cmd.Execute(regexNode);
            Assert.AreEqual(0, ConfigurationErrorsCount);
        }

        private class MyRegexTestNode : ConfigurationNode
        {
            private string email;

            public MyRegexTestNode() : base()
            {
            }

            [Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
            public string Email
            {
                get { return email; }
                set { email = value; }
            }
        }
    }
}

#endif