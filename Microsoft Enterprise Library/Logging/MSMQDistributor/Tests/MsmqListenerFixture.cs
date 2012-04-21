//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    [TestFixture]
    public class MsmqListenerFixture : ConfigurationContextFixtureBase
    {
        private MsmqListener listener;
        private DistributorServiceTestFacade distributorServiceTestFacade;
        private MockMsmqLogDistributor mockQ;

        private DistributorEventLogger eventLogger;

        [SetUp]
        public void Setup()
        {
            this.distributorServiceTestFacade = new DistributorServiceTestFacade();
            this.listener = new MsmqListener(this.distributorServiceTestFacade, 1000);
            this.eventLogger = distributorServiceTestFacade.EventLogger;
            this.mockQ = new MockMsmqLogDistributor(Context);
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void StartListener()
        {
            this.eventLogger.AddMessage("HEADER", "Simulated Start");

            this.listener.QueueTimerInterval = 10;
            this.listener.SetMsmqLogDistributor(mockQ);
            this.listener.StartListener();

            Thread.Sleep(this.listener.QueueTimerInterval + 300);
            Assert.IsTrue(this.mockQ.ReceiveMsgCalled, "receive initiated");

            this.eventLogger.WriteToLog(new Exception("simulated exception - forced event logger flush"), Severity.Error);

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStarting), "start begin");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStartComplete(listener.QueueTimerInterval)), "start complete");

            this.listener.StopListener();
        }

        [Test]
        public void StopListener()
        {
            this.eventLogger.AddMessage("HEADER", "Simulated Stop");

            this.listener.QueueTimerInterval = 10;
            this.listener.SetMsmqLogDistributor(mockQ);
            this.listener.StartListener();

            Thread.Sleep(listener.QueueTimerInterval + 300);
            bool result = this.listener.StopListener();

            Assert.IsTrue(result, "stopListener result");

            try
            {
                throw new Exception("simulated exception - forced event logger flush");
            }
            catch (Exception e)
            {
                this.eventLogger.WriteToLog(e, Severity.Error);
            }

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStopStarted), "stop begin");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStopCompleted("0")), "stop complete");
        }

        [Test]
        public void StopListenerAndExceedStopRetries()
        {
            this.eventLogger.AddMessage("HEADER", "Simulated Stop and Exceed Timeout");

            this.listener.QueueTimerInterval = 10;
            this.listener.QueueListenerRetries = 1;
            this.mockQ.SetIsCompleted(false);
            this.listener.SetMsmqLogDistributor(mockQ);
            this.listener.StartListener();

            Thread.Sleep(listener.QueueTimerInterval + 300);
            bool result = this.listener.StopListener();

            Assert.IsFalse(this.mockQ.StopReceiving, "stop receiving");
            Assert.IsFalse(result, "stopListener result");

            try
            {
                throw new Exception("simulated exception - forced event logger flush");
            }
            catch (Exception e)
            {
                this.eventLogger.WriteToLog(e, Severity.Error);
            }
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStopStarted), "stop begin");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerCannotStop("1")), "cannot stop warning");
        }

        [Test]
        public void StopListenerError()
        {
            this.eventLogger.AddMessage("HEADER", "Simulated Stop Exception");

            this.listener.QueueTimerInterval = 10000;
            this.mockQ.ExceptionOnGetIsCompleted = true;
            this.listener.SetMsmqLogDistributor(mockQ);
            this.listener.StartListener();

            try
            {
                this.listener.StopListener();
            }
            catch (Exception e)
            {
                this.eventLogger.WriteToLog(e, Severity.Error);

                Assert.IsTrue(CommonUtil.LogEntryExists(SR.ListenerStopError), "stop error");
                return;
            }

            Assert.Fail("exception not raised");
        }

        [Test]
        public void RevertToDefaultTimerInterval()
        {
            listener.QueueTimerInterval = 0;

            Assert.AreEqual(20000, listener.QueueTimerInterval);
        }

    }
}

#endif