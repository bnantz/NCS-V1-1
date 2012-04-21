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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>
    /// <devdoc>
    /// Instrumentation event called when the cache is flushed.
    /// </devdoc>
    public class CachingServiceCacheFlushedEvent : CachingServiceEvent
    {
        private static CachingServiceCacheFlushedEvent cachingEvent = new CachingServiceCacheFlushedEvent();

        private CachingServiceCacheFlushedEvent() : base()
        {
        }

        /// <summary/>
        /// <exclude/>
        public static void FireEvent()
        {
            cachingEvent.FireEventCore();
        }

        private void FireEventCore()
        {
            InstrumentedEvent.FireWmiEvent(this);
        }
    }
}