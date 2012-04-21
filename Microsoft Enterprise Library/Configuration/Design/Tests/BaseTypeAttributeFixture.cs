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
using System;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class BaseTypeAttributeFixture
    {
        private BaseTypeAttribute baseTypeAttribute;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            baseTypeAttribute = new BaseTypeAttribute(typeof(Array));
        }

        [Test]
        public void TypeTest()
        {
            Assert.AreEqual(typeof(Array), baseTypeAttribute.BaseType);
        }
    }
}

#endif