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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation.Tests
{
    [TestFixture]
    public class WMIEventsFixture : WMIEventsFixtureBase
    {
        private string testMessage = "test";
        private string defaultExpected = "";

        [SetUp]
        public void Setup()
        {
            base.wmiLogged = false;
            base.wmiResult = string.Empty;
            this.defaultExpected = "Message = \"" + testMessage + "\"";
        }

        [Test]
        public void SecurityAuthenticationCheckEventTest()
        {
            string eventName = "SecurityAuthenticationCheckEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityAuthenticationCheckEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecurityAuthenticationCheckEvent()
        {
            SecurityAuthenticationCheckEvent.Fire(testMessage);
        }

        [Test]
        public void SecurityAuthenticationFailedEventTest()
        {
            string eventName = "SecurityAuthenticationFailedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityAuthenticationFailedEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecurityAuthenticationFailedEvent()
        {
            SecurityAuthenticationFailedEvent.Fire(testMessage);
        }

        [Test]
        public void SecurityAuthorizationCheckEventTest()
        {
            string eventName = "SecurityAuthorizationCheckEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityAuthorizationCheckEvent);

            base.FireMessageToWMI(eventName, action, "Message = \"" + testMessage + ":" + "action" + "\"");
        }

        private void FireSecurityAuthorizationCheckEvent()
        {
            SecurityAuthorizationCheckEvent.Fire(testMessage, "action");
        }

        [Test]
        public void SecurityAuthorizationFailedEventTest()
        {
            string eventName = "SecurityAuthorizationFailedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityAuthorizationFailedEvent);

            base.FireMessageToWMI(eventName, action, "Message = \"" + testMessage + ":" + "action" + "\"");
        }

        private void FireSecurityAuthorizationFailedEvent()
        {
            SecurityAuthorizationFailedEvent.Fire(testMessage, "action");
        }

        [Test]
        public void SecurityProfileLoadEventTest()
        {
            string eventName = "SecurityProfileLoadEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityProfileLoadEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecurityProfileLoadEvent()
        {
            SecurityProfileLoadEvent.Fire(testMessage);
        }

        [Test]
        public void SecurityProfileSaveEventTest()
        {
            string eventName = "SecurityProfileSaveEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityProfileSaveEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecurityProfileSaveEvent()
        {
            SecurityProfileSaveEvent.Fire(testMessage);
        }

        [Test]
        public void SecurityRoleLoadEventTest()
        {
            string eventName = "SecurityRoleLoadEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityRoleLoadEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecurityRoleLoadEvent()
        {
            SecurityRoleLoadEvent.Fire(testMessage);
        }
    }
}

#endif