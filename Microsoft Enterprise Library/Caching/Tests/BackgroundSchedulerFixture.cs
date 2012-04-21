//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © 2004 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class BackgroundSchedulerFixture
    {
        [Test]
        public void SchedulerCanBeStoppedWhenRequested()
        {
            BackgroundScheduler scheduler = new BackgroundScheduler(null, null);
            scheduler.Start();
            Thread.Sleep(2500);

            Assert.IsTrue(scheduler.IsActive);

            scheduler.Stop();
            Thread.Sleep(10000);

            Assert.IsFalse(scheduler.IsActive);
        }
    }
}

#endif
