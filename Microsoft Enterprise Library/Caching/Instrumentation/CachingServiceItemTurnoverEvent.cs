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
    /// Instrumentation event called when the items in cache change. 
    /// </devdoc>
    public class CachingServiceItemTurnoverEvent
    {
        private static CachingServiceItemTurnoverEvent cachingEvent =
            new CachingServiceItemTurnoverEvent(
                new string[]
                    {
                        SR.PerfCounterCacheTurnoverRate,
                        SR.PerfCounterTotalCacheEntries
                    }
                );

        private PerformanceCounterInstances turnOverInstances;
        private PerformanceCounterInstances totalEntriesInstances;

        private CachingServiceItemTurnoverEvent(string[] counterNames)
        {
            InstrumentedEvent internalEvent;

            internalEvent = new InstrumentedEvent(SR.CachingInstrumentationCounterCategory,
                                                  counterNames,
                                                  true);

            turnOverInstances = internalEvent.GetPerformanceCounterInstances(SR.PerfCounterCacheTurnoverRate);
            totalEntriesInstances = internalEvent.GetPerformanceCounterInstances(SR.PerfCounterTotalCacheEntries);
        }

        /// <summary/>
        /// <param name="addCount"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires event when an item is added to the cache with the number of item added.
        /// </devdoc>
        public static void FireAddItems(long addCount)
        {
            cachingEvent.FireEvent(addCount);
        }

        /// <summary/>
        /// <param name="removeCount"/>
        /// <exclude/>
        /// <devdoc>
        /// Fires event when an item is removed from the cache with the number of items removed.
        /// </devdoc>
        public static void FireRemoveItems(long removeCount)
        {
            cachingEvent.FireEvent(removeCount);
        }

        /// <summary/>
        /// <param name="totalItems"/>
        /// <exclude/>
        /// <devdoc>
        /// Sets the total number of items in the cache
        /// </devdoc>
        public static void SetItemsTotal(long totalItems)
        {
            cachingEvent.SetTotal(totalItems);
        }

        private void FireEvent(long count)
        {
            turnOverInstances.IncrementBy(count);
        }

        private void SetTotal(long totalItems)
        {
            totalEntriesInstances.RawValue(totalItems);
        }
    }
}