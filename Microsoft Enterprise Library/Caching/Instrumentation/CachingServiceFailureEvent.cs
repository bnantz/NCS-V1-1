//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>
    public class CachingServiceFailureEvent : CachingServiceEvent
    {
        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Exception reported from the failure
        /// </devdoc>
        [CLSCompliant(false)] protected Exception reportedException;

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Message for the reported exception
        /// </devdoc>
        [CLSCompliant(false)] protected string reportedExceptionMessage;

        /// <summary/>
        /// <param name="eventLogIdentifiers"/>
        /// <exclude/>
        protected CachingServiceFailureEvent(EventLogIdentifier[] eventLogIdentifiers) : base(null, eventLogIdentifiers)
        {
            reportedException = null;
            reportedExceptionMessage = string.Empty;
            eventMessage = string.Empty;
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Returns stack trace of reported exception
        /// </devdoc>
        public string ExceptionStackTrace
        {
            get { return (reportedException == null) ? string.Empty : reportedException.StackTrace; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Returns message associated with reported exception
        /// </devdoc>
        public string ExceptionMessage
        {
            get
            {
                if (reportedExceptionMessage.Length == 0)
                {
                    reportedExceptionMessage = InstrumentedEvent.GetErrorMessage(reportedException);
                }
                return reportedExceptionMessage;
            }
        }
    }
}