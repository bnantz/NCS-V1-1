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
    public class RoleExpressionFixture
    {
        private IPrincipal principal;

        public RoleExpressionFixture()
        {
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            string[] roles = new string[] {"Manager"};
            this.principal = new GenericPrincipal(identity, roles);
        }

        [Test]
        public void TrueTest()
        {
            RoleExpression expression = new RoleExpression("Manager");
            Assert.IsTrue(expression.Evaluate(this.principal));
        }

        [Test]
        public void FalseTest()
        {
            RoleExpression expression = new RoleExpression("Admin");
            Assert.IsFalse(expression.Evaluate(this.principal));
        }

        [Test]
        public void AnyTest()
        {
            RoleExpression expression = new RoleExpression("*");
            Assert.AreEqual(typeof(AnyExpression), expression.Word.GetType());
        }

        [Test]
        public void WordTest()
        {
            RoleExpression expression = new RoleExpression("Role1");
            Assert.AreEqual(typeof(WordExpression), expression.Word.GetType());
        }
    }
}

#endif