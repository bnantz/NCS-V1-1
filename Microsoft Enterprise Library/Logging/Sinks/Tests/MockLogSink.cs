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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    public class MockLogSink : LogSink
    {
        private static ArrayList entries = new ArrayList();
    	private static ILogFormatter lastSetFormatter;

    	public MockLogSink()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
        }

		public override ILogFormatter Formatter
		{
			set
			{
				lastSetFormatter = value;
				base.Formatter = value;
			}
		}

        protected override void SendMessageCore(LogEntry logEntry)
        {
            entries.Add(logEntry);
        }

        public static LogEntry GetEntry(int index)
        {
            return (LogEntry)entries[index];
        }

        public static LogEntry GetLastEntry()
        {
            if (entries.Count > 0)
            {
                return GetEntry(entries.Count - 1);
            }
            else
            {
                return null;
            }
        }

        public static int Count
        {
            get { return entries.Count; }
        }

        public static void Clear()
        {
            entries.Clear();
        }

    	public string FormatLastEntry()
    	{
    		return FormatEntry( GetLastEntry());
    	}

    	public static string FormatLogEntry(LogEntry entry)
    	{
    		MockLogSink sink = new MockLogSink();
			sink.Formatter = lastSetFormatter;
			return sink.FormatEntry(entry);
    	}

    }
}

#endif