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

#if  LONG_RUNNING_TESTS
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    /// <summary>
    /// Black box testing of the distributor service using the service controller
    /// </summary>
    [TestFixture]
    public class DistributorServiceExternalFixture
    {
        private const string DefaultApplicationName = "Enterprise Library Logging Distributor Service";

        private string installUtilPath = MsCorLibDirectory + @"\installutil";
        private ServiceController serviceController;
        private int serviceCommandTimeoutSecs = 60;
        private int origCount;

        /// <devdoc>
        /// Get the directory for current NDP
        /// </devdoc>
        private static string MsCorLibDirectory
        {
            get
            {
                // location of mscorlib.dll
                string filename;
                filename = Assembly.GetAssembly(typeof(object)).Location.Replace('/', '\\');
                return Path.GetDirectoryName(filename);
            }
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            CommonUtil.SetDistributionStrategy("Msmq");

            CommonUtil.ExecuteProcess(this.installUtilPath, "/u MsmqDistributor.exe");

            this.serviceController = new ServiceController(DefaultApplicationName);

            CommonUtil.DeletePrivateTestQ();
            CommonUtil.CreatePrivateTestQ();
            Thread.Sleep(1000);
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            CommonUtil.DeletePrivateTestQ();
            CommonUtil.SetDistributionStrategy("InProc");
        }

        [SetUp]
        public void Setup()
        {
            CommonUtil.ExecuteProcess(this.installUtilPath, "MsmqDistributor.exe");
            CommonUtil.ResetEventLogCounterCustom();
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.ExecuteProcess(this.installUtilPath, "/u MsmqDistributor.exe");
        }

        [Test]
        public void StartAndStopService()
        {
            // start and stop the service using service control manager
            StartService();

            Assert.AreEqual(ServiceControllerStatus.Running, this.serviceController.Status);
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceStartComplete(DefaultApplicationName)), "start complete failed");

            StopService();
            Assert.IsTrue(CommonUtil.LogEntryExists(SR.ServiceStopComplete(DefaultApplicationName)), "stop complete falied");
            Assert.AreEqual(ServiceControllerStatus.Stopped, this.serviceController.Status);
        }

        [Test]
        public void WriteToEventLog()
        {
            StartService();

            CommonUtil.ResetEventLogCounterCustom();
            this.origCount = CommonUtil.EventLogEntryCountCustom();

            Logger.Write("Test Message", "AppTest");

            BlockUntilEventWritten();
            Assert.AreEqual(1, CommonUtil.EventLogEntryCountCustom());

            string result = CommonUtil.GetLastEventLogEntryCustom();
            string expected = "<EntLibLog>\r\n\t<message>Test Message</message>\r\n\t<timestamp>";
            Assert.IsTrue(result.IndexOf(expected) > -1);

            Assert.AreEqual(ServiceControllerStatus.Running, this.serviceController.Status);
            StopService();
        }

        [Test]
        public void WriteTwoMessagesToEventLog()
        {
            StartService();

            CommonUtil.ResetEventLogCounterCustom();
            this.origCount = CommonUtil.EventLogEntryCountCustom();

            Logger.Write("Test Message", "AppTest");
            Logger.Write("Test Message", "AppTest");

            BlockUntilEventWritten();
            Assert.AreEqual(2, CommonUtil.EventLogEntryCountCustom());

            string result = CommonUtil.GetLastEventLogEntryCustom();
            string expected = "<EntLibLog>\r\n\t<message>Test Message</message>\r\n\t<timestamp>";
            Assert.IsTrue(result.IndexOf(expected) > -1);

            StopService();
        }

        [Test]
        public void PauseServiceThenWriteThenContinue()
        {
            StartService();

            PauseService();

            CommonUtil.ResetEventLogCounterCustom();

            Logger.Write("Test Message", "AppTest");

            ContinueService();

            BlockUntilEventWritten();
            Assert.AreEqual(1, CommonUtil.EventLogEntryCountCustom(), "log entry");

            StopService();
        }

        private void StartService()
        {
            this.serviceController.Start();
            WaitForServiceStatus(ServiceControllerStatus.Running);
        }

        private void PauseService()
        {
            this.serviceController.Pause();
            WaitForServiceStatus(ServiceControllerStatus.Paused);
        }

        private void ContinueService()
        {
            this.serviceController.Continue();
            WaitForServiceStatus(ServiceControllerStatus.Running);

            this.origCount = CommonUtil.EventLogEntryCountCustom();

        }

        private void StopService()
        {
            Assert.AreEqual(ServiceControllerStatus.Running, this.serviceController.Status);
            this.serviceController.Stop();
            WaitForServiceStatus(ServiceControllerStatus.Stopped);
        }

        private void WaitForServiceStatus(ServiceControllerStatus status)
        {
            TimeSpan timeOut = new TimeSpan(0, 0, 0, serviceCommandTimeoutSecs);
            this.serviceController.WaitForStatus(status, timeOut);
        }

        private void BlockUntilEventWritten()
        {
            int loopCounter = 0;
            int maxLoops = 200;
            int sleepAmount = 50;
            int count = CommonUtil.EventLogEntryCountCustom();

            while (count == this.origCount && loopCounter < maxLoops)
            {
                count = CommonUtil.EventLogEntryCountCustom();
                Thread.Sleep(sleepAmount);
                loopCounter++;
            }

            if (loopCounter == maxLoops)
            {
                Assert.Fail("Failed to write message to event log");
            }
        }
    }
}

#endif