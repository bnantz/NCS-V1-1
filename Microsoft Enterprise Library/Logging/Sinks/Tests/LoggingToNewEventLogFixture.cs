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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
#if NEVER_DEFINED
    [TestFixture]
    public class LoggingToNewEventLogFixture
    {
        [Test]
        public void EventLoggedToNewLogAndSourceGoToRightLog()
        {
            EventLogSink sink = new EventLogSink(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            sink.SendMessage(new LogEntry("foo", "", 1, 2, Severity.Error, "FooTitle", null));
        }
    }
#endif
}

#endif