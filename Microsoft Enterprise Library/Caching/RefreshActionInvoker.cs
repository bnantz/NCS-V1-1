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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <devdoc>
    /// Purpose of this class is to encapsulate the behavior of how ICacheItemRefreshActions
    /// are invoked in the background.
    /// </devdoc>
    internal class RefreshActionInvoker
    {
        /// <summary>
        /// Invokes the refresh action on a thread pool thread
        /// </summary>
        /// <param name="removedCacheItem">Cache item being removed. Must never be null.</param>
		/// <param name="removalReason">The reason the item was removed.</param>
        public static void InvokeRefreshAction(CacheItem removedCacheItem, CacheItemRemovedReason removalReason)
        {
            if (removedCacheItem.RefreshAction == null)
            {
                return;
            }

            try
            {
                RefreshActionData refreshActionData =
                    new RefreshActionData(removedCacheItem.RefreshAction, removedCacheItem.Key, removedCacheItem.Value, removalReason);
                refreshActionData.InvokeOnThreadPoolThread();
            }
            catch (Exception e)
            {
                CachingServiceInternalFailureEvent.Fire(SR.FailureToSpawnUserSpecifiedRefreshAction, e);
            }
            catch
            {            
                CachingServiceInternalFailureEvent.Fire(SR.FailureToSpawnUserSpecifiedRefreshAction, new Exception(SR.UnknownFailureReason));
            }
        }

        private class RefreshActionData
        {
            private ICacheItemRefreshAction refreshAction;
            private string keyToRefresh;
            private object removedData;
            private CacheItemRemovedReason removalReason;

            public RefreshActionData(ICacheItemRefreshAction refreshAction, string keyToRefresh, object removedData, CacheItemRemovedReason removalReason)
            {
                this.refreshAction = refreshAction;
                this.keyToRefresh = keyToRefresh;
                this.removalReason = removalReason;
                this.removedData = removedData;
            }

            public ICacheItemRefreshAction RefreshAction
            {
                get { return refreshAction; }
            }

            public string KeyToRefresh
            {
                get { return keyToRefresh; }
            }

            public CacheItemRemovedReason RemovalReason
            {
                get { return removalReason; }
            }

            public object RemovedData
            {
                get { return removedData; }
            }

            public void InvokeOnThreadPoolThread()
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolRefreshActionInvoker));
            }

            private void ThreadPoolRefreshActionInvoker(object notUsed)
            {
                try
                {
                    RefreshAction.Refresh(KeyToRefresh, RemovedData, RemovalReason);
                }
                catch (Exception e)
                {
                    CachingServiceInternalFailureEvent.Fire(SR.FailureWhileRunningUserSpecifiedRefreshAction, e);
                }
                catch
                {            
                    CachingServiceInternalFailureEvent.Fire(SR.FailureWhileRunningUserSpecifiedRefreshAction, new Exception(SR.UnknownFailureReason));
                }

            }
        }
    }
}
