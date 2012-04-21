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
    internal class BackgroundScheduler : ICacheScavenger
    {
        private ProducerConsumerQueue inputQueue = new ProducerConsumerQueue();
        private Thread inputQueueThread;
        private ExpirationTask expirer;
        private ScavengerTask scavenger;
        private bool isActive = false;
        private bool running = false;

        public BackgroundScheduler(ExpirationTask expirer, ScavengerTask scavenger)
        {
            this.expirer = expirer;
            this.scavenger = scavenger;

            ThreadStart queueReader = new ThreadStart(QueueReader);
            inputQueueThread = new Thread(queueReader);
            inputQueueThread.IsBackground = true;
        }

        public void Start()
        {
            running = true;
            inputQueueThread.Start();
        }

        public void Stop()
        {
            running = false;
            inputQueueThread.Interrupt();
        }

        internal bool IsActive
        {
            get { return isActive; }
        }

        public void ExpirationTimeoutExpired(object notUsed)
        {
            inputQueue.Enqueue(new ExpirationTimeoutExpiredMsg(this));
        }

        public void StartScavenging()
        {
            inputQueue.Enqueue(new StartScavengingMsg(this));
        }

        internal void DoStartScavenging()
        {
            scavenger.DoScavenging();
        }

        internal void DoExpirationTimeoutExpired()
        {
            expirer.DoExpirations();
        }

        private void QueueReader()
        {
            isActive = true;
            while (running)
            {
                IQueueMessage msg = inputQueue.Dequeue() as IQueueMessage;
                try
                {
                    if (msg == null)
                    {
                        continue;
                    }

                    msg.Run();
                }
                catch (ThreadInterruptedException)
                {
                }
                catch (Exception e)
                {
                    CachingServiceInternalFailureEvent.Fire(SR.BackgroundSchedulerProducerConsumerQueueFailure, e);
                }
                catch
                {
                    CachingServiceInternalFailureEvent.Fire(SR.BackgroundSchedulerProducerConsumerQueueFailure, new Exception(SR.UnknownFailureReason));
                }
            }
            isActive = false;
        }
    }
}
