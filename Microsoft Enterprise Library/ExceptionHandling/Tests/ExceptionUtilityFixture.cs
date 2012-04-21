//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

//---------------------------------------------------------------------
// <copyright file="ExceptionUtilityFixture.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Defines the ExceptionUtilityFixture class.
// </summary>
//---------------------------------------------------------------------

#if UNIT_TESTS
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestFixture]
    public class ExceptionUtilityFixture
    {
        private const string EventLogName = "Application";
        private const string EventLogSource = "EntLib Exception Handler";

        [Test]
        public void LogHandlingError()
        {
            Exception ex = new Exception("Unable to handle exception");
            ExceptionUtility.LogHandlingException("policy", null, null, ex);

            StringBuilder message = new StringBuilder();
            StringWriter writer = null;
            try
            {
                writer = new StringWriter(message);
                TextExceptionFormatter formatter = new TextExceptionFormatter(writer, ex);
                formatter.Format();
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            using (EventLog log = new EventLog(EventLogName))
            {
                EventLogEntry entry = log.Entries[log.Entries.Count - 1];

                Assert.AreEqual(EventLogEntryType.Error, entry.EntryType);
                Assert.AreEqual(EventLogSource, entry.Source);
            }
        }
    }
}

#endif