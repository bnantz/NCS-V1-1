//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System.Security.Principal;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests
{
    [TestFixture]
    public class OrOperatorFixture
    {
        private IPrincipal principal;

        public OrOperatorFixture()
        {
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            this.principal = new GenericPrincipal(identity, null);
        }

        [Test]
        public void TrueTrueTest()
        {
            MockExpression leftExpression = new MockExpression(true);
            MockExpression rightExpression = new MockExpression(true);
            OrOperator expression = new OrOperator(leftExpression, rightExpression);
            Assert.IsTrue(expression.Evaluate(this.principal));
        }

        [Test]
        public void TrueFalseTest()
        {
            MockExpression leftExpression = new MockExpression(true);
            MockExpression rightExpression = new MockExpression(false);
            OrOperator expression = new OrOperator(leftExpression, rightExpression);
            Assert.IsTrue(expression.Evaluate(this.principal));
        }

        [Test]
        public void FalseFalseTest()
        {
            MockExpression leftExpression = new MockExpression(false);
            MockExpression rightExpression = new MockExpression(false);
            OrOperator expression = new OrOperator(leftExpression, rightExpression);
            Assert.IsFalse(expression.Evaluate(this.principal));
        }

        [Test]
        public void FalseTrueTest()
        {
            MockExpression leftExpression = new MockExpression(false);
            MockExpression rightExpression = new MockExpression(true);
            OrOperator expression = new OrOperator(leftExpression, rightExpression);
            Assert.IsTrue(expression.Evaluate(this.principal));
        }
    }
}

#endif