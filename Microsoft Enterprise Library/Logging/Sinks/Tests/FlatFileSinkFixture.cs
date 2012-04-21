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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    [TestFixture]
    public class FlatFileSinkFixture
    {
        private FlatFileSink sink;

        [SetUp]
        public void Setup()
        {
            FlatFileSinkData sinkParams = new FlatFileSinkData();
            sinkParams.FileName = CommonUtil.FileName;
            sinkParams.Header = "";
            sinkParams.Footer = "";
            this.sink = new FlatFileSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));
            CommonUtil.ResetEventLogCounter();
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.ResetEventLogCounter();

            string directory = Path.GetDirectoryName(CommonUtil.FileName);
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        [Test]
        public void LogMessageToFlatFile()
        {
            CommonUtil.SendTestMessage(this.sink);

            FileStream stream = new FileStream(CommonUtil.FileName,
                                               FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            string contents = reader.ReadToEnd();
            stream.Close();

            Assert.AreEqual(0, CommonUtil.EventLogEntryCount(), "confirm no messages written to event log");

            Assert.IsTrue(contents.IndexOf(CommonUtil.FormattedMessage) > -1, "confirm message is part of contents");
        }

        [Test]
        public void LogTextFormattedMessageToFlatFile()
        {
            string template = "Timestamp: {timestamp}\n\nTitle: {title}\n\nBody: {message}";
            sink.Formatter = CommonUtil.CreateTextFormatter(template);

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			sink.SendMessage(entry);

            FileStream stream = new FileStream(CommonUtil.FileName,
                                               FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            string contents = reader.ReadToEnd();
            stream.Close();

            Assert.AreEqual(0, CommonUtil.EventLogEntryCount(), "confirm no messages written to event log");

            string expected = "Timestamp: " + entry.TimeStampString + "\n\nTitle: " + entry.Title + "\n\nBody: " + entry.Message;

            Assert.IsTrue(contents.IndexOf(expected) > -1, "confirm message is part of contents");
        }

        [Test]
        public void MissingRequiredParameters()
        {
            // create a flatfile sink without the required parameters
            FlatFileSink badSink = new FlatFileSink();
            badSink.Initialize(new TestLogSinkConfigurationView(new FlatFileSinkData()));
            CommonUtil.SendTestMessage(badSink);
            Assert.AreEqual(1, CommonUtil.EventLogEntryCount(), "confirm an error message was written to event log");

            string entry = CommonUtil.GetLastEventLogEntry();
            string expected = SR.DefaultLogDestinationMessage + Environment.NewLine + Environment.NewLine +
                SR.FileSinkMissingConfiguration;
            Assert.IsTrue(entry.IndexOf(expected) > -1, "confirm error message");

            expected = CommonUtil.FormattedMessage;
            Assert.IsTrue(entry.IndexOf(expected) > -1, "confirm message is inside");
        }

        [Test]
        [ExpectedException(typeof(IOException))]
        public void LogMessageToLockedFile()
        {
            CommonUtil.SendTestMessage(this.sink);

            // open the file and lock it
            FileStream stream = new FileStream(CommonUtil.FileName,
                                               FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            reader.ReadToEnd();

            try
            {
                // try to write to a locked log file
                CommonUtil.SendTestMessage(this.sink);
            }
            finally
            {
                stream.Close();
            }
        }

        [Test]
        public void LogMessageToSharedFile()
        {
            // write the first message to create the file
            CommonUtil.SendTestMessage(this.sink);

            // open the file for reading - no lock
            FileStream stream1 = new FileStream(CommonUtil.FileName,
                                                FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader reader1 = new StreamReader(stream1);
            reader1.ReadToEnd();

            // try to write to a locked log file
            CommonUtil.SendTestMessage(this.sink);

            // open the file for reading - no lock
            FileStream stream2 = new FileStream(CommonUtil.FileName,
                                                FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader reader2 = new StreamReader(stream2);
            string contents2 = reader2.ReadToEnd();

            stream1.Close();
            stream2.Close();

            Assert.AreEqual(0, CommonUtil.EventLogEntryCount(), "confirm no error messages were written to event log");

            int firstMessageIndex = contents2.IndexOf(CommonUtil.FormattedMessage);
            Assert.IsTrue(firstMessageIndex > -1, "confirm the first log entry was written");

            Assert.IsTrue(contents2.IndexOf(CommonUtil.FormattedMessage, firstMessageIndex + 1) > -1,
                          "make sure the second entry is there also");
        }

        [Test]
        public void LogMessageToFlatFileWithNoPath()
        {
            string fileName = "trace.log";
            FlatFileSinkData sinkParams = new FlatFileSinkData();
            sinkParams.FileName = fileName;
            sinkParams.Header = "";
            sinkParams.Footer = "";

            this.sink = new FlatFileSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            CommonUtil.SendTestMessage(this.sink);

            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            string contents = reader.ReadToEnd();
            stream.Close();

            Assert.AreEqual(0, CommonUtil.EventLogEntryCount(), "confirm no messages written to event log");
            Assert.IsTrue(contents.IndexOf(CommonUtil.FormattedMessage) > -1, "confirm message is part of contents");
        }

        [Test]
        public void LogMessageToFlatFileWithNullHeaderFooter()
        {
            string fileName = "trace.log";
            FlatFileSinkData sinkParams = new FlatFileSinkData();
            sinkParams.FileName = fileName;
            sinkParams.Header = null;
            sinkParams.Footer = null;

            this.sink = new FlatFileSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            CommonUtil.SendTestMessage(this.sink);

            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            string contents = reader.ReadToEnd();
            stream.Close();

            Assert.AreEqual(0, CommonUtil.EventLogEntryCount(), "confirm no messages written to event log");
            Assert.IsTrue(contents.IndexOf(CommonUtil.FormattedMessage) > -1, "confirm message is part of contents");
        }

        [Test]
        public void LogMessageToFlatFileWithNullFileName()
        {
            FlatFileSinkData sinkParams = new FlatFileSinkData();
            sinkParams.FileName = null;

            this.sink = new FlatFileSink();
            sink.Initialize(new TestLogSinkConfigurationView(sinkParams));

            CommonUtil.SendTestMessage(this.sink);

            Assert.AreEqual(1, CommonUtil.EventLogEntryCount(), "confirm warning written to event log");
            string entry = CommonUtil.GetLastEventLogEntry();
            string expected = SR.DefaultLogDestinationMessage + Environment.NewLine + Environment.NewLine +
                SR.FileSinkMissingConfiguration;
            Assert.IsTrue(entry.IndexOf(expected) > -1, "confirm error message");
        }
	}
}

#endif