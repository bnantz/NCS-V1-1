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

using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    internal class PriorityDateComparer : IComparer
    {
        private Hashtable unsortedItems;

        internal PriorityDateComparer(Hashtable unsortedItems)
        {
            this.unsortedItems = unsortedItems;
        }

        public int Compare(object left, object right)
        {
            CacheItem leftCacheItem = (CacheItem)unsortedItems[(string)left];
            CacheItem rightCacheItem = (CacheItem)unsortedItems[(string)right];

            lock (rightCacheItem)
            {
                lock (leftCacheItem)
                {
                    if (rightCacheItem == null && leftCacheItem == null)
                    {
                        return 0;
                    }
                    if (leftCacheItem == null)
                    {
                        return -1;
                    }
                    if (rightCacheItem == null)
                    {
                        return 1;
                    }

                    return leftCacheItem.ScavengingPriority == rightCacheItem.ScavengingPriority
                        ? leftCacheItem.LastAccessedTime.CompareTo(rightCacheItem.LastAccessedTime)
                        : leftCacheItem.ScavengingPriority - rightCacheItem.ScavengingPriority;
                }
            }
        }
    }
}