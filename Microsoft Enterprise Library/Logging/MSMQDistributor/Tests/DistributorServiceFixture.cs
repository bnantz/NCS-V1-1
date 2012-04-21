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
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    [TestFixture]
    public class DistributorServiceFixture
    {
        private DistributorServiceTestFacade distributorServiceTestFacade;
        private DistributorEventLogger eventLogger;
        private MockMsmqListener mockListener;

        private const int sleepTimer = 300;

        [SetUp]
        public void Setup()
        {
            this.distributorServiceTestFacade = new DistributorServiceTestFacade();
            this.eventLogger = distributorServiceTestFacade.EventLogger;
            this.mockListener = new MockMsmqListener(this.distributorServiceTestFacade, 1000);

            this.distributorServiceTestFacade.QueueListener = mockListener;

            CommonUtil.SetDistributionStrategy("Msmq");
        }

        [TearDown]
        public void Teardown()
        {
            // Reset distribution strategy to in process
            CommonUtil.SetDistributionStrategy("InProc");
        }

        [Test]
        public void Initialization()
        {
            DistributorServiceTestFacade distributor = new DistributorServiceTestFacade();
            distributor.InitializeComponent();
            Assert.IsNotNull(distributor);
            Assert.AreEqual(ServiceStatus.OK, distributor.Status);
            Assert.AreEqual("Enterprise Library Logging Distributor Service", distributor.ApplicationName);

            // force log entry
            this.eventLogger.WriteToLog(new Exception("simulated exception - forced event logger flush"), Severity.Error);

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.InitializeComponentStarted), "init begin");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.InitializeComponentCompleted), "init end");
        }

        [Test]
        public void StartAndStopService()
        {
            this.distributorServiceTestFacade.OnStart();

            Assert.IsTrue(mockListener.StartCalled, "mock start called");
            Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status, "status");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ValidationStarted), "validate start");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ValidationComplete), "validate complete");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceStartComplete(distributorServiceTestFacade.ApplicationName)), "start complete");
            // TODO: assert WMI event fired

            this.distributorServiceTestFacade.OnStop();
            Assert.IsTrue(mockListener.StopCalled, "mock stop called");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceStopComplete(distributorServiceTestFacade.ApplicationName)), "stop complete");
            Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status);
        }

        [Test]
        public void StartServiceWithError()
        {
            mockListener.ExceptionOnStart = true;
            this.distributorServiceTestFacade.OnStart();

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceStartError(distributorServiceTestFacade.ApplicationName)), "start error");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ValidationError), "validate error");

            Assert.AreEqual(ServiceStatus.Shutdown, this.distributorServiceTestFacade.Status);
        }

        [Test]
        public void StopServiceWithError()
        {
            mockListener.ExceptionOnStop = true;

            this.distributorServiceTestFacade.OnStart();
            this.distributorServiceTestFacade.OnStop();

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceStopError(distributorServiceTestFacade.ApplicationName)), "stop error");
            Assert.AreEqual(ServiceStatus.Shutdown, this.distributorServiceTestFacade.Status);
        }

        [Test]
        public void StopServiceWithWarning()
        {
            mockListener.StopReturnsFalse = true;

            this.distributorServiceTestFacade.OnStart();
            this.distributorServiceTestFacade.OnStop();

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceStopWarning(distributorServiceTestFacade.ApplicationName)), "stop warning");
        }

        [Test]
        public void PauseAndContinueService()
        {
            this.distributorServiceTestFacade.OnStart();

            this.distributorServiceTestFacade.OnPause();
            Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status, "status");
            Assert.IsTrue(mockListener.StopCalled, "mock stop called");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServicePausedSuccess(distributorServiceTestFacade.ApplicationName)), "start complete");
            // TODO: assert WMI event fired

            this.distributorServiceTestFacade.OnContinue();
            Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status);
            Assert.IsTrue(mockListener.StartCalled, "mock start called");
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceResumeComplete(distributorServiceTestFacade.ApplicationName)), "stop complete");
        }

        [Test]
        public void PauseServiceWithWarning()
        {
            mockListener.StopReturnsFalse = true;

            this.distributorServiceTestFacade.OnStart();
            this.distributorServiceTestFacade.OnPause();

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServicePauseWarning(distributorServiceTestFacade.ApplicationName)), "stop warning");
            Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status);
        }

        [Test]
        public void PauseServiceWithError()
        {
            mockListener.ExceptionOnStop = true;

            this.distributorServiceTestFacade.OnStart();
            this.distributorServiceTestFacade.OnPause();

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServicePauseError(distributorServiceTestFacade.ApplicationName)), "stop warning");
            Assert.AreEqual(ServiceStatus.Shutdown, this.distributorServiceTestFacade.Status);
        }

        [Test]
        public void ContinueServiceWithError()
        {
            this.distributorServiceTestFacade.OnStart();
            this.distributorServiceTestFacade.OnPause();
            mockListener.ExceptionOnStart = true;
            this.distributorServiceTestFacade.OnContinue();

            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceResumeError(distributorServiceTestFacade.ApplicationName)), "continue error");
            Assert.AreEqual(ServiceStatus.Shutdown, this.distributorServiceTestFacade.Status);
        }
    }
}

#endif