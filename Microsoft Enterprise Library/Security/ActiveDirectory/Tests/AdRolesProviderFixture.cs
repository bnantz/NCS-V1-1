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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.Tests
{
	[TestFixture]
	public class AdRolesProviderFixture
	{
		private AdRolesProvider adRolesProvider;
		private TestConfigurationContext context = new TestConfigurationContext();

		public AdRolesProviderFixture()
		{
		}

		[SetUp]
		public void SetUp()
		{
			adRolesProvider = (AdRolesProvider) RolesFactory.GetRolesProvider("AdRolesProviderName", context);
		}

		[Test]
		public void CheckThatRoleProviderFactoryCreatesValidADRoleProviderInstance()
		{
			adRolesProvider = null;

			Assert.IsNull(adRolesProvider);
			adRolesProvider = (AdRolesProvider) RolesFactory.GetRolesProvider("AdRolesProviderName", context);

			Assert.IsNotNull(adRolesProvider);
			Assert.AreEqual("Microsoft.Practices.EnterpriseLibrary.Security.ActiveDirectory.AdRolesProvider", adRolesProvider.ToString());
		}

		[ExpectedException(typeof (ConfigurationException))]
		[Test]
		public void CheckThatRoleProviderFactoryDoesNotReturnInvalidAdRoleProviderInstance()
		{
			RolesFactory.GetRolesProvider("NonConfiguredAdRolesProviderName", context);
		}

		[Test]
		public void CheckIfValidIdentityAssociatedWithValidRoles()
		{
			IIdentity testIdentity = new GenericIdentity("testuser", "EnterpriseLibrary.Security.ActiveDirectory.DefaultUserAds");

			IPrincipal testPrincipal = adRolesProvider.GetRoles(testIdentity);

			Assert.IsNotNull(testPrincipal);

			Assert.AreEqual("testuser", testPrincipal.Identity.Name);
			Assert.AreEqual(testIdentity.Name, testPrincipal.Identity.Name);
			Assert.AreEqual("EnterpriseLibrary.Security.ActiveDirectory.DefaultUserAds", testPrincipal.Identity.AuthenticationType);

			Assert.IsTrue(testPrincipal.IsInRole("Users"));
			Assert.IsTrue(testPrincipal.IsInRole("Managers"));
			Assert.IsFalse(testPrincipal.IsInRole("Admins"));
		}

		[Test]
		public void CheckIfValidIdentityNotAssociatedWithInvalidRole()
		{
			IIdentity testIdentity = new GenericIdentity("testuser", "EnterpriseLibrary.Security.ActiveDirectory.DefaultUserAds");

			IPrincipal testPrincipal = adRolesProvider.GetRoles(testIdentity);

			Assert.IsNotNull(testPrincipal);

			Assert.AreEqual("testuser", testPrincipal.Identity.Name);
			Assert.AreEqual(testIdentity.Name, testPrincipal.Identity.Name);
			Assert.AreEqual("EnterpriseLibrary.Security.ActiveDirectory.DefaultUserAds", testPrincipal.Identity.AuthenticationType);

			Assert.IsFalse(testPrincipal.IsInRole("SuperUsers"));
		}

		[ExpectedException(typeof (ArgumentNullException))]
		[Test]
		public void CheckIfGetRolesFailsWithNullIdentity()
		{
			IIdentity testIdentity = null;

			adRolesProvider.GetRoles(testIdentity);

			Assert.Fail();
		}

		[Test]
		public void CheckIfValidIdentityNotAssociatedWithAnyRoles()
		{
			IIdentity testIdentity = new GenericIdentity("bogususer", "EnterpriseLibrary.Security.ActiveDirectory.DefaultUserAds");

			IPrincipal testPrincipal = adRolesProvider.GetRoles(testIdentity);

			Assert.IsNotNull(testPrincipal);

			Assert.IsFalse(testPrincipal.IsInRole("Users"));
			Assert.IsFalse(testPrincipal.IsInRole("Managers"));
			Assert.IsFalse(testPrincipal.IsInRole("Admins"));
		}

		[Test]
		[ExpectedException(typeof (UserNotFoundException))]
		public void CheckIfInvalidIdentityNotAssociatedWithAnyRoles()
		{
			IIdentity testIdentity = new GenericIdentity("InvalidUser", "EnterpriseLibrary.Security.ActiveDirectory.DefaultUserAds");

		    IPrincipal testPrincipal = adRolesProvider.GetRoles(testIdentity);
		    Assert.IsNotNull(testPrincipal);
		    Assert.IsFalse(testPrincipal.IsInRole("Users"));
		    Assert.IsFalse(testPrincipal.IsInRole("Managers"));
		    Assert.IsFalse(testPrincipal.IsInRole("Admins"));
		}

	    [Test]
	    public void GetRolesForDomainUser()
	    {
	        IIdentity testIdentity = new GenericIdentity("TestDomain\\testuser", "EnterpriseLibrary.Security.ActiveDirectory.DefaultUserAds");

			IPrincipal testPrincipal = adRolesProvider.GetRoles(testIdentity);

			Assert.IsNotNull(testPrincipal);
			Assert.IsTrue(testPrincipal.IsInRole("Users"));
			Assert.IsTrue(testPrincipal.IsInRole("Managers"));
			Assert.IsFalse(testPrincipal.IsInRole("Admins"));
		}
	}
}

#endif