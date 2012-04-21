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

using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>
    /// <devdoc>
    /// Instrumentation event called when the cache is scavenged.
    /// </devdoc>
    [ComVisible(false)]
    public class CachingServiceCacheScavengedEvent : CachingServiceEvent
    {
        private static CachingServiceCacheScavengedEvent cachingEvent = new CachingServiceCacheScavengedEvent();
        private static object lockObj = new object();

        private long itemRemoved;
        private long sizeLimit;
        private int scavengingRange;

        private CachingServiceCacheScavengedEvent() : base()
        {
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Returns the number of items removed during this scavenging pass
        /// </devdoc>
        public long NumberOfItemRemoved
        {
            get { return itemRemoved; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Returns the maximum number of elements stored in the cache before scavenging occurs.
        /// </devdoc>
        public long ConfiguredSizeLimit
        {
            get { return sizeLimit; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Returns the number of items scavenged from the cache
        /// </devdoc>
        public int ScavengingRange
        {
            get { return scavengingRange; }
        }

        /// <summary/>
        /// <param name="itemRemoved"/>
        /// <param name="sizeLimit"/>
        /// <param name="scavengingRange"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires the event to instrumentation with the number of items removed, the size limit and scavenging range.
        /// </devdoc>
        public static void Fire(long itemRemoved, long sizeLimit, int scavengingRange)
        {
            cachingEvent.FireEvent(itemRemoved, sizeLimit, scavengingRange);
        }

        private void FireEvent(long itemRemoved, long sizeLimit, int scavengingRange)
        {
            lock (lockObj)
            {
                this.itemRemoved = itemRemoved;
                this.sizeLimit = sizeLimit;
                this.scavengingRange = scavengingRange;
                InstrumentedEvent.FireWmiEvent(this);
            }
        }
    }
}