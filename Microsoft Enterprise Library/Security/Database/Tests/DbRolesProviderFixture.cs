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
using System.Configuration;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Database.Tests
{
    [TestFixture]
    public class DbRolesProviderFixture : ConfigurationContextFixtureBase
    {
        private static DbRolesProvider dbRolesProvider;

        public override void FixtureSetup()
        {
            base.FixtureSetup();
            TestDataSetup.CreateTestData(Context);
        }

        public override void FixtureTeardown()
        {
            TestDataSetup.DeleteTestData(Context);
            base.FixtureTeardown();
        }

        [SetUp]
        public void Setup()
        {
            dbRolesProvider = (DbRolesProvider)RolesFactory.GetRolesProvider("DbRolesProviderName");
        }

        [TearDown]
        public void Teardown()
        {
            dbRolesProvider = null;
        }

        [Test]
        public void CheckThatRoleProviderFactoryCreatesValidDBRoleProviderInstance()
        {
            dbRolesProvider = null;

            Assert.IsNull(dbRolesProvider);

            dbRolesProvider = (DbRolesProvider)RolesFactory.GetRolesProvider("DbRolesProviderName");

            Assert.IsNotNull(dbRolesProvider);
            Assert.AreEqual("Microsoft.Practices.EnterpriseLibrary.Security.Database.DbRolesProvider", dbRolesProvider.ToString());
        }

        [ExpectedException(typeof(ConfigurationException))]
        [Test]
        public void CheckThatRoleProviderFactoryDoesNotReturnInvalidDBRoleProviderInstance()
        {
            DbRolesProvider tmpDbRolesProvider = (DbRolesProvider)RolesFactory.GetRolesProvider("NonConfiguredDbRolesProviderName");

            Assert.Fail();
        }

        [Test]
        public void CheckIfValidIdentityAssociatedWithValidRoles()
        {
            IIdentity testIdentity = new GenericIdentity("testuser", "EnterpriseLibrary.Security.Database.DefaultUserDB");

            IPrincipal testPrincipal = dbRolesProvider.GetRoles(testIdentity);

            Assert.IsNotNull(testPrincipal);

            Assert.AreEqual("testuser", testPrincipal.Identity.Name);
            Assert.AreEqual(testIdentity.Name, testPrincipal.Identity.Name);
            Assert.AreEqual("EnterpriseLibrary.Security.Database.DefaultUserDB", testPrincipal.Identity.AuthenticationType);

            Assert.IsTrue(testPrincipal.IsInRole("Users"));
            Assert.IsTrue(testPrincipal.IsInRole("Managers"));
            Assert.IsFalse(testPrincipal.IsInRole("Admins"));
        }

        [Test]
        public void CheckIfValidIdentityNotAssociatedWithInvalidRole()
        {
            IIdentity testIdentity = new GenericIdentity("testuser", "EnterpriseLibrary.Security.Database.DefaultUserDB");

            IPrincipal testPrincipal = dbRolesProvider.GetRoles(testIdentity);

            Assert.IsNotNull(testPrincipal);

            Assert.AreEqual("testuser", testPrincipal.Identity.Name);
            Assert.AreEqual(testIdentity.Name, testPrincipal.Identity.Name);
            Assert.AreEqual("EnterpriseLibrary.Security.Database.DefaultUserDB", testPrincipal.Identity.AuthenticationType);

            Assert.IsFalse(testPrincipal.IsInRole("SuperUsers"));
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [Test]
        public void CheckIfGetRolesFailsWithNullIdentity()
        {
            IIdentity testIdentity = null;

            IPrincipal testPrincipal = dbRolesProvider.GetRoles(testIdentity);

            Assert.Fail();
        }

        [Test]
        public void CheckIfValidIdentityNotAssociatedWithAnyRoles()
        {
            IIdentity testIdentity = new GenericIdentity("bogususer", "EnterpriseLibrary.Security.Database.DefaultUserDB");

            IPrincipal testPrincipal = dbRolesProvider.GetRoles(testIdentity);

            Assert.IsNotNull(testPrincipal);

            Assert.IsFalse(testPrincipal.IsInRole("Users"));
            Assert.IsFalse(testPrincipal.IsInRole("Managers"));
            Assert.IsFalse(testPrincipal.IsInRole("Admins"));
        }

        [Test]
        public void CheckIfInvalidIdentityNotAssociatedWithAnyRoles()
        {
            IIdentity testIdentity = new GenericIdentity("InvalidUser", "EnterpriseLibrary.Security.Database.DefaultUserDB");

            IPrincipal testPrincipal = dbRolesProvider.GetRoles(testIdentity);

            Assert.IsNotNull(testPrincipal);
            Assert.IsFalse(testPrincipal.IsInRole("Users"));
            Assert.IsFalse(testPrincipal.IsInRole("Managers"));
            Assert.IsFalse(testPrincipal.IsInRole("Admins"));
        }
    }
}

#endif