//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if       UNIT_TESTS
using System.Management;
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    /// <summary>
    /// Summary description for WMIEventsFixtureBase.
    /// </summary>
    public class WMIEventsFixtureBase
    {
        protected bool wmiLogged = false;
        protected string wmiResult = string.Empty;
        private const string WmiPath = @"\root\EnterpriseLibrary";

        public delegate void WmiEventDelegate();

        public void FireMessageToWMI(string eventName, WmiEventDelegate action, string expected)
        {
            ManagementScope scope = new ManagementScope(@"\\." + WmiPath);
            scope.Options.EnablePrivileges = true;

            EventQuery eq = new EventQuery("SELECT * FROM " + eventName);

            using (ManagementEventWatcher watcher = new ManagementEventWatcher(scope, eq))
            {
                watcher.EventArrived += new EventArrivedEventHandler(WatcherEventArrived);
                watcher.Start();

                action();

                WaiUntilWMIEventArrives();
                watcher.Stop();
            }

            Assert.IsTrue(this.wmiLogged);
            Assert.IsTrue(this.wmiResult.IndexOf(expected) > -1);
        }

        private void WaiUntilWMIEventArrives()
        {
            // loop and poll the event watcher
            bool loop = true;
            int count = 0;
            int timeoutCount = 100;
            while (loop)
            {
                // keep looping until the event handler gets called or we reach our timeout
                loop = !this.wmiLogged && (count++ < timeoutCount);
                Thread.Sleep(250);
            }
        }

        private void WatcherEventArrived(object sender, EventArrivedEventArgs args)
        {
            this.wmiLogged = true;
            this.wmiResult = args.NewEvent.GetText(TextFormat.Mof);
        }
    }
}

#endif