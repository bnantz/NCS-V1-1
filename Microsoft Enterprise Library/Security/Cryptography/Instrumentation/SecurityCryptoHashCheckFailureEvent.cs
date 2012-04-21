//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>    
    public class SecurityCryptoHashCheckFailureEvent : SecurityCryptoServiceEvent
    {
        private static SecurityCryptoHashCheckFailureEvent securityEvent = new SecurityCryptoHashCheckFailureEvent(new string[]
            {
                SecurityCryptoServiceEvent.Counters[(int)CounterId.HashCheckFailure].CounterName
            });

        private static object lockObj = new object();

        private SecurityCryptoHashCheckFailureEvent(string[] counterNames) : base(counterNames, null)
        {
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        public static void Fire(string message)
        {
            securityEvent.FireEvent(message);
        }

        /// <summary/>
        /// <param name="message"/>
        /// <exclude/>
        protected void FireEvent(string message)
        {
            lock (lockObj)
            {
                eventMessage = message;
                InstrumentedEvent.FireWmiEvent(this);
            }

            FireAuxEvent(string.Empty);
        }
    }
}