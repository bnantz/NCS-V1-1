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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class PasswordFixture
    {
        [Test]
        public void Create()
        {
            string password = "erotu5hr";
            Password p = new Password(password);
            Assert.AreEqual(p.PasswordText, password);
        }

        [Test]
        public void ToStringTest()
        {
            string password = "erogijerg";
            Password p = new Password(password);
            Assert.IsFalse(p.ToString().Equals(password));
        }
    }
}

#endif