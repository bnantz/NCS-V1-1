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
    public class TypeValidationAttributeFixture : ConfigurationDesignHostTestBase
    {
        private MyTypeTestNode typeNode;
        private PropertyInfo valueInfo1;

        public override void SetUp()
        {
            base.SetUp();
            typeNode = new MyTypeTestNode();
            valueInfo1 = typeNode.GetType().GetProperty("TypeName");
            CreateHierarchyAndAddToHierarchyService(typeNode, CreateDefaultConfiguration());
        }

        [Test]
        public void ValidTypeProducesNoErrors()
        {
            TypeValidationAttribute attribute = new TypeValidationAttribute();
            ValidationErrorCollection errors = new ValidationErrorCollection();
            typeNode.TypeName = GetType().AssemblyQualifiedName;
            attribute.Validate(typeNode, valueInfo1, errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void InvalidTypeProducesAnError()
        {
            TypeValidationAttribute attribute = new TypeValidationAttribute();
            typeNode.TypeName = "MyTest";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(typeNode, valueInfo1, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void EmptyTypeNameProducesAnError()
        {
            TypeValidationAttribute attribute = new TypeValidationAttribute();
            typeNode.TypeName = string.Empty;
            ValidationErrorCollection errors = new ValidationErrorCollection();
            attribute.Validate(typeNode, valueInfo1, errors);
            Assert.AreEqual(1, errors.Count);
        }

        private class MyTypeTestNode : ConfigurationNode
        {
            private string value1;

            public MyTypeTestNode() : base()
            {
            }

            [TypeValidation]
            public string TypeName
            {
                get { return value1; }
                set { value1 = value; }
            }
        }
    }
}

#endif