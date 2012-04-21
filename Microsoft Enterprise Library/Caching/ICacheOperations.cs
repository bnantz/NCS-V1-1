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
    /// <summary>
    /// Summary description for ICacheOperations.
    /// </summary>
    internal interface ICacheOperations
    {
        Hashtable GetCurrentCacheState();
        void RemoveItemFromCache(string keyToRemove, CacheItemRemovedReason removalReason);
    }
}