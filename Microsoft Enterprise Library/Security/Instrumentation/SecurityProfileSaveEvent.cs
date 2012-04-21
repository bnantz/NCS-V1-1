//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Instrumentation infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>
    public class SecurityProfileSaveEvent : SecurityServiceEvent
    {
        private static SecurityProfileSaveEvent securityEvent;
        private static ReaderWriterLock writerLock;
        private const int LockTimeout = -1;

        static SecurityProfileSaveEvent()
        {
            string[] counterNames = new string[]
                {
                    SecurityServiceEvent.Counters[(int)CounterID.ProfileSave].CounterName
                };
            securityEvent = new SecurityProfileSaveEvent(counterNames);
            writerLock = new ReaderWriterLock();
        }

        private SecurityProfileSaveEvent(string[] counterNames) : base(counterNames, null)
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
            writerLock.AcquireWriterLock(LockTimeout);
            try
            {
                eventMessage = message;
                InstrumentedEvent.FireWmiEvent(this);
            }
            finally
            {
                writerLock.ReleaseWriterLock();
            }

            FireAuxEvent(string.Empty);
        }
    }
}