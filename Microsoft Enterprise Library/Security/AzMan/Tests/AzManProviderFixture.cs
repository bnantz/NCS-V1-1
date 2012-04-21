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
using System.IO;
using System.Management;
using System.Security;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Tests
{
    [TestFixture]
    public class AzManProviderFixture : ConfigurationContextFixtureBase
    {
        private AzManAuthorizationProvider azman;
        private AzManAuthorizationProviderData data = new AzManAuthorizationProviderData();
        private static readonly string scope = "http://entlib.com";
        private static readonly string authorizedTask = "Authorize Purchase";
        private static readonly string unauthorizedTask = "Fire Employee";
        private IPrincipal principal;

        public override void FixtureSetup()
        {
            base.FixtureSetup();
            ReplaceSIDinConfigFile();
            this.principal = new GenericPrincipal(
                WindowsIdentity.GetCurrent(),
                new string[] {"Guest"});
        }

        [SetUp]
        public void Setup()
        {
            data.Application = "Enterprise Library Unit Test";
            data.AuditIdentifierPrefix = "myAuditId";
            data.Scope = "";
            data.StoreLocation = @"msxml://{currentPath}/testAzStore.xml";
            azman = new AzManAuthorizationProvider();
            azman.Initialize(new TestAuthorizationProviderConfigurationView(data, Context));
        }

        [Test]
        public void AuthorizeTask()
        {
            bool res = azman.Authorize(this.principal, authorizedTask);
            Assert.IsTrue(res);
        }

        [Test]
        public void UnauthorizedTask()
        {
            bool res = azman.Authorize(this.principal, unauthorizedTask);
            Assert.IsFalse(res);
        }

        [Test]
        public void AuthorizedThenUnauthorizedTask()
        {
            bool res = azman.Authorize(this.principal, authorizedTask);
            Assert.IsTrue(res);

            res = azman.Authorize(this.principal, unauthorizedTask);
            Assert.IsFalse(res);

            res = azman.Authorize(this.principal, authorizedTask);
            Assert.IsTrue(res);
        }

        [Test]
        public void AuthorizedScopeTask()
        {
            azman = new AzManAuthorizationProvider();
            azman.Initialize(new TestAuthorizationProviderConfigurationView(data, Context));
            string task = "Manage Extranet";

            data.Scope = scope;
            bool res = azman.Authorize(this.principal, task);

            Assert.IsTrue(res);
        }

        [Test]
        public void UnauthorizedScopeTask()
        {
            string task = "Publish Extranet";

            data.Scope = scope;
            azman = new AzManAuthorizationProvider();
            azman.Initialize(new TestAuthorizationProviderConfigurationView(data, Context));
            bool res = azman.Authorize(this.principal, task);

            Assert.IsFalse(res);
        }

        [Test]
        public void AuthorizedOperationUsingPrefix()
        {
            string operation = "O:Approve Purchase Order";
            bool res = azman.Authorize(this.principal, operation);

            Assert.IsTrue(res);
        }

        [Test]
        [ExpectedException(typeof(SecurityException), "Element not found.")]

        public void InvalidTask()
        {
            azman.Authorize(this.principal, "INVALID");
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]

        public void AuthorizeTaskWithNoOperations()
        {
            string task = "Missing Operations Task";
            azman.Authorize(this.principal, task);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]

        public void IdentityParameterIsNull()
        {
            WindowsIdentity ident = null;
            IPrincipal testPrincipal = new GenericPrincipal(
                ident,
                new string[] {"Guest"});

            azman.Authorize(testPrincipal, authorizedTask);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]

        public void TaskParameterIsNull()
        {
            azman.Authorize(this.principal, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]

        public void TaskParameterLengthIsZero()
        {
            azman.Authorize(this.principal, "");
        }

        [Test]
        [ExpectedException(typeof(SecurityException))]

        public void InvalidApplication()
        {
            data.Application = "INVALID";
            azman = new AzManAuthorizationProvider();
            azman.Initialize(new TestAuthorizationProviderConfigurationView(data, Context));
            azman.Authorize(this.principal, authorizedTask);
        }

        private void ReplaceSIDinConfigFile()
        {
            string sidToReplace = "S-1-5-21-839522115-764733703-1343024091-500";
            string currentSid = GetUserSID();

            // read current config file
            FileStream f = new FileStream("testAzStore.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader r = new StreamReader(f);
            string contents = r.ReadToEnd();
            f.Close();

            // write updated config file
            f = new FileStream("testAzStore.xml", FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter w = new StreamWriter(f);
            contents = contents.Replace(sidToReplace, currentSid);
            w.Write(contents);
            w.Flush();
            f.Close();
        }

        private string GetUserSID()
        {
            string currentSid = "";
            string ident = WindowsIdentity.GetCurrent().Name;
            string domain = ident.Substring(0, ident.IndexOf(@"\"));
            string user = ident.Substring(domain.Length + 1);

            // get current users sid
            string pGroup = "ROOT\\CIMV2:Win32_Account.Domain=\"" + domain + "\",Name=\"" + user + "\"";
            ManagementPath path = new ManagementPath(pGroup);
            using (ManagementObject o = new ManagementObject(path))
            {
                currentSid = o["SID"].ToString();
            }

            return currentSid;
        }

        private class TestAuthorizationProviderConfigurationView : SecurityConfigurationView
        {
            public AzManAuthorizationProviderData data;

            public TestAuthorizationProviderConfigurationView(AzManAuthorizationProviderData data, ConfigurationContext context) : base(context)
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