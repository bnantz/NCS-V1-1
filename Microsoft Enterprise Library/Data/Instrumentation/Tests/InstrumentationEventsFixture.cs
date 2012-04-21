//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation.Tests
{
    [TestFixture]
    public class InstrumentationEventsFixture : WMIEventsFixtureBase
    {
        private string testMessage = "test";
        private string testConnection = "test connection";

        [SetUp]
        public void Setup()
        {
            base.wmiLogged = false;
            base.wmiResult = string.Empty;
        }

        [Test]
        public void DataCommandFailedTest()
        {
            string eventName = "DataCommandFailedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireDataCommandFailedEvent);

            string expected = "Message = \"Database command failed: test connection\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireDataCommandFailedEvent()
        {
            DataCommandFailedEvent.Fire(testMessage, testConnection);
        }

        [Test]
        public void DataConnectionOpenedEventTest()
        {
            string eventName = "DataConnectionOpenedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireDataConnectionOpenedEvent);

            string expected = "Message = \"Data Connection opened: test connection\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireDataConnectionOpenedEvent()
        {
            DataConnectionOpenedEvent.Fire(testConnection);
        }

        [Test]
        public void DataConnectionFailedEventTest()
        {
            string eventName = "DataConnectionFailedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireDataConnectionFailedEvent);

            string expected = "Message = \"Data connection failed to open: test connection\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireDataConnectionFailedEvent()
        {
            DataConnectionFailedEvent.Fire(testConnection);
        }

        [Test]
        public void DataServiceFailureEventTest()
        {
            string eventName = "DataServiceFailureEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireDataServiceFailureEvent);

            string expected = "ExceptionMessage = \"System.Exception: test\";\n\tExceptionStackTrace = NULL;";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireDataServiceFailureEvent()
        {
            DataServiceFailureEvent.Fire(testMessage, new Exception(testMessage));
        }

        [Test]
        public void DataServiceConfigFailureEventTest()
        {
            string eventName = "DataServiceConfigFailureEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireDataServiceConfigFailureEvent);

            string expected = "ExceptionMessage = \"System.Exception: test\";\n\tExceptionStackTrace = NULL;\n\tFailedConfigurationFile = \"ConnectionSettings.config\";\n\tMessage = \"test\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireDataServiceConfigFailureEvent()
        {
            DataServiceConfigFailureEvent.Fire(testMessage, new Exception(testMessage), "ConnectionSettings.config");
        }

        [Test]
        public void DataTransactionFailedEventTest()
        {
            string eventName = "DataTransactionFailedEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireDataTransactionFailedEvent);

            string expected = "Message = \"Connection: test connection\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireDataTransactionFailedEvent()
        {
            DataTransactionFailedEvent.Fire(testConnection);
        }

        [Test]
        public void DataTransactionRolledBackEventTest()
        {
            string eventName = "DataTransactionRolledBackEvent";
            WmiEventDelegate action =
                new WmiEventDelegate(FireDataTransactionRolledBackEvent);

            string expected = "Message = \"Connection: test connection\"";
            base.FireMessageToWMI(eventName, action, expected);
        }

        private void FireDataTransactionRolledBackEvent()
        {
            DataTransactionRolledBackEvent.Fire(testConnection);
        }
    }
}

#endif