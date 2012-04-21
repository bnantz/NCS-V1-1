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
using System.Text;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestFixture]
    public class NamePasswordCredentialFixture
    {
        private const string pwd = "password123ABC";
        private byte[] pwdBytes = null;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            pwdBytes = Encoding.Unicode.GetBytes(pwd);
        }

        [Test]
        public void ConstructorBytes()
        {
            NamePasswordCredential cred = new NamePasswordCredential("joe", pwdBytes);

            Assert.AreEqual("joe", cred.Name);
            Assert.AreEqual(pwdBytes, cred.PasswordBytes);
            Assert.AreEqual(pwd, cred.Password);
        }

        [Test]
        public void ConstructorString()
        {
            NamePasswordCredential cred = new NamePasswordCredential("joe", pwd);

            Assert.AreEqual("joe", cred.Name);
            Assert.AreEqual(pwd, cred.Password);
        }

        [Test]
        public void ZeroLengthPasswordIsValid()
        {
            NamePasswordCredential cred = new NamePasswordCredential("fred", "");

            Assert.AreEqual("fred", cred.Name);
            Assert.AreEqual("", cred.Password);
            Assert.AreEqual(0, cred.PasswordBytes.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullName()
        {
            new NamePasswordCredential(null, pwd);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZeroLengthName()
        {
            new NamePasswordCredential("", pwd);
        }
    }
}

#endif