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
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
	[TestFixture]
	public class TracerFixture
	{
		private const string testActivityId1 = "1CF75C3C-127F-41c9-97B2-FBEDB64F974A";
		private const string testActivityId2 = "5D352811-28DF-4baf-BD18-5997FE1260A3";
		private const string testCategory1 = "TraceCategory1";
		private const string testCategory2 = "TraceCategory2";

		private ConfigurationContext Context = new TestConfigurationContext();

		[SetUp]
		public void SetUp()
		{
		}

		[TearDown]
		public void TearDown()
		{
			MockLogSink.Clear();
		}

		[Test]
		public void ConstructorProducesValidLogMessage()
		{
			using (new Tracer(Context, testCategory1, testActivityId1))
			{
			}

			Assert.AreEqual(2, MockLogSink.Count);
			AssertLogEntryIsValid(MockLogSink.GetEntry(0), Tracer.startTitle, testCategory1, testActivityId1, true);
			AssertLogEntryIsValid(MockLogSink.GetEntry(1), Tracer.endTitle, testCategory1, testActivityId1, false);
		}

		[Test]
		public void ActivityIdsAreUniqueOnEachThread()
		{
			// This will put two events into the sink using testCategory2 and testActivityId2
			using (new Tracer(Context, testCategory2, testActivityId2))
			{
				// This will put two events into the sink using testCategory1 and testActivityId1
				CrossThreadTestRunner t = new CrossThreadTestRunner( new ThreadStart(this.DoOtherThreadWork));
				t.Run();

				// Confirm that the Tracer on this thread has the expected activityID
				Assert.AreEqual(testActivityId2, Tracer.CurrentActivityId);
			}

			Assert.AreEqual(null, Tracer.CurrentActivityId);

			Assert.AreEqual(4, MockLogSink.Count);
			AssertLogEntryIsValid(MockLogSink.GetEntry(0), Tracer.startTitle, testCategory2, testActivityId2, true);
			AssertLogEntryIsValid(MockLogSink.GetEntry(1), Tracer.startTitle, testCategory1, testActivityId1, true);
			AssertLogEntryIsValid(MockLogSink.GetEntry(2), Tracer.endTitle, testCategory1, testActivityId1, false);
			AssertLogEntryIsValid(MockLogSink.GetEntry(3), Tracer.endTitle, testCategory2, testActivityId2, false);
		}

		[Test]
		public void NestedConstructorsMaintainIndependentActivityIds()
		{
			using (new Tracer(Context, testCategory1, testActivityId1))
			{
				Assert.AreEqual(testActivityId1, Tracer.CurrentActivityId);
				using (new Tracer(Context, testCategory1, testActivityId2))
				{
					Assert.AreEqual(testActivityId2, Tracer.CurrentActivityId);
				}
				Assert.AreEqual(testActivityId1, Tracer.CurrentActivityId);
			}
		}

		[Test]
		public void NestedConstructorProducesFourCorrectLogMessages()
		{
			using (new Tracer(Context, testCategory1, null))
			{
				string activityId = Tracer.CurrentActivityId;
				using (new Tracer(Context, testCategory1, null))
				{
					Assert.AreEqual( activityId, Tracer.CurrentActivityId);
				}
			}

			Assert.AreEqual(4, MockLogSink.Count);
			AssertLogEntryIsValid(MockLogSink.GetEntry(0), Tracer.startTitle, testCategory1, null, true);
			AssertLogEntryIsValid(MockLogSink.GetEntry(1), Tracer.startTitle, testCategory1, null, true);
			AssertLogEntryIsValid(MockLogSink.GetEntry(2), Tracer.endTitle, testCategory1, null, false);
			AssertLogEntryIsValid(MockLogSink.GetEntry(3), Tracer.endTitle, testCategory1, null, false);
		}

		[Test]
		public void OmittingActivityIdDoesntOverwriteExistingActivityId()
		{
			using (Tracer tracer1 = new Tracer(Context, testCategory1, testActivityId1))
			{
				Assert.IsNotNull(tracer1);
				using (Tracer tracer2 = new Tracer(Context, testCategory1, null))
				{
					Assert.IsNotNull(tracer2);
					Assert.AreEqual(testActivityId1, Tracer.CurrentActivityId);
				}
			}
		}

		[Test]
		public void StacksAreNullWhenNotInTracer()
		{
			Assert.IsNull(Tracer.CurrentCategory);
			Assert.IsNull(Tracer.RootCategory);
		}

		[Test]
		public void LoggerWithEmptyCategoryStackDoesntThrowException()
		{
			Assert.IsNull(Tracer.CurrentCategory);
			Assert.IsNull(Tracer.RootCategory);
			Logger.Write("Foo", Tracer.CurrentCategory);
			Logger.Write("Foo", Tracer.RootCategory);
		}

		[Test]
		public void LogMessageContainsProperMethodName()
		{
			using (Tracer tracer = new Tracer(Context, Tracer.defaultCategory, null))
			{
				Assert.IsNotNull(tracer);
			}

			Assert.AreEqual(2, MockLogSink.Count);
			AssertStringContainsString(MockLogSink.GetEntry(0).Message, MethodInfo.GetCurrentMethod().Name);
			AssertStringContainsString(MockLogSink.GetEntry(1).Message, MethodInfo.GetCurrentMethod().Name);
		}

		private void AssertStringContainsString(string actual, string substring)
		{
			if (actual.IndexOf(substring) == -1)
			{
				Assert.Fail(String.Format("String \n\t{0}\ndoes not contain string\n\t{1}\n", actual, substring));
			}
		}

		private void AssertLogEntryIsValid(LogEntry entry, string expectedTitle, string expectedCategory, string expectedActivityId, bool isStartMessage)
		{
			Assert.AreEqual(expectedTitle, entry.Title);
			Assert.AreEqual(expectedCategory, entry.Category);

			if (expectedActivityId != null)
			{
				Assert.AreEqual(expectedActivityId, entry.ExtendedProperties[Tracer.ActivityIdPropertyKey].ToString());
			}

			Assert.AreEqual(Tracer.eventId, entry.EventId);
			Assert.AreEqual(Tracer.priority, entry.Priority);
			Assert.AreEqual(Tracer.eventId, entry.EventId);
			Assert.AreEqual(Tracer.severity, entry.Severity);

			if (isStartMessage)
			{
				AssertMessageIsValidStartMessage(entry.Message);
			}
			else
			{
				AssertMessageIsValidEndMessage(entry.Message);
			}
		}

		private void AssertMessageIsValidStartMessage(string message)
		{
			Assert.IsNotNull(message);

			string format = SR.Keys.GetString(SR.Keys.Tracer_StartMessageFormat);
			string pattern = ConvertFormatToRegex(format);

			Regex re = new Regex(pattern);

			Assert.IsTrue(re.IsMatch(message));

			MatchCollection matches = re.Matches(message);
			foreach (Match match in matches)
			{
				Assert.IsNotNull(match.Value);
				Assert.IsTrue(match.Value.ToString().Length > 0);
			}
		}

		private void AssertMessageIsValidEndMessage(string message)
		{
			Assert.IsNotNull(message);

			string format = SR.Keys.GetString(SR.Keys.Tracer_EndMessageFormat);
			string pattern = ConvertFormatToRegex(format);

			Regex re = new Regex(pattern);

			Assert.IsTrue(re.IsMatch(message));

			MatchCollection matches = re.Matches(message);
			foreach (Match match in matches)
			{
				Assert.IsNotNull(match.Value);
				Assert.IsTrue(match.Value.ToString().Length > 0);
			}
		}

		private void DoOtherThreadWork()
		{
			using (Tracer tracer = new Tracer(Context, testCategory1, testActivityId1))
			{
				Assert.IsNotNull(tracer);
				Assert.AreEqual(testActivityId1, Tracer.CurrentActivityId);
			}
		}

		private string ConvertFormatToRegex(string format)
		{
			string pattern = format;
			pattern = pattern.Replace("(", @"\(");
			pattern = pattern.Replace(")", @"\)");
			pattern = Regex.Replace(pattern, @"\{[0-9]\}", "(.*?)");
			return pattern;
		}
	}
}

#endif