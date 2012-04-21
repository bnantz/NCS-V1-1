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

#if  UNIT_TESTS
using System;
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    [TestFixture]
    public class AbsoluteTimeFixture
    {
        [Test]
        public void WillExpireAfterOneSecondFromNow()
        {
            AbsoluteTime expiration = new AbsoluteTime(TimeSpan.FromSeconds(0.2));
            Assert.IsFalse(expiration.HasExpired(), "Should not be expired immediately after creation");
            Thread.Sleep(400);
            Assert.IsTrue(expiration.HasExpired(), "Should have expired by now");
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowsExceptionIfTimeEqualToNow()
        {
            AbsoluteTime expirationRightNow = new AbsoluteTime(TimeSpan.FromSeconds(0.0));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowsExceptionIfTimeInPast()
        {
            AbsoluteTime expirationInThePast = new AbsoluteTime(TimeSpan.FromSeconds(-1.0));
        }
    }
}

#endif