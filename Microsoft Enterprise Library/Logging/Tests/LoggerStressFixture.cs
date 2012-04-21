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

//#if  LONG_RUNNING_TESTS
#if  UNIT_TESTS
using System.Diagnostics;
using System.Messaging;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestFixture]
    public class LoggerStressFixture : ThreadStressFixtureBase
    {
        private const int threadCount = 100;
        private const int loopCount = 10;
        private const string customEventLog = "EntLib Tests";

        [SetUp]
        public void SetUp()
        {
            CommonUtil.CreatePrivateTestQ();
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.DeletePrivateTestQ();
        }

        [Test]
        public void SingleThreadToMsmqSink()
        {
            SendTestMessages();
            Thread.Sleep(500);
            int count = GetCount();
            Assert.AreEqual(loopCount, count);
        }

        [Test]
        public void MultiThreadToMsmqSink()
        {
            ThreadStart testMethod = new ThreadStart(SendTestMessages);

            base.ThreadStress(testMethod, threadCount);

            Thread.Sleep(500);

            int count = GetCount();
            Assert.AreEqual(threadCount * loopCount, count);
        }

        [Test]
        public void MultiThreadToEventLogSink()
        {
            using (EventLog log = new EventLog(customEventLog))
            {
                log.Clear();
            }

            ThreadStart testMethod = new ThreadStart(SendTestMessagesEventLog);

            base.ThreadStress(testMethod, threadCount);

            Thread.Sleep(500);

            int count = GetCountCustomEventLog();
            Assert.AreEqual(threadCount * loopCount, count);
        }

        [Test]
        public void SingleThreadMsmqDistributionStrategy()
        {
            CommonUtil.SetDistributionStrategy("Msmq");

            SingleThreadToMsmqSink();

            CommonUtil.SetDistributionStrategy("InProc");
        }

        [Test]
        public void MultiThreadToMsmqDistributionStrategy()
        {
            CommonUtil.SetDistributionStrategy("Msmq");

            MultiThreadToMsmqSink();

            CommonUtil.SetDistributionStrategy("InProc");
        }

        private void SendTestMessages()
        {
            for (int i = 0; i < loopCount; i++)
            {
                Logger.Write("test", "MsmqCategory");
            }
        }

        private void SendTestMessagesEventLog()
        {
            for (int i = 0; i < loopCount; i++)
            {
                Logger.Write("test", "AppTest");
            }
        }

        private int GetCount()
        {
            using(MessageQueue msmq = new MessageQueue(CommonUtil.MessageQueuePath))
            {
                Message[] messages = msmq.GetAllMessages();
                int count = messages.Length;
                msmq.Purge();
                msmq.Close();
                return count;
            }
        }

        private int GetCountCustomEventLog()
        {
            using (EventLog log = new EventLog(customEventLog))
            {
                return log.Entries.Count;
            }
        }
    }
}

#endif