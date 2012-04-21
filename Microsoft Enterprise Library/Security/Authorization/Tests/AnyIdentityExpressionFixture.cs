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
    public class AnyIdentityExpressionFixture
    {
        public AnyIdentityExpressionFixture()
        {
        }

        [Test]
        public void TrueTest()
        {
            AnyExpression expression = new AnyExpression();
            Assert.IsTrue(expression.Evaluate(new GenericPrincipal(new GenericIdentity("foo"), null)));
        }
    }
}

#endif