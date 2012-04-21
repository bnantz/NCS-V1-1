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
using System;
using System.Security.Principal;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests
{
    [TestFixture]
    public class AnonymousExpressionFixture
    {
        public AnonymousExpressionFixture()
        {
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void NotSupportedExceptionTest()
        {
            GenericIdentity identity = new GenericIdentity(String.Empty);
            GenericPrincipal principal = new GenericPrincipal(identity, null);
            AnonymousExpression expression = new AnonymousExpression();
            Assert.IsTrue(expression.Evaluate(principal));
        }

        [Test]
        public void FalseTest()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            Assert.IsTrue(identity.IsAuthenticated);
            AnonymousExpression expression = new AnonymousExpression();
            Assert.IsFalse(expression.Evaluate(identity));
        }

        [Test]
        public void TrueTest()
        {
            GenericIdentity identity = new GenericIdentity(String.Empty);
            Assert.IsFalse(identity.IsAuthenticated);

            AnonymousExpression expression = new AnonymousExpression();
            Assert.IsTrue(expression.Evaluate(identity));
        }
    }
}

#endif