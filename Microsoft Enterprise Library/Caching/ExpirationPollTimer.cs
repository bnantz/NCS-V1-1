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

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Summary description for ExpirationPollTimer.
    /// </summary>
    internal class ExpirationPollTimer
    {
        private Timer pollTimer = null;

        public void StartPolling(TimerCallback callbackMethod, int pollCycleInMilliseconds)
        {
            if (callbackMethod == null)
            {
                throw new ArgumentNullException("callbackMethod");
            }
            if (pollCycleInMilliseconds <= 0)
            {
                throw new ArgumentException(SR.InvalidExpirationPollCycleTime, "pollCycleInMilliseconds");
            }

            pollTimer = new Timer(callbackMethod, null, pollCycleInMilliseconds, pollCycleInMilliseconds);
        }

        public void StopPolling()
        {
            if (pollTimer == null)
            {
                throw new InvalidOperationException(SR.InvalidPollingStopOperation);
            }

            pollTimer.Dispose();
            pollTimer = null;
        }
    }
}