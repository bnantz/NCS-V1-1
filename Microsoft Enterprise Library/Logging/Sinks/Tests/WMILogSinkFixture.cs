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
using System.Management;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    [TestFixture]
    public class WMILogSinkFixture
    {
        private WMILogSink sink;
        private bool wmiLogged = false;
        private string wmiResult = string.Empty;
        private string wmiPath = @"\root\EnterpriseLibrary";

        [SetUp]
        public void Setup()
        {
            this.sink = new WMILogSink();
            this.wmiLogged = false;
            this.wmiResult = string.Empty;
        }

        [Test]
        public void Constructor()
        {
            WMILogSink sink = new WMILogSink();
            Assert.IsNotNull(sink);
        }

        [Test]
        public void LogMessageToWMI()
        {
            ManagementScope scope = new ManagementScope(@"\\." + this.wmiPath);
            scope.Options.EnablePrivileges = true;

            string query = "SELECT * FROM LogEntry";
            EventQuery eq = new EventQuery(query);

            using (ManagementEventWatcher watcher = new ManagementEventWatcher(scope, eq))
            {
                watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
                watcher.Start();

                this.sink.SendMessage(CommonUtil.GetDefaultLogEntry());

                BlockUntilWMIEventArrives();

                watcher.Stop();
            }

            Assert.IsTrue(this.wmiLogged);
            Assert.IsTrue(this.wmiResult.IndexOf(CommonUtil.MsgBody) > -1);
        }

        [Test]
        public void TestLoggingACustomEntry()
        {
            ManagementScope scope = new ManagementScope(@"\\." + this.wmiPath);
            scope.Options.EnablePrivileges = true;

            string query = "SELECT * FROM MyCustomLogEntry";
            EventQuery eq = new EventQuery(query);

            using (ManagementEventWatcher watcher = new ManagementEventWatcher(scope, eq))
            {
                watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
                watcher.Start();

                this.sink.SendMessage(GetCustomLogEntry());

                BlockUntilWMIEventArrives();

                watcher.Stop();
            }

            Assert.IsTrue(this.wmiLogged);
            Assert.IsTrue(this.wmiResult.IndexOf(CommonUtil.MsgBody) > -1);
        }

        private static MyCustomLogEntry GetCustomLogEntry()
        {
            MyCustomLogEntry entry = new MyCustomLogEntry();
            entry.Message = CommonUtil.MsgBody;
            entry.Category = CommonUtil.MsgCategory;
            entry.EventId = CommonUtil.MsgEventID;
            entry.Title = CommonUtil.MsgTitle;
            entry.Severity = Severity.Error;
            entry.Priority = 100;
            entry.TimeStamp = DateTime.MaxValue;
            entry.MachineName = "machine";
            return entry;
        }

        private void BlockUntilWMIEventArrives()
        {
            // loop and poll the event watcher
            bool loop = true;
            int count = 0;
            int timeoutCount = 100;
            while (loop)
            {
                // keep looping until the event handler gets called or we reach our timeout
                loop = !this.wmiLogged && (count++ < timeoutCount);
                Thread.Sleep(100);
            }
        }

        private void watcher_EventArrived(object sender, EventArrivedEventArgs args)
        {
            wmiLogged = true;
            wmiResult = args.NewEvent.GetText(TextFormat.Mof);
        }
    }
}

#endif