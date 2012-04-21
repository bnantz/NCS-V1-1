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
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
	[TestFixture]
	public class LogEntryFixture
	{
		private LogEntry log;

		[SetUp]
		public void SetUp()
		{
			log = new LogEntry();
		}

		[TearDown]
		public void TearDown()
		{
			MockLogSink.Clear();
		}

		[Test]
		public void GetSetProperties()
		{
			string stringVal = "my test string";
			int counter = 0;

			log.Category = stringVal + counter;
			Assert.AreEqual(stringVal + counter, log.Category);
			counter++;
			log.Category = "";
			Assert.AreEqual("", log.Category);

			log.EventId = counter;
			Assert.AreEqual(counter, log.EventId);
			log.EventId = -1234;
			Assert.AreEqual(-1234, log.EventId);

			log.Message = stringVal + counter;
			Assert.AreEqual(stringVal + counter, log.Message);
			counter++;
			log.Message = "";
			Assert.AreEqual("", log.Message);

			log.Priority = counter;
			Assert.AreEqual(counter, log.Priority);
			log.Priority = -1234;
			Assert.AreEqual(-1234, log.Priority);

			log.Severity = Severity.Warning;
			Assert.AreEqual(Severity.Warning, log.Severity);
			counter++;
			log.Severity = Severity.Unspecified;
			Assert.AreEqual(Severity.Unspecified, log.Severity);

			log.TimeStamp = DateTime.MinValue;
			Assert.AreEqual(DateTime.MinValue, log.TimeStamp);
			counter++;
			log.TimeStamp = DateTime.MaxValue;
			Assert.AreEqual(DateTime.MaxValue, log.TimeStamp);

			log.Title = stringVal + counter;
			Assert.AreEqual(stringVal + counter, log.Title);
			counter++;
			log.Title = "";
			Assert.AreEqual("", log.Title);
		}

		[Test]
		public void ConfirmThatIntrinsicPropertiesAreReadWrite()
		{
			log.TimeStamp = DateTime.MinValue;
			Assert.AreEqual(DateTime.MinValue, log.TimeStamp);

			log.MachineName = "MachineName";
			Assert.AreEqual("MachineName", log.MachineName);

			log.AppDomainName = "AppDomainName";
			Assert.AreEqual("AppDomainName", log.AppDomainName);

			log.ProcessId = "ProcessId";
			Assert.AreEqual("ProcessId", log.ProcessId);

			log.ProcessName = "ProcessName";
			Assert.AreEqual("ProcessName", log.ProcessName);

			log.ManagedThreadName = "ManagedThreadId";
			Assert.AreEqual("ManagedThreadId", log.ManagedThreadName);

			log.Win32ThreadId = "Win32ThreadId";
			Assert.AreEqual("Win32ThreadId", log.Win32ThreadId);
		}

		[Test]
		public void GetSetExtendedPropertiesProperty()
		{
			Hashtable hash = new Hashtable();
			hash["key1"] = "val1";
			hash["key2"] = "val2";

			log.ExtendedProperties = hash;
			Assert.AreEqual(2, log.ExtendedProperties.Count);
			Assert.AreEqual("val1", log.ExtendedProperties["key1"]);
			Assert.AreEqual("val2", log.ExtendedProperties["key2"]);
		}

		[Test]
        public void CanSetExtendedPropertiesWithoutProvidingHashTableFirst()
        {
            log.ExtendedProperties["foo"] = "bar";
            Assert.AreEqual(1, log.ExtendedProperties.Count);
            Assert.AreEqual("bar", log.ExtendedProperties["foo"]);
        }

		[Test]
		public void GetSetTimeStampString()
		{
			string expected = "12/31/9999 11:59:59 PM";
			log.TimeStamp = DateTime.MaxValue;
			Assert.AreEqual(expected, log.TimeStampString);
		}

		[Test]
		public void ConfirmIntrinsicPropertiesContainExpectedValues()
		{
			// NOTE: There is no way to test Timestamp

			Assert.AreEqual(Environment.MachineName, log.MachineName);
			Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, log.AppDomainName);
			Assert.AreEqual(NativeMethods.GetCurrentProcessId().ToString(), log.ProcessId);
			Assert.AreEqual(GetExpectedProcessName(), log.ProcessName);
			Assert.AreEqual(Thread.CurrentThread.Name, log.ManagedThreadName);
			Assert.AreEqual(NativeMethods.GetCurrentThreadId().ToString(), log.Win32ThreadId);
		}

		[Test]
		public void ReuseLogEntryForMultipleWrites()
		{
			log.Category = "MockCategoryOne";

			log.Message = "apples";
			Logger.Write(log);
			Assert.AreEqual("apples", MockLogSink.GetLastEntry().Message);

			log.Message = "oranges";
			Logger.Write(log);
			Assert.AreEqual("oranges", MockLogSink.GetLastEntry().Message);
		}

		[Test]
		public void ConfirmCloneWorksWithICloneableExtendedProperties()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = new Hashtable();
			entry.ExtendedProperties.Add("one", "two");

			LogEntry entry2 = entry.Clone() as LogEntry;
			Assert.IsNotNull(entry2);
			Assert.AreEqual("two", entry2.ExtendedProperties["one"]);
		}

		[Test]
		public void ConfirmCloningNonCloneableExtendedPropertiesReplacesExtendedPropertiesCollection()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
            ListDictionary extendedProperties = new ListDictionary();
			entry.ExtendedProperties = extendedProperties;
			entry.ExtendedProperties.Add("one", "two");

			LogEntry entry2 = entry.Clone() as LogEntry;
			Assert.IsNotNull(entry2);
            Assert.IsFalse(object.ReferenceEquals(extendedProperties, entry2.ExtendedProperties));
			Assert.AreEqual(0, entry2.ExtendedProperties.Count);
		}

		private string GetExpectedProcessName()
		{
			StringBuilder buffer = new StringBuilder(1024);
			NativeMethods.GetModuleFileName(NativeMethods.GetModuleHandle(null), buffer, buffer.Capacity);
			return buffer.ToString();
		}

	}
}

#endif
