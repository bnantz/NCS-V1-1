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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests
{
    [TestFixture]
    public class AuthorizationRuleProviderFixture
    {
		private ConfigurationContext context = new TestConfigurationContext();
        private AuthorizationRuleProviderData data;
        private AuthorizationRuleProvider provider;
        private IPrincipal principal;
        private const string testRuleName = "rule1";

		[TestFixtureSetUp]
        public void FixtureSetup()
        {
            data = new AuthorizationRuleProviderData("foo");
            data.TypeName = typeof(AuthorizationRuleProviderData).AssemblyQualifiedName;

            data.Rules.Add(new AuthorizationRuleData(testRuleName, "I:*"));

            provider = new AuthorizationRuleProvider();
            provider.ConfigurationName = data.Name;
            provider.Initialize(new TestAuthorizationRuleProviderView(data, context));

            principal = new GenericPrincipal(new GenericIdentity("user1"), new string[] {"Admin", "Manager"});
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizeInvalidAuthorityException()
        {
            this.provider.Authorize(null, testRuleName);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizeInvalidNameException()
        {
            this.provider.Authorize(this.principal, "");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizeNullContextException()
        {
            this.provider.Authorize(this.principal, null);
        }

        [Test]
        public void AuthorizeRuleNotFoundTest()
        {
            const string ruleName = "invalidRuleName";
            try
            {
                provider.Authorize(this.principal, ruleName);
            }
            catch (AuthorizationRuleNotFoundException e)
            {
                Assert.AreEqual(SR.AuthorizationRuleNotFoundMsg(ruleName), e.Message);
            }
            catch
            {
                throw;
            }
        }

        [Test]
        public void AuthorizeTest()
        {
            bool result = provider.Authorize(this.principal, testRuleName);
            Assert.IsTrue(result);
        }

        [Test]
        public void FactoryTest()
        {
			AuthorizationProviderFactory factory = new AuthorizationProviderFactory(context);
			IAuthorizationProvider ruleProvider = factory.GetAuthorizationProvider("RuleProvider");
			Assert.IsTrue(ruleProvider.Authorize(this.principal, testRuleName));
        }

        private class TestAuthorizationRuleProviderView : SecurityConfigurationView
        {
            public AuthorizationRuleProviderData data;

            public TestAuthorizationRuleProviderView(AuthorizationRuleProviderData data, ConfigurationContext context) : base(context)
            {
                this.data = data;
            }

            public override AuthorizationProviderData GetAuthorizationProviderData(string authorizationProviderName)
            {
                return data;
            }

        }
    }
}

#endif