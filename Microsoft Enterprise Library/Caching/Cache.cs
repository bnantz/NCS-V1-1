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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// The real worker of the block. The Cache class is the traffic cop that prevents 
    /// resource contention among the different threads in the system. It also will act
    /// as the remoting gateway when that feature is added to the cache.
    /// </summary>
    internal class Cache : ICacheOperations, IDisposable
    {
        private Hashtable inMemoryCache;
        private ICacheScavenger cacheScavenger;
        private IBackingStore backingStore;
        private CacheCapacityScavengingPolicy scavengingPolicy;

        private const string addInProgressFlag = "Dummy variable used to flag temp cache item added during Add";

        public Cache(IBackingStore backingStore, CacheCapacityScavengingPolicy scavengingPolicy)
        {
            this.backingStore = backingStore;
            this.scavengingPolicy = scavengingPolicy;

            Hashtable initialItems = backingStore.Load();
            inMemoryCache = Hashtable.Synchronized(initialItems);

            CachingServiceItemTurnoverEvent.SetItemsTotal(Count);
        }

        public int Count
        {
            get { return inMemoryCache.Count; }
        }

        public bool Contains(string key)
        {
            ValidateKey(key);

            return inMemoryCache.Contains(key);
        }

        public void Initialize(ICacheScavenger cacheScavenger)
        {
            this.cacheScavenger = cacheScavenger;
        }

        public void Add(string key, object value)
        {
            Add(key, value, CacheItemPriority.Normal, null);
        }

        public void Add(string key, object value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
        {
            ValidateKey(key);
            EnsureCacheInitialized();

            CacheItem cacheItemBeforeLock = null;
            bool lockWasSuccessful = false;

            do
            {
                lock (inMemoryCache.SyncRoot)
                {
                    if (inMemoryCache.Contains(key) == false)
                    {
                        cacheItemBeforeLock = new CacheItem(key, addInProgressFlag, CacheItemPriority.NotRemovable, null);
                        inMemoryCache[key] = cacheItemBeforeLock;
                    }
                    else
                    {
                        cacheItemBeforeLock = (CacheItem)inMemoryCache[key];
                    }

                    lockWasSuccessful = Monitor.TryEnter(cacheItemBeforeLock);
                }

                if (lockWasSuccessful == false)
                {
                    Thread.Sleep(0);
                }
            } while (lockWasSuccessful == false);

            try
            {
                cacheItemBeforeLock.TouchedByUserAction(true);

                CacheItem newCacheItem = new CacheItem(key, value, scavengingPriority, refreshAction, expirations);
                try
                {
                    backingStore.Add(newCacheItem);
                    cacheItemBeforeLock.Replace(value, refreshAction, scavengingPriority, expirations);
                    inMemoryCache[key] = cacheItemBeforeLock;
                }
                catch
                {
                    backingStore.Remove(key);
                    inMemoryCache.Remove(key);
                    throw;
                }

                if (scavengingPolicy.IsScavengingNeeded(inMemoryCache.Count))
                {
                    cacheScavenger.StartScavenging();
                }

                CachingServiceItemTurnoverEvent.FireAddItems(1);
                CachingServiceItemTurnoverEvent.SetItemsTotal(inMemoryCache.Count);
            }
            finally
            {
                Monitor.Exit(cacheItemBeforeLock);
            }
        }

        public void Remove(string key)
        {
            Remove(key, CacheItemRemovedReason.Removed);
        }

        public void Remove(string key, CacheItemRemovedReason removalReason)
        {
            ValidateKey(key);

            CacheItem cacheItemBeforeLock = null;
            bool lockWasSuccessful;
            do
            {
                lock (inMemoryCache.SyncRoot)
                {
                    cacheItemBeforeLock = (CacheItem)inMemoryCache[key];

                    if (IsObjectInCache(cacheItemBeforeLock))
                    {
                        return;
                    }

                    lockWasSuccessful = Monitor.TryEnter(cacheItemBeforeLock);
                }

                if (lockWasSuccessful == false)
                {
                    Thread.Sleep(0);
                }
            } while (lockWasSuccessful == false);

            try
            {
                cacheItemBeforeLock.TouchedByUserAction(true);

                backingStore.Remove(key); // Does exception safety matter here? We're removing it due to expiration or scavenging...
                inMemoryCache.Remove(key);

                RefreshActionInvoker.InvokeRefreshAction(cacheItemBeforeLock, removalReason);

                CachingServiceItemTurnoverEvent.FireRemoveItems(1);
                CachingServiceItemTurnoverEvent.SetItemsTotal(inMemoryCache.Count);
            }
            finally
            {
                Monitor.Exit(cacheItemBeforeLock);
            }

        }

        /// <devdoc>
        /// This seemingly redundant method is here to be called through the ICacheOperations 
        /// interface. I put this in place to break any dependency from any other class onto 
        /// the Cache class
        /// </devdoc>
        public void RemoveItemFromCache(string key, CacheItemRemovedReason removalReason)
        {
            Remove(key, removalReason);
        }

        public object GetData(string key)
        {
            ValidateKey(key);
            CacheItem cacheItemBeforeLock = null;
            bool lockWasSuccessful = false;

            do
            {
                lock (inMemoryCache.SyncRoot)
                {
                    cacheItemBeforeLock = (CacheItem)inMemoryCache[key];
                    if (IsObjectInCache(cacheItemBeforeLock))
                    {
                        CachingServiceItemReadEvent.Fire(false);
                        return null;
                    }

                    lockWasSuccessful = Monitor.TryEnter(cacheItemBeforeLock);
                }

                if (lockWasSuccessful == false)
                {
                    Thread.Sleep(0);
                }
            } while (lockWasSuccessful == false);

            try
            {
                if (cacheItemBeforeLock.HasExpired())
                {
                    cacheItemBeforeLock.TouchedByUserAction(true);

                    backingStore.Remove(key); // Does exception safety matter here? We're removing it due to expiration or scavenging...
                    inMemoryCache.Remove(key);

                    RefreshActionInvoker.InvokeRefreshAction(cacheItemBeforeLock, CacheItemRemovedReason.Expired);
                    CachingServiceItemReadEvent.Fire(false);
                    CachingServiceItemTurnoverEvent.FireRemoveItems(1);
                    CachingServiceItemTurnoverEvent.SetItemsTotal(inMemoryCache.Count);
                    return null;
                }

                backingStore.UpdateLastAccessedTime(cacheItemBeforeLock.Key, DateTime.Now); // Does exception safety matter here?
                cacheItemBeforeLock.TouchedByUserAction(false);

                CachingServiceItemReadEvent.Fire(true);
                return cacheItemBeforeLock.Value;
            }
            finally
            {
                Monitor.Exit(cacheItemBeforeLock);
            }
        }

        /// <devdoc>
        /// There may still be thread safety issues in this class with respect to expirations
        /// and scavenging, but I really doubt that either of those will be happening while
        /// a Flush is in progress. It seems that the most likely scenario for a flush
        /// to be called is at the very start of a program, or when absolutely nothing else
        /// is going on. Calling flush in the middle of an application would seem to be
        /// an "interesting" thing to do in normal circumstances.
        /// </devdoc>
        public void Flush()
        {
            RestartFlushAlgorithm:
            lock (inMemoryCache.SyncRoot)
            {
                foreach (string key in inMemoryCache.Keys)
                {
                    bool lockWasSuccessful = false;
                    CacheItem itemToRemove = (CacheItem)inMemoryCache[key];
                    try
                    {
                        if(lockWasSuccessful = Monitor.TryEnter(itemToRemove))
                        {
                            itemToRemove.TouchedByUserAction(true);
                        }
                        else
                        {
                           goto RestartFlushAlgorithm;
                        }
                    }
                    finally
                    {
                        if(lockWasSuccessful) Monitor.Exit(itemToRemove);
                    }
                }

                int countBeforeFlushing = inMemoryCache.Count;

                backingStore.Flush();
                inMemoryCache.Clear();

                CachingServiceItemTurnoverEvent.FireRemoveItems(countBeforeFlushing);
                CachingServiceItemTurnoverEvent.SetItemsTotal(0);
                CachingServiceCacheFlushedEvent.FireEvent();
            }
        }

        public Hashtable GetCurrentCacheState()
        {
            return (Hashtable)inMemoryCache.Clone();
        }

        private void ValidateKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (key.Length == 0)
            {
                throw new ArgumentException(SR.EmptyParameterName, "key");
            }
        }

        private void EnsureCacheInitialized()
        {
            if (cacheScavenger == null)
            {
                throw new InvalidOperationException(SR.CacheNotInitializedException);
            }
        }

        private bool IsObjectInCache(CacheItem cacheItemBeforeLock)
        {
            return cacheItemBeforeLock == null || Object.ReferenceEquals(cacheItemBeforeLock.Value, addInProgressFlag);
        }

        ~Cache()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                backingStore.Dispose();
                backingStore = null;
            }
        }
    }
}