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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    /// <devdoc>
    /// The event that get's fired if the handling of an Exception fails.
    /// </devdoc>
    public class ExceptionHandlingFailureEvent : ExceptionHandlingEvent
    {
        private string message;
        private Exception exception;
        private string exceptionMessage;

        /// <summary/>
        /// <param name="eventLogIDs"/>
        /// <exclude/>
        protected ExceptionHandlingFailureEvent(EventLogIdentifier[] eventLogIDs) : base(null, eventLogIDs)
        {
            this.exception = null;
            this.exceptionMessage = string.Empty;
            this.message = string.Empty;
        }

        /// <summary/>
        /// <exclude/>
        public string Message
        {
            get { return this.message; }
        }

        /// <devdoc>
        /// The stack trace at the time of the failure.
        /// </devdoc>
        /// <summary/>
        /// <exclude/>
        public string ExceptionStackTrace
        {
            get { return this.exception == null ? string.Empty : this.exception.StackTrace; }
        }

        /// <devdoc>
        /// The message of the exception that caused the failure.
        /// </devdoc>
        /// <summary/>
        /// <exclude/>
        public string ExceptionMessage
        {
            get
            {
                if (this.exceptionMessage.Length == 0)
                {
                    this.exceptionMessage = InstrumentedEvent.GetErrorMessage(this.exception);
                }
                return this.exceptionMessage;
            }
        }

        /*
		 * The following are "setter" methods to provide "set access" for
		 * certain properties for superclasses. 
		 * We do not use properties since we need a public property for
		 * getting the value.  Whidbey will support different access 
		 * specifiers so that this scenario can be support properly with
		 * properties.
		 */

        /// <devdoc>
        /// Sets the failure message.
        /// </devdoc>
        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        protected void SetMessage(string message)
        {
            this.message = message;
        }

        /// <summary/>
        /// <param name="exception"/>
        /// <exclude/>
        protected void SetException(Exception exception)
        {
            this.exception = exception;
        }

        /// <summary/>
        /// <param name="exceptionMessage"/>
        /// <exclude/>
        protected void SetExceptionMessage(string exceptionMessage)
        {
            this.exceptionMessage = exceptionMessage;
        }
    }
}