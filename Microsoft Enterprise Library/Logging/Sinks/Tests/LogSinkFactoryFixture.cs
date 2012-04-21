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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests 
{
	[TestFixture]
    public class LogSinkFactoryFixture : ConfigurationContextFixtureBase
	{
		[Test]
		public void EmailSink()
		{
            string sinkName = "SampleEmailSink";
			EmailSinkData sink = new EmailSinkData();
			sink.Name = sinkName;

			SinkFromFactory(sink);
		}

		[Test]
		public void EventLogSink()
		{
            string sinkName = "ApplicationLogSink";
			EventLogSinkData sink = new EventLogSinkData();
			sink.Name = sinkName;

			SinkFromFactory(sink);
		}

		[Test]
		public void FlatFileSink()
		{
            string sinkName = "SampleTextFileSink";
			FlatFileSinkData sink = new FlatFileSinkData();
			sink.Name = sinkName;

			SinkFromFactory(sink);
		}

		[Test]
		public void MsmqSink()
		{
            string sinkName = "MsmqSink";
			MsmqSinkData sink = new MsmqSinkData();
			sink.Name = sinkName;

			SinkFromFactory(sink);
		}

		[Test]
		public void WmiSink()
		{
            string sinkName = "SampleInstrumentationSink";
			WMILogSinkData sink = new WMILogSinkData();
			sink.Name = sinkName;

			SinkFromFactory(sink);
		}

		private void SinkFromFactory(SinkData sink)
		{
            LogSinkFactory factory = new LogSinkFactory(Context);
            ILogSink output = factory.CreateSink(sink.Name);
                Assert.IsNotNull(output);
            }
		}
	}

#endif
