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

using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    public class CustomLogEntrySink : LogSink
    {
        // fields are static to support unit tests
        public static string Field1;
        public static string Field2;
        public static string Field3;
        public static string fullMessage;
        public static string Body;
        public static int EventID;
        public static short CategoryID;
        public static string Category;

        public CustomLogEntrySink()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
        }

        protected override void SendMessageCore(LogEntry logEntry)
        {
            if (logEntry is CustomLogEntry)
            {
                //handle a CustomMessageInfo object with extra metadata
                CustomLogEntry customLog = (CustomLogEntry)logEntry;

                CustomLogEntrySink.Field1 = customLog.AcmeCoField1;
                CustomLogEntrySink.Field2 = customLog.AcmeCoField2;
                CustomLogEntrySink.Field3 = customLog.AcmeCoField3;
            }

            CustomLogEntrySink.fullMessage = FormatEntry(logEntry);
            CustomLogEntrySink.Body = logEntry.Message;
            CustomLogEntrySink.EventID = logEntry.EventId;
            CustomLogEntrySink.Category = logEntry.Category;
        }

        public string[] RequiredParameters
        {
            get { return null; }
        }
    }
}

#endif