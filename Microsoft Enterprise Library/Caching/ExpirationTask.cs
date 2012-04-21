//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Summary description for ExpirationTask.
    /// </summary>
    internal class ExpirationTask
    {
        private ICacheOperations cacheOperations;

        internal ExpirationTask(ICacheOperations cacheOperations)
        {
            this.cacheOperations = cacheOperations;
        }

        public void DoExpirations()
        {
            Hashtable liveCacheRepresentation = cacheOperations.GetCurrentCacheState();
            MarkAsExpired(liveCacheRepresentation);
            PrepareForSweep();
            SweepExpiredItemsFromCache(liveCacheRepresentation);
        }

        public virtual int MarkAsExpired(Hashtable liveCacheRepresentation)
        {
            int markedCount = 0;
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                lock (cacheItem)
                {
                    if (cacheItem.HasExpired())
                    {
                        markedCount++;
                        cacheItem.WillBeExpired = true;
                    }
                }
            }

            return markedCount;
        }

        public virtual void SweepExpiredItemsFromCache(Hashtable liveCacheRepresentation)
        {
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                RemoveItemFromCache(cacheItem);
            }
        }

        public virtual void PrepareForSweep()
        {
        }

        private void RemoveItemFromCache(CacheItem itemToRemove)
        {
            lock (itemToRemove)
            {
                if (itemToRemove.WillBeExpired)
                {
                    try
                    {
                        cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Expired);
                    }
                    catch (Exception e)
                    {
                        CachingServiceInternalFailureEvent.Fire(SR.FailureToRemoveCacheItemInBackground, e);
                    }
                    catch
                    {
                        CachingServiceInternalFailureEvent.Fire(SR.FailureToRemoveCacheItemInBackground, new Exception(SR.UnknownFailureReason));
                    }
                }
            }
        }
    }
}