//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>    
    public class SecurityCryptoServiceEvent : BaseEvent
    {
        internal static CounterCreationData[] Counters = new CounterCreationData[]
            {
                new CounterCreationData(
                    "# of Hash Creates/Sec",
                    SR.PerfCounterNumCreateHashSecMsg,
                    PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(
                    "# of Hash Checks/Sec",
                    SR.PerfCounterNumCheckHashSecMsg,
                    PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(
                    "# of Hash Check Failures/Sec",
                    SR.PerfCounterNumCheckHashFailureSecMsg,
                    PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(
                    "# of Symmetric Encryptions/Sec",
                    SR.PerfCounterNumSymmEncryptionSecMsg,
                    PerformanceCounterType.RateOfCountsPerSecond32),
                new CounterCreationData(
                    "# of Symmetric Decryptions/Sec",
                    SR.PerfCounterNumSymmDecryptionSecMsg,
                    PerformanceCounterType.RateOfCountsPerSecond32)
            };

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The internal event object responsible for the Performance Counters and
        /// the EventLog.  And, it constructed with a BaseEvent object, it also
        /// ties the WMI event when fired.
        /// </devdoc>
        [CLSCompliant(false)] protected InstrumentedEvent internalEvent;

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The message for the event.
        /// </devdoc>
        [CLSCompliant(false)] protected string eventMessage;

        /// <summary/>
        /// <exclude/>
        protected SecurityCryptoServiceEvent()
        {
            this.eventMessage = string.Empty;
            this.internalEvent = null;
        }

        /// <summary/>
        /// <param name="counterNames"/>
        /// <param name="eventLogIDs"/>
        /// <exclude/>
        protected SecurityCryptoServiceEvent(string[] counterNames, EventLogIdentifier[] eventLogIDs)
        {
            this.eventMessage = string.Empty;
            this.internalEvent = new InstrumentedEvent(
                SR.InstrumentationCounterCategory,
                counterNames,
                true,
                SR.InstrumentationEventSource,
                eventLogIDs);
        }

        /// <summary/>
        /// <param name="counterNames"/>
        /// <exclude/>
        protected SecurityCryptoServiceEvent(string[] counterNames)
        {
            this.eventMessage = string.Empty;
            this.internalEvent = new InstrumentedEvent(
                SR.InstrumentationCounterCategory,
                counterNames,
                true);
        }

        /// <summary/>
        /// <exclude/>
        public string Message
        {
            get { return this.eventMessage; }
            set { this.eventMessage = value; }
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        protected void FireAuxEvent(string message)
        {
            if (this.internalEvent == null)
            {
                return;
            }

            this.internalEvent.FireEvent(message);
        }

        /// <devdoc>
        /// Index to the Counters array.
        /// </devdoc>
        internal enum CounterId : int
        {
            HashCreate = 0,
            HashCheck = 1,
            HashCheckFailure = 2,
            SymmetricEncrypt = 3,
            SymmetricDecrypt = 4,
        }

        /// <summary>
        /// Event Log Event IDs
        /// </summary>
        internal enum Log : int
        {
            // including generic failures and configuration failures
            InternalFailureOccurred = 1,
        }

    }
}