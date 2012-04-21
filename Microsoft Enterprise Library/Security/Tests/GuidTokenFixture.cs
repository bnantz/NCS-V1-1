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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestFixture]
    public class GuidTokenFixture
    {
        [Test]
        public void DefaultTest()
        {
            GuidToken token = new GuidToken();
            Assert.IsTrue(token.Value.Length == 36);
        }

        [Test]
        public void DefinedTest()
        {
            Guid guid = Guid.NewGuid();
            GuidToken token = new GuidToken(guid);
            Assert.AreEqual(guid.ToString(), token.Value);
        }

        [Test]
        public void EmptyTest()
        {
            GuidToken token = new GuidToken(Guid.Empty);
            Assert.IsNotNull(token.Value.Length == 0);
        }
    }
}

#endif