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

#if UNIT_TESTS

using System;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    /// <summary>
    /// Summary description for MsmqSinkTransactionMessageBehaviorFixture.
    /// </summary>
    [TestFixture]
    public class MsmqSinkTransactionMessageBehaviorFixture
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            CommonUtil.DeletePrivateTestQ();
            CommonUtil.CreateTransactionalPrivateTestQ();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            CommonUtil.DeletePrivateTestQ();
        }

        [SetUp]
        public void SetUp()
        {
            CommonUtil.ResetEventLogCounter();
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.ResetEventLogCounter();
            MessageQueue.ClearConnectionCache();
        }

        [Test]
        public void LogMessageToMSMQ()
        {
            string path = @"FormatName:DIRECT=OS:" + CommonUtil.MessageQueuePath;

            MsmqSinkData sinkParams = new MsmqSinkData();
            sinkParams.QueuePath = path;

            MsmqSink sink = new MsmqSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            CommonUtil.SendTestMessage(sink);

            using (MessageQueue mq = new MessageQueue(path))
            {
                Message msg = mq.Receive(new TimeSpan(0, 0, 1));
                mq.Close();
                msg.Formatter = new XmlMessageFormatter(new string[] {"System.String,mscorlib"});
                Assert.AreEqual(CommonUtil.FormattedMessage, msg.Body.ToString());
            }
        }

        [Test]
        public void LogTwoMessagesToQueueAndBeAbleToReadBothOfThem()
        {
            string path = @"FormatName:DIRECT=OS:" + CommonUtil.MessageQueuePath;

            MsmqSinkData sinkParams = new MsmqSinkData();
            sinkParams.QueuePath = path;

            MsmqSink sink = new MsmqSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            CommonUtil.SendTestMessage(sink);

            using (MessageQueue mq = new MessageQueue(path))
            {
                Message msg = mq.Receive(new TimeSpan(0, 0, 1));
                mq.Close();
                msg.Formatter = new XmlMessageFormatter(new string[] {"System.String,mscorlib"});
                Assert.AreEqual(CommonUtil.FormattedMessage, msg.Body.ToString());
            }

            CommonUtil.SendTestMessage(sink);
            using (MessageQueue mq = new MessageQueue(path))
            {
                Message msg = mq.Receive(new TimeSpan(0, 0, 1));
                mq.Close();
                msg.Formatter = new XmlMessageFormatter(new string[] {"System.String,mscorlib"});
                Assert.AreEqual(CommonUtil.FormattedMessage, msg.Body.ToString());
            }
        }

    }
}

#endif