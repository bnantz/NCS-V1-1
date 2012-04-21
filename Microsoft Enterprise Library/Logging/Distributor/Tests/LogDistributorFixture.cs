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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests
{
	[TestFixture]
	public class LogDistributorFixture : ConfigurationContextFixtureBase
	{
		private LogDistributor logDistributor;

		[SetUp]
		public void SetUp()
		{
			logDistributor = new LogDistributor(Context);
		}

		[TearDown]
		public void TearDown()
		{
			MockLogSink.Clear();
		}

		[Test]
		public void ProcessLogsSyncConstructor()
		{
			Assert.IsNotNull(logDistributor);
		}

		[Test]
		public void SendMessageToOneSink()
		{
			const string category = "MockCategoryOne";

			//  build a message
			LogEntry msg = CommonUtil.GetDefaultLogEntry();
			msg.Category = category;

			logDistributor.ProcessLog(msg);

			Assert.AreEqual(1, MockLogSink.Count);
			Assert.AreEqual(CommonUtil.MsgBody, MockLogSink.GetLastEntry().Message, "Body");
			Assert.AreEqual(CommonUtil.MsgTitle, MockLogSink.GetLastEntry().Title, "Header");
			Assert.AreEqual(category, MockLogSink.GetLastEntry().Category, "Category");
			Assert.AreEqual(CommonUtil.MsgEventID, MockLogSink.GetLastEntry().EventId, "EventID");
			Assert.AreEqual(Severity.Unspecified, MockLogSink.GetLastEntry().Severity, "Severity");
		}

		[Test]
		public void SendMessageToManySinks()
		{
			const string category = "MockCategoryMany";

			//  build a message
			LogEntry msg = CommonUtil.GetDefaultLogEntry();
			msg.Category = category;

			logDistributor.ProcessLog(msg);

			Assert.AreEqual(2, MockLogSink.Count);
			Assert.AreEqual(CommonUtil.MsgBody, MockLogSink.GetLastEntry().Message, "Body");
			Assert.AreEqual(CommonUtil.MsgBody, MockLogSink.GetLastEntry().Message, "Body");
		}

		[Test]
		public void MissingDefaultFormatter()
		{
			DistributorSettings settings = (DistributorSettings) Context.GetConfiguration(DistributorSettings.SectionName);

			string originalFormatter = settings.DefaultFormatter;

			settings.DefaultFormatter = null;
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Category = "AppError";
			logDistributor.ProcessLog(entry);

			string eventLogEntry = CommonUtil.GetLastEventLogEntry();

			Assert.IsTrue(eventLogEntry.IndexOf(SR.MissingDefaultFormatter) > -1, "formatter message");
			Assert.AreEqual(entry.Message, MockLogSink.GetLastEntry().Message, "message");
			Assert.AreEqual(entry.Category, MockLogSink.GetLastEntry().Category, "categry");

			settings.DefaultFormatter = originalFormatter;
		}

		[Test]
		public void WriteEntryWithMissingDefaultFormatter()
		{
			DistributorSettings settings = (DistributorSettings) ConfigurationManager.GetConfiguration(DistributorSettings.SectionName);

			string originalFormatter = settings.DefaultFormatter;

			settings.DefaultFormatter = null;
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Category = "AppTest";

			Logger.Write(entry);

			EventLog log = new EventLog(CommonUtil.EventLogName);
			string actual = log.Entries[log.Entries.Count - 2].Message;

			Assert.IsTrue(actual.IndexOf(SR.MissingDefaultFormatter) > -1, "formatter message");

			settings.DefaultFormatter = originalFormatter;
		}
	}
}

#endif