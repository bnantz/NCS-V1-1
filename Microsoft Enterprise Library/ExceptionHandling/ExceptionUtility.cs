//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Provides common functions for Exception Handling. This class cannot be inherited from.
    /// </summary>
    public sealed class ExceptionUtility
    {
        private const string EventLogSource = "EntLib Exception Handler";
        private const string HandlingInstanceToken = "{handlingInstanceID}";

        private ExceptionUtility()
        {
        }

        /// <summary>
        /// Formats a message by replacing the token "{handlingInstanceID}" with the handlingInstanceID.
        /// </summary>
        /// <param name="message">The original message.</param>
        /// <param name="handlingInstanceId">The handlingInststanceID passed into the handler.</param>
        /// <returns>The formatted message.</returns>
        public static string FormatExceptionMessage(string message, Guid handlingInstanceId)
        {
            return message.Replace(HandlingInstanceToken, handlingInstanceId.ToString());
        }

        /// <devDoc>
        /// Logs a handling exception to the eventLog
        /// </devDoc>
        internal static void LogHandlingException(string policyName, Exception offendingException, Exception chainException, Exception originalException)
        {
            StringBuilder message = new StringBuilder();
            StringWriter writer = null;
            try
            {
                writer = new StringWriter(message, CultureInfo.CurrentCulture);

                if (policyName.Length > 0)
                {
                    writer.WriteLine(SR.PolicyName(policyName));
                }

                FormatHandlingException(writer, SR.OffendingException, offendingException);
                FormatHandlingException(writer, SR.OriginalException, originalException);
                FormatHandlingException(writer, SR.ChainException, chainException);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            EventLog.WriteEntry(EventLogSource, message.ToString(), EventLogEntryType.Error);
        }

        private static void FormatHandlingException(StringWriter writer, string header, Exception ex)
        {
            if (ex != null)
            {
                writer.WriteLine();
                writer.WriteLine(header);
                writer.Write(writer.NewLine);
                TextExceptionFormatter formatter = new TextExceptionFormatter(writer, ex);
                formatter.Format();
            }
        }
    }
}