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
using System.Collections;
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestFixture]
    public class ProducerConsumerQueueFixture
    {
        private ProducerConsumerQueue queue;
        private string dequeuedMsg = null;
        private ArrayList accumulatedResults;

        private static object staticLock = new object();
        private static int staticCounter = 0;

        [SetUp]
        public void SetUp()
        {
            dequeuedMsg = null;
            accumulatedResults = new ArrayList();
            staticCounter = 0;
        }

        [Test]
        public void CanStopConsumerSide()
        {
            queue = new ProducerConsumerQueue();
            dequeuedMsg = "This will be null if we break out of dequeue and return a null msg";
            ThreadStart consumerMethod = new ThreadStart(runner);
            Thread consumerThread = new Thread(consumerMethod);
            consumerThread.Start();
            Thread.Sleep(500);
            consumerThread.Interrupt();
            consumerThread.Join();
            Thread.Sleep(500);
            Assert.IsNull(dequeuedMsg);
        }

        [Test]
        public void CanAddElementAsProducerAndRemoveAsConsumer()
        {
            queue = new ProducerConsumerQueue();
            ThreadStart consumerMethod = new ThreadStart(runner);
            Thread consumerThread = new Thread(consumerMethod);
            consumerThread.Start();
            Thread.Sleep(500);

            queue.Enqueue("this is a string");

            Thread.Sleep(500);
            consumerThread.Join();

            Assert.AreEqual("this is a string", dequeuedMsg);
        }

        [Test]
        public void StressTest()
        {
            queue = new ProducerConsumerQueue();

            ThreadStart consumerMethod = new ThreadStart(accumulatingRunner);
            Thread consumerThread = new Thread(consumerMethod);
            consumerThread.Start();
            Thread.Sleep(500);

            ArrayList threads = new ArrayList();
            for (int i = 0; i < 100; i++)
            {
                ThreadStart producerMethod = new ThreadStart(producingThread);
                threads.Add(new Thread(producerMethod));
            }

            for (int i = 0; i < 100; i++)
            {
                ((Thread)threads[i]).Start();
            }

            for (int i = 0; i < 100; i++)
            {
                ((Thread)threads[i]).Join();
            }

            consumerThread.Join();
            Assert.AreEqual(0, queue.Count);
            Assert.AreEqual(100000, staticCounter);

            for (int i = 0; i < 100000; i++)
            {
                Assert.AreEqual(i, (int)accumulatedResults[i], "Failed at index " + i);
            }
        }

        private void producingThread()
        {
            Random generator = new Random(12345);
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(generator.Next(5));
                int valueToInsert = -1;
                lock (staticLock)
                {
                    staticCounter++;
                    valueToInsert = staticCounter;
                }
                queue.Enqueue(valueToInsert);
            }
        }

        private void accumulatingRunner()
        {
            for (int i = 0; i < 100000; i++)
            {
                object dequeuedObject = queue.Dequeue();
                accumulatedResults.Add(i);
            }
        }

        private void runner()
        {
            object dequeuedObject = queue.Dequeue();
            dequeuedMsg = dequeuedObject as string;
        }
    }
}

#endif