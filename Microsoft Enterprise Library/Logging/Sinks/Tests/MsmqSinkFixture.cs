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
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    [TestFixture]
    public class MsmqSinkFixture
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            CommonUtil.DeletePrivateTestQ();
            CommonUtil.CreatePrivateTestQ();
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
        public void Constructor()
        {
            MsmqSink sink = CreateSink(new MsmqSinkData("MsmqSink"));

            Assert.IsNotNull(sink);
        }

        [Test]
        public void LogMessageToMSMQ()
        {
            string path = @"FormatName:DIRECT=OS:" + CommonUtil.MessageQueuePath;

            MsmqSinkData sinkParams = new MsmqSinkData("MsmqSink");
            sinkParams.QueuePath = path;
            sinkParams.MessagePriority = MessagePriority.Low;
            MsmqSink sink = CreateSink(sinkParams);

            CommonUtil.SendTestMessage(sink);

            MessageQueue mq = new MessageQueue(path);
            mq.MessageReadPropertyFilter.Priority = true;
            Message msg = mq.Receive(new TimeSpan(0, 0, 1));
            msg.Formatter = new XmlMessageFormatter(new string[] {"System.String,mscorlib"});

            mq.Close();
            mq = null;
            Assert.AreEqual(MessagePriority.Low, msg.Priority);
            Assert.AreEqual(CommonUtil.FormattedMessage, msg.Body.ToString());
        }

        [Test]
		[ExpectedException(typeof(ArgumentException), "Path syntax is invalid.")]
        public void InvalidQueuePath()
        {
            string path = @"INVALID PATH";

            MsmqSinkData sinkParams = new MsmqSinkData("MsmqSink");
            sinkParams.QueuePath = path;
            MsmqSink sink = CreateSink(sinkParams);
            sink.SendMessage(new LogEntry());
        }

        [Test]
		[ExpectedException(typeof(MessageQueueException))]
        public void QueueDoesNotExist()
        {
            // delete the queue
            CommonUtil.DeletePrivateTestQ();

            MsmqSinkData sinkParams = new MsmqSinkData("MsmqSink");
            sinkParams.QueuePath = CommonUtil.MessageQueuePath;
            MsmqSink sink = CreateSink(sinkParams);

			try
			{
				CommonUtil.SendTestMessage(sink);
			}
            finally
			{
				CommonUtil.CreatePrivateTestQ();
			}
        }

        private MsmqSink CreateSink(MsmqSinkData msmqSinkData)
        {
            MsmqSink msmqSink = new MsmqSink();
            msmqSink.Initialize(new TestLogSinkConfigurationView(msmqSinkData));
            return msmqSink;
        }

    }
}

#endif
