//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if   UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation.Tests
{
    [TestFixture]
    public class WMIEventsFixture : WMIEventsFixtureBase
    {
        [SetUp]
        public void Setup()
        {
            base.wmiLogged = false;
            base.wmiResult = string.Empty;
        }

        [Test]
        public void ConfigFailureTest()
        {
            string eventName = "ExceptionHandlingConfigFailureEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireExceptionHandlingConfigFailureEvent);

            string expected = "Message = \"\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireExceptionHandlingConfigFailureEvent()
        {
            ExceptionHandlingConfigFailureEvent.Fire("test");
        }
    }
}

#endif