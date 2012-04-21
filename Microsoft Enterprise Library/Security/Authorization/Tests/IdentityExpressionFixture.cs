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
    public class IdentityExpressionFixture
    {
        private IPrincipal principal;

        public IdentityExpressionFixture()
        {
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            this.principal = new GenericPrincipal(identity, null);
        }

        [Test]
        public void IgnoreCaseTest()
        {
            IdentityExpression expression = new IdentityExpression("Foo");
            Assert.IsTrue(expression.Evaluate(principal));
        }

        [Test]
        public void CaseTest()
        {
            IdentityExpression expression = new IdentityExpression("foo");
            Assert.IsTrue(expression.Evaluate(this.principal));
        }

        [Test]
        public void FalseEvaluationTest()
        {
            IdentityExpression expression = new IdentityExpression("bar");
            Assert.IsFalse(expression.Evaluate(this.principal));
        }

        [Test]
        public void AnyTest()
        {
            IdentityExpression expression = new IdentityExpression("*");
            Assert.AreEqual(typeof(AnyExpression), expression.Word.GetType());
        }

        [Test]
        public void AnonymousTest()
        {
            IdentityExpression expression = new IdentityExpression("?");
            Assert.AreEqual(typeof(AnonymousExpression), expression.Word.GetType());
        }

        [Test]
        public void WordTest()
        {
            IdentityExpression expression = new IdentityExpression("user1");
            Assert.AreEqual(typeof(WordExpression), expression.Word.GetType());
        }

    }
}

#endif