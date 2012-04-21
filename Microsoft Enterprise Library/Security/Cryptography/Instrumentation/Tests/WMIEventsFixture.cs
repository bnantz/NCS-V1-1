//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation.Tests
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
        public void SecurityCryptoHashCreateEventTest()
        {
            string eventName = "SecurityCryptoHashCreateEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityCryptoHashCreateEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecurityCryptoHashCreateEvent()
        {
            SecurityCryptoHashCreateEvent.Fire(testMessage);
        }

        [Test]
        public void SecurityCryptoHashCheckEventTest()
        {
            string eventName = "SecurityCryptoHashCheckEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityCryptoHashCheckEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecurityCryptoHashCheckEvent()
        {
            SecurityCryptoHashCheckEvent.Fire(testMessage);
        }

        [Test]
        public void SecurityCryptoHashFailureEventTest()
        {
            string eventName = "SecurityCryptoHashCheckFailureEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecurityCryptoHashFailureEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecurityCryptoHashFailureEvent()
        {
            SecurityCryptoHashCheckFailureEvent.Fire(testMessage);
        }

        [Test]
        public void SecuritySymmetricEncryptionEventTest()
        {
            string eventName = "SecurityCryptoSymmetricEncryptionEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecuritySymmetricEncryptionEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecuritySymmetricEncryptionEvent()
        {
            SecurityCryptoSymmetricEncryptionEvent.Fire(testMessage);
        }

        [Test]
        public void SecuritySymmetricDecryptionEventTest()
        {
            string eventName = "SecurityCryptoSymmetricDecryptionEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireSecuritySymmetricDecryptionEvent);

            base.FireMessageToWMI(eventName, action, defaultExpected);
        }

        private void FireSecuritySymmetricDecryptionEvent()
        {
            SecurityCryptoSymmetricDecryptionEvent.Fire(testMessage);
        }

    }
}

#endif