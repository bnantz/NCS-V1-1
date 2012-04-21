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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>
    public class CachingServiceInternalFailureEvent : CachingServiceFailureEvent
    {
        private static CachingServiceInternalFailureEvent cachingEvent =
            new CachingServiceInternalFailureEvent(
                new EventLogIdentifier[]
                    {
                        new EventLogIdentifier(EventLogEntryType.Error, (int)Log.InternalFailureOccurred)
                    });

        private static object lockObj = new object();

        private CachingServiceInternalFailureEvent(EventLogIdentifier[] eventLogIDs)
            : base(eventLogIDs)
        {
            reportedException = null;
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The path of the pricipal configuration file.
        /// </devdoc>
        public string ConfigurationFilePath
        {
            get
            {
                string fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                if (fileName == null)
                {
                    return string.Empty;
                }
                return fileName;
            }
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires the event to instrumentation with a message
        /// </devdoc>
        public static void Fire(string message)
        {
            cachingEvent.FireEvent(message);
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="ex"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires to event to instrumentation with a message and reported exception
        /// </devdoc>
        public static void Fire(string message, Exception ex)
        {
            cachingEvent.FireEvent(message, ex);
        }

        private void FireEvent(string message)
        {
            lock (lockObj)
            {
                base.eventMessage = message;
                reportedException = null;
                reportedExceptionMessage = string.Empty;

                InstrumentedEvent.FireWmiEvent(this);
            }
            FireAuxEvent(message);
        }

        private void FireEvent(string message, Exception ex)
        {
            lock (lockObj)
            {
                base.eventMessage = message;
                reportedException = ex;
                reportedExceptionMessage = string.Empty;
                InstrumentedEvent.FireWmiEvent(this);
            }
            FireAuxEvent(message + "  Exception: " + ExceptionMessage);
        }
    }
}