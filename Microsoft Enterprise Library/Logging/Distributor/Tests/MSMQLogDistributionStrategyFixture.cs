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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests
{
    [TestFixture]
    public class MSMQLogDistributionStrategyFixture
    {
        private MsmqLogDistributionStrategy msmq;

        [SetUp]
        public void SetUp()
        {
            CommonUtil.DeletePrivateTestQ();
            CreateTestQueue();

            msmq = new MsmqLogDistributionStrategy();
            msmq.Initialize(new TestLoggingConfigurationView(new ConfigurationContext(new ConfigurationDictionary())));

        }

        protected virtual void CreateTestQueue()
        {
            CommonUtil.CreatePrivateTestQ();
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.DeletePrivateTestQ();
        }

        [Test]
        public void MSMQLogDistStrategyConstructor()
        {
            Assert.IsNotNull(msmq);
        }

        [Test]
        public void SerializeMessageToMSMQ()
        {
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();

            msmq.SendLog(logEntry);

            string path = @"FormatName:DIRECT=OS:" + CommonUtil.MessageQueuePath;
            MessageQueue mq = new MessageQueue(path);
            Message msg = mq.Receive(new TimeSpan(0, 0, 1));
            msg.Formatter = new XmlMessageFormatter(new string[] {"System.String,mscorlib"});

            mq.Close();
            mq = null;

            string actual = msg.Body.ToString();

            Assert.IsTrue(actual.IndexOf(CommonUtil.MsgCategory) > 0);
            Assert.IsTrue(actual.IndexOf(logEntry.Message) > 0);
        }

        [Test]
        public void SerializeDictionaryToMSMQ()
        {
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.Category = "foo";
            logEntry.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            msmq.SendLog(logEntry);

            string path = @"FormatName:DIRECT=OS:" + CommonUtil.MessageQueuePath;
            MessageQueue mq = new MessageQueue(path);
            Message msg = mq.Receive(new TimeSpan(0, 0, 1));
            msg.Formatter = new XmlMessageFormatter(new string[] {"System.String,mscorlib"});

            mq.Close();

            string actual = msg.Body.ToString();

            Assert.IsTrue(actual.IndexOf(CommonUtil.MsgCategory) > 0);
            Assert.IsTrue(actual.IndexOf(logEntry.Message) > 0);
            Assert.IsTrue(actual.IndexOf(logEntry.ExtendedProperties["key1"].ToString()) > 0);
        }

        private class TestLoggingConfigurationView : LoggingConfigurationView
        {
            public TestLoggingConfigurationView(ConfigurationContext context) : base(context)
            {
            }

            public override DistributionStrategyData GetDistributionStrategyData(string name)
            {
                MsmqDistributionStrategyData data = new MsmqDistributionStrategyData();
                data.QueuePath = CommonUtil.MessageQueuePath;
                return data;
            }

        }
    }
}

#endif