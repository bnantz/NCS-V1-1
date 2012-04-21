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
using System.Collections;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Summary description for ProducerConsumerQueue.
    /// </summary>
    internal class ProducerConsumerQueue
    {
        private object lockObject = new Object();
        private Queue queue = new Queue();

        public int Count
        {
            get { return queue.Count; }
        }

        public object Dequeue()
        {
            lock (lockObject)
            {
                while (queue.Count == 0)
                {
                    if (WaitUntilInterrupted())
                    {
                        return null;
                    }
                }

                return queue.Dequeue();
            }
        }

        public void Enqueue(object o)
        {
            lock (lockObject)
            {
                queue.Enqueue(o);
                Monitor.Pulse(lockObject);
            }
        }

        private bool WaitUntilInterrupted()
        {
            try
            {
                Monitor.Wait(lockObject);
            }
            catch (ThreadInterruptedException)
            {
                return true;
            }

            return false;
        }
    }
}