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
using System.Configuration;
using System.Security.Principal;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.Tests
{
    [TestFixture]
    public class DbAuthenticationProviderFixture : ConfigurationContextFixtureBase
    {
        private static DbAuthenticationProvider dbAuthProvider;
        private byte[] password;
        private static string username;

        [SetUp]
        public void SetUp()
        {
            TestDataSetup.CreateTestData(Context);
            dbAuthProvider = (DbAuthenticationProvider)AuthenticationFactory.GetAuthenticationProvider("DbAuthenticationProviderName");
            password = ASCIIEncoding.ASCII.GetBytes("Password");
            username = "testuser";
        }

        [TearDown]
        public void TearDown()
        {
            TestDataSetup.DeleteTestData(Context);
            dbAuthProvider = null;
        }

        [Test]
        public void CheckThatProviderFactoryCreatesConfiguredInstance()
        {
            dbAuthProvider = (DbAuthenticationProvider)AuthenticationFactory.GetAuthenticationProvider("DbAuthenticationProviderName");

            Assert.IsNotNull(dbAuthProvider);
            Assert.AreEqual("Microsoft.Practices.EnterpriseLibrary.Security.Database.Authentication.DbAuthenticationProvider", dbAuthProvider.ToString());
        }

        [ExpectedException(typeof(ConfigurationException))]
        [Test]

        public void CheckThatProviderFactoryDoesNotInitializeInvalidInstance()
        {
            AuthenticationFactory.GetAuthenticationProvider("NonConfiguredDbAuthenticationProviderName");
        }

        [Test]
        public void CheckIfAuthenticationSucceedsWithValidCredentials()
        {
            NamePasswordCredential credentials = new NamePasswordCredential(username, password);
            IIdentity identity;

            bool retVal = dbAuthProvider.Authenticate(credentials, out identity);

            Assert.IsTrue(retVal);
            Assert.IsNotNull(identity);
            Assert.AreEqual(username, identity.Name);
            Assert.IsTrue(identity.IsAuthenticated);
        }

        [Test]
        public void AuthenticationSucceedsWithZeroLengthPassword()
        {
            NamePasswordCredential credentials = new NamePasswordCredential("emptyUser", new byte[] {});
            IIdentity identity;

            bool successfulAuthentication = dbAuthProvider.Authenticate(credentials, out identity);
            Assert.IsTrue(successfulAuthentication);
            Assert.IsTrue(identity.IsAuthenticated);
        }

        [Test]
        public void AuthenticationCanBeSuccessfulTwice()
        {
            NamePasswordCredential credentials = new NamePasswordCredential(username, password);
            IIdentity firstIdentity;
            bool firstCall = dbAuthProvider.Authenticate(credentials, out firstIdentity);

            IIdentity secondIdentity;
            bool secondCall = dbAuthProvider.Authenticate(credentials, out secondIdentity);

            Assert.IsTrue(firstCall);
            Assert.IsTrue(secondCall);
            Assert.IsNotNull(secondIdentity);
        }

        [Test]
        public void CheckIfAuthenticationSucceedsWithValidCredentialsThroughInterface()
        {
            IAuthenticationProvider IAuthProvider = AuthenticationFactory.GetAuthenticationProvider("DbAuthenticationProviderName");

            NamePasswordCredential credentials = new NamePasswordCredential(username, password);
            IIdentity identity;

            bool retVal = IAuthProvider.Authenticate(credentials, out identity);

            Assert.IsTrue(retVal);
            Assert.IsNotNull(identity);
            Assert.AreEqual(username, identity.Name);
            Assert.IsTrue(identity.IsAuthenticated);
        }

        [Test]
        public void CheckIfAuthenticationSucceedsWithCredentialsCastToObject()
        {
            NamePasswordCredential credentials = new NamePasswordCredential(username, password);
            IIdentity identity;

            bool retVal = dbAuthProvider.Authenticate(credentials, out identity);

            Assert.IsTrue(retVal);
            Assert.IsNotNull(identity);
            Assert.AreEqual(username, identity.Name);
            Assert.IsTrue(identity.IsAuthenticated);
        }

        [Test]
        public void CheckIfAuthenticationFailsWithBogusCredentialsObject()
        {
            string credentials = "TestCredentials";
            IIdentity identity;

            bool retVal = dbAuthProvider.Authenticate(credentials, out identity);

            Assert.IsFalse(retVal);
            Assert.IsNull(identity);
        }

        [Test]
        public void CheckIfAuthenticationFailsWithInvalidPassword()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("InvalidPassword");
            NamePasswordCredential credentials = new NamePasswordCredential(username, password);
            IIdentity identity;

            bool retVal = dbAuthProvider.Authenticate(credentials, out identity);

            Assert.IsFalse(retVal);
            Assert.IsNull(identity);
        }

        [Test]
        public void CheckIfAuthenticationFailsWithInvalidUsername()
        {
            byte[] password = ASCIIEncoding.ASCII.GetBytes("password");
            NamePasswordCredential credentials = new NamePasswordCredential("invalidUserName", password);
            IIdentity identity;

            bool retVal = dbAuthProvider.Authenticate(credentials, out identity);

            Assert.IsFalse(retVal);
            Assert.IsNull(identity);
        }

        [Test]
        public void CheckIfAuthenticationFailsWithNullCredentialObject()
        {
            NamePasswordCredential credentials = null;
            IIdentity identity;

            bool retVal = dbAuthProvider.Authenticate(credentials, out identity);

            Assert.IsFalse(retVal);
            Assert.IsNull(identity);
        }

        [Test]
        public void CheckIfAuthenticationFailsWithInvalidCredentialObject()
        {
            object credentials = new object();
            IIdentity identity;

            bool retVal = dbAuthProvider.Authenticate(credentials, out identity);

            Assert.IsFalse(retVal);
            Assert.IsNull(identity);
        }
    }
}

#endif