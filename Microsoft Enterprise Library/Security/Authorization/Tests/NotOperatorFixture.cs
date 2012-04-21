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
    public class NotOperatorFixture
    {
        private IPrincipal principal;

        public NotOperatorFixture()
        {
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            this.principal = new GenericPrincipal(identity, null);
        }

        [Test]
        public void FalseTest()
        {
            MockExpression expression = new MockExpression(true);
            NotOperator notExpression = new NotOperator(expression);
            Assert.IsFalse(notExpression.Evaluate(this.principal));
        }

        [Test]
        public void TrueTest()
        {
            MockExpression expression = new MockExpression(false);
            NotOperator notExpression = new NotOperator(expression);
            Assert.IsTrue(notExpression.Evaluate(this.principal));
        }
    }
}

#endif