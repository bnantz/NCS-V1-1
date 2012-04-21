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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class ExpirationPollTimerFixture
    {
        private int counter;

        [SetUp]
        public void InitializeCount()
        {
            counter = 0;
        }

        private void CallbackMethod(object notUsed)
        {
            counter++;
        }

        [Test]
        public void WillCallBackAtSetInterval()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StartPolling(new TimerCallback(CallbackMethod), 100);
            Thread.Sleep(1100);
            timer.StopPolling();
            Assert.IsTrue((counter >= 9) && (counter <= 12));
        }

        [Test]
        public void CanStopPolling()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StartPolling(new TimerCallback(CallbackMethod), 100);
            Thread.Sleep(1100);
            timer.StopPolling();
            Thread.Sleep(250);
            Assert.IsTrue((counter >= 9) && (counter <= 12));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartingWithNullCallbackThrowsException()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StartPolling(null, 100);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void StartingWithZeroPollTimeThrowsException()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StartPolling(new TimerCallback(CallbackMethod), 0);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCallStopBeforeCallingStart()
        {
            ExpirationPollTimer timer = new ExpirationPollTimer();
            timer.StopPolling();
        }
    }
}

#endif