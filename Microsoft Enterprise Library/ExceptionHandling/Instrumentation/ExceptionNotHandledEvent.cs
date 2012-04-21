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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    /// <devdoc>
    /// For performance reason, this class does not inherit from ApplicationEvent because
    /// the event does not need to fire a WMI event.
    /// </devdoc>
    public sealed class ExceptionNotHandledEvent
    {
        private static InstrumentedEvent internalEvent = new InstrumentedEvent(ExceptionHandledEvent.InstrumentationCounterCategory,
                                                                               new string[]
                                                                                   {
                                                                                       ExceptionHandlingEvent.counters[(int)ExceptionHandlingEvent.CounterID.UnhandledPerSec].CounterName
                                                                                   },
                                                                               true);

        private ExceptionNotHandledEvent()
        {
        }

        /// <devdoc>
        /// Increment the # of TLEs Handled/Sec performance counter.
        /// </devdoc>
        /// <summary/>
        /// <exclude/>
        public static void Fire()
        {
            internalEvent.FireEvent(string.Empty);
        }
    }
}