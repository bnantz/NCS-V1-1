//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    /// <summary>
    /// Summary description for MsmqLogDistributorTransactionalBehaviorFixture.
    /// </summary>
    public class MsmqLogDistributorTransactionalBehaviorFixture : MsmqLogDistributorFixture
    {
        protected override void CreateQueueForTesting()
        {
            CommonUtil.CreateTransactionalPrivateTestQ();
        }
    }
}

#endif
