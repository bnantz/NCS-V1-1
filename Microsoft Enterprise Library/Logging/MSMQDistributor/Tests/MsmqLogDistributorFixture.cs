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
using System.Collections;
using System.Messaging;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    [TestFixture]
    public class MsmqLogDistributorFixture : ConfigurationContextFixtureBase
    {
        private MsmqLogDistributor msmqDistributor;
        private MsmqDistributionStrategyData data;
        private MsmqLogDistributionStrategy msmq;


        [SetUp]
        public void SetUp()
        {
            data = new MsmqDistributionStrategyData();
            data.QueuePath = CommonUtil.MessageQueuePath;

            CommonUtil.DeletePrivateTestQ();
            CreateQueueForTesting();
            msmqDistributor = new MsmqLogDistributor(Context);
            msmqDistributor.StopReceiving = false;

            msmq = new MsmqLogDistributionStrategy();
            msmq.Initialize(new TestLoggingConfigurationView(data, Context));
        }

		protected virtual void CreateQueueForTesting()
		{
			CommonUtil.CreatePrivateTestQ();
		}

        [TearDown]
        public void TearDown()
        {
            CommonUtil.DeletePrivateTestQ();
        	MockLogSink.Clear();
        }

        [Test]
        public void Constructor()
        {
            msmqDistributor = new MsmqLogDistributor(Context);

            Assert.IsNotNull(msmqDistributor);
        }

        [Test]
        public void ReceiveMSMQMessage()
        {
            SendMessageToQ(CommonUtil.MsgBody);

            msmqDistributor.CheckForMessages();

            Assert.AreEqual(1, MockLogSink.Count);
            Assert.AreEqual(CommonUtil.MsgBody, MockLogSink.GetLastEntry().Message, "Body");
        }

        [Test]
        public void ReceiveTwoMessages()
        {
            SendMessageToQ(CommonUtil.MsgBody);
            SendMessageToQ(CommonUtil.MsgBody + " 4 5 6");

			Assert.AreEqual(2, CommonUtil.GetNumberOfMessagesOnQueue());

            msmqDistributor.CheckForMessages();

			Assert.AreEqual(0, CommonUtil.GetNumberOfMessagesOnQueue());

            // confirm that the second message was processed by the sink
            Assert.AreEqual(2, MockLogSink.Count);
            Assert.AreEqual(CommonUtil.MsgBody + " 4 5 6", MockLogSink.GetLastEntry().Message);
        }

        [Test]
        public void SendTwoMessagesWithPauseReceiving()
        {
            SendMessageToQ(CommonUtil.MsgBody);
            SendMessageToQ(CommonUtil.MsgBody + " 4 5 6");

            // By setting StopRecieving = true, only one message will be processed from the Q
            msmqDistributor.StopReceiving = true;
            msmqDistributor.CheckForMessages();

            // confirm that the second message was NOT processed by the sink
            Assert.AreEqual(1, MockLogSink.Count);
            Assert.AreEqual(CommonUtil.MsgBody, MockLogSink.GetLastEntry().Message);

            msmqDistributor.CheckForMessages();
        }

        [Test]
        public void SendCustomLogEntryViaMsmq()
        {
            CustomLogEntry log = new CustomLogEntry();
            log.TimeStamp = DateTime.MaxValue;
            log.Title = "My custom message title";
            log.Message = "My custom message body";
            log.Category = "CustomFormattedCategory";
            log.AcmeCoField1 = "apple";
            log.AcmeCoField2 = "orange";
            log.AcmeCoField3 = "lemon";

            msmq.SendLog(log);

            msmqDistributor.CheckForMessages();

            string expected = "Timestamp: 12/31/9999 11:59:59 PM\r\nTitle: My custom message title\r\n\r\nAcme Field1: apple\r\nAcme Field2: orange\r\nAcme Field3: lemon\r\n\r\nMessage: My custom message body";
            string actual = CommonUtil.GetLastEventLogEntryCustom();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SendLogEntryViaMsmq()
        {
            LogEntry log = new LogEntry();
            log.TimeStamp = DateTime.MaxValue;
            log.Title = "My custom message title";
            log.Message = "My custom message body";
            log.Category = "FormattedCategory";

            msmq.SendLog(log);

            msmqDistributor.CheckForMessages();

            string expected = "12/31/9999 11:59:59 PM: My custom message title\r\n\r\nMy custom message body";
            string actual = CommonUtil.GetLastEventLogEntryCustom();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SendLogEntryWithDictionaryViaMsmq()
        {
            LogEntry log = new LogEntry();
            log.TimeStamp = DateTime.MaxValue;
            log.Title = "My custom message title";
            log.Message = "My custom message body";
            log.Category = "AppTest";
            log.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            msmq.SendLog(log);

            msmqDistributor.CheckForMessages();

            string expected = "<EntLibLog>\r\n\t<message>My custom message body</message>\r\n\t<timestamp>12/31/9999 11:59:59 PM</timestamp>\r\n\t<title>My custom message title</title>\r\n</EntLibLog>";
            string actual = CommonUtil.GetLastEventLogEntryCustom();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SendDictionaryWithNestedInvalidXml()
        {
            LogEntry log = new LogEntry();
            log.TimeStamp = DateTime.MaxValue;
            log.Title = "My custom message title";
            log.Message = "My custom message body";
            log.Category = "DictionaryCategory";
            Hashtable hash = new Hashtable();
            hash["key1"] = "value1";
            hash["key2"] = "<xml>my values<field1>INVALID ><><XML</field2></xml>";
            hash["key3"] = "value3";
            log.ExtendedProperties = hash;

            msmq.SendLog(log);

            msmqDistributor.CheckForMessages();

            string expected = "Timestamp: 12/31/9999 11:59:59 PM\r\nTitle: My custom message title\r\n\r\nMessage: My custom message body\r\n\r\nExtended Properties:\r\nKey: key1\t\tValue: value1\r\nKey: key3\t\tValue: value3\r\nKey: key2\t\tValue: <xml>my values<field1>INVALID ><><XML</field2></xml>\r\n";
            string actual = CommonUtil.GetLastEventLogEntryCustom();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MSMQInvalidTransactionUsage()
        {
            MsmqReceiverTestWrapper testSync = new MsmqReceiverTestWrapper(Context);
            testSync.LogMsgQueueException(MessageQueueErrorCode.TransactionUsage);

            string expected = SR.MsmqInvalidTransactionUsage(CommonUtil.MessageQueuePath);

            //assert warning is written to event log			
            Assert.IsTrue(CommonUtil.GetLastEventLogEntry().IndexOf(expected) > -1);
        }

        [Test]
        public void MsmqRecieveTimeout()
        {
            MsmqReceiverTestWrapper testSync = new MsmqReceiverTestWrapper(Context);
            testSync.LogMsgQueueException(MessageQueueErrorCode.IOTimeout);

            string expected = SR.MsmqReceiveTimeout(CommonUtil.MessageQueuePath);

            //assert warning is written to event log			
            string actual = CommonUtil.GetLastEventLogEntry();
            Assert.IsTrue(actual.IndexOf(expected) > -1);
        }

        [Test]
        public void MsmqAccessDenied()
        {
            MsmqReceiverTestWrapper testSync = new MsmqReceiverTestWrapper(Context);
            testSync.LogMsgQueueException(MessageQueueErrorCode.AccessDenied);

            string expected = SR.MsmqAccessDenied(CommonUtil.MessageQueuePath, WindowsIdentity.GetCurrent().Name);
            string actual = CommonUtil.GetLastEventLogEntry();
            Assert.IsTrue(actual.IndexOf(expected) > -1);
        }

        private class TestLoggingConfigurationView : LoggingConfigurationView
        {
            public MsmqDistributionStrategyData data;

            public TestLoggingConfigurationView(MsmqDistributionStrategyData data, ConfigurationContext context) : base(context)
            {
                this.data = data;
            }

            public override DistributionStrategyData GetDistributionStrategyData(string name)
            {
                return data;
            }

        }

        private void SendMessageToQ(string body)
        {
            //submit msg to queue
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.Category = "MockCategoryOne";
            logEntry.Message = body;

            msmq.SendLog(logEntry);
        }

        internal class MsmqReceiverTestWrapper : MsmqLogDistributor
        {
            public MsmqReceiverTestWrapper(ConfigurationContext context) : base(context)
            {
            }

            public void LogMsgQueueException(MessageQueueErrorCode code)
            {
                // a better approach would be to derive a new Exception type from
                // MessagequeueException and then throw it using a mock MessageQueue
                // However, this exception is protected and we cannot created a derived type
                base.LogMessageQueueException(code, new Exception("simulated exception"));
            }
        }
    }
}

#endif
