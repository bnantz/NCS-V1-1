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
    internal class ScavengerTask
    {
        private CacheCapacityScavengingPolicy scavengingPolicy;
        private readonly CachingConfigurationView currentConfigurationData;
        private ICacheOperations cacheOperations;
        private string cacheManagerName;

        private int NumberOfItemsToBeScavenged
        {
            get { return currentConfigurationData.GetCacheManagerSettings().CacheManagers[cacheManagerName].NumberToRemoveWhenScavenging; }
        }

        internal ScavengerTask(string cacheManagerName, 
                               CachingConfigurationView currentConfigurationData, 
                               CacheCapacityScavengingPolicy scavengingPolicy,
                               ICacheOperations cacheOperations)
        {
            this.scavengingPolicy = scavengingPolicy;
            this.cacheManagerName = cacheManagerName;
            this.currentConfigurationData = currentConfigurationData;
            this.cacheOperations = cacheOperations;
        }

        public void DoScavenging()
        {
            Hashtable liveCacheRepresentation = cacheOperations.GetCurrentCacheState();

            int currentNumberItemsInCache = liveCacheRepresentation.Count;

            if (scavengingPolicy.IsScavengingNeeded(currentNumberItemsInCache))
            {
                ResetScavengingFlagInCacheItems(liveCacheRepresentation);
                SortedList scavengableItems = SortItemsForScavenging(liveCacheRepresentation);
                RemoveScavengableItems(scavengableItems);
            }
        }

        private void ResetScavengingFlagInCacheItems(Hashtable liveCacheRepresentation)
        {
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                lock (cacheItem)
                {
                    cacheItem.MakeEligibleForScavenging();
                }
            }
        }

        private SortedList SortItemsForScavenging(Hashtable unsortedItemsInCache)
        {
            return new SortedList(unsortedItemsInCache, new PriorityDateComparer(unsortedItemsInCache));
        }

        private void RemoveScavengableItems(SortedList scavengableItems)
        {
            int scavengedItemCount = 0;
            foreach (CacheItem scavengableItem in scavengableItems.Values)
            {
                bool wasRemoved = RemoveItemFromCache(scavengableItem);
                if (wasRemoved)
                {
                    scavengedItemCount++;
                    if (scavengedItemCount == NumberOfItemsToBeScavenged)
                    {
                        break;
                    }
                }
            }

            CachingServiceCacheScavengedEvent.Fire(scavengedItemCount, scavengingPolicy.MaximumItemsAllowedBeforeScavenging, NumberOfItemsToBeScavenged);
        }

        private bool RemoveItemFromCache(CacheItem itemToRemove)
        {
            lock (itemToRemove)
            {
                if (itemToRemove.EligibleForScavenging)
                {
                    try
                    {
                        cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Scavenged);
                        return true;
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

            return false;
        }
    }
}