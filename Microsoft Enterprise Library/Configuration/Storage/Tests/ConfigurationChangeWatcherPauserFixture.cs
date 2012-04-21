//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Storage.Tests 
{
    [TestFixture]
    public class ConfigurationChangeWatcherPauserFixture 
    {
        [Test]
        public void WatchingSessionWillBeControlledThroughDisposePattern()
        {
            MockConfigurationChangeWatcher watcher = new MockConfigurationChangeWatcher();
            using(new ConfigurationChangeWatcherPauser(watcher))
            {
                ;
            }

            Assert.AreEqual("StoppedStarted", watcher.Log);
        }

        [Test]
        public void WillIgnoreNullWatcherReferenceAndNotCauseNullRefException()
        {
            using(new ConfigurationChangeWatcherPauser(null))
            {
                ;
            }
        }

        public class MockConfigurationChangeWatcher : IConfigurationChangeWatcher
        {
            private string log = "";

            public event ConfigurationChangedEventHandler ConfigurationChanged;

            public void Dispose() {}

            public void StartWatching() { log += "Started"; }

            public void StopWatching() { log += "Stopped"; }

            public string SectionName { get { return ""; }}

            public void NeverEverUsed()
            {
                ConfigurationChanged(this, null);
            }
          
            public string Log
            {
                get { return log; }
            }
        }
    }
}
#endif