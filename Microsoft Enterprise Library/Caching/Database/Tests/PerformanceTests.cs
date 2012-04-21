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

#if LONG_RUNNING_TESTS
using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
    [TestFixture]
    //  [Ignore("Fixing race condition in caching before running these tests again")]
        public class PerformanceTests
    {
        private static readonly int MAX_THREADS = 50;
        private ArrayList threads;
        private CacheManager currentCacheManager;

        [SetUp]
        public void CreateThreads()
        {
            threads = new ArrayList();

            currentCacheManager = null;
        }

        [TearDown]
        public void EmptyCache()
        {
            if (currentCacheManager != null)
            {
                currentCacheManager.Flush();
            }
        }

        private void ReadRandomWorkerMethod()
        {
            int[] keys = GenerateKeyList(700, 1000);
            for (int i = 0; i < 70; i++)
            {
                try
                {
                    string randomNumber = keys[i].ToString();
                    string value = (string)currentCacheManager.GetData(randomNumber);
                    Assert.AreEqual(value, randomNumber);
                }
                catch (SqlException e)
                {
                    Console.Write(e);
                }
            }
        }

        private void WorkerMethod()
        {
            int[] keys = GenerateKeyList(700, 1000);
            for (int i = 0; i < 70; i++)
            {
                try
                {
                    string randomNumber = keys[i].ToString();
                    currentCacheManager.Add(randomNumber, randomNumber);
                }
                catch (SqlException e)
                {
                    Console.Write(e);
                }
            }
        }

        private void RandomWorkerMethod()
        {
            int[] keys = GenerateKeyList(700, 1000);
            for (int i = 0; i < 70; i++)
            {
                try
                {
                    string randomNumber = keys[i].ToString();
                    currentCacheManager.Add(randomNumber, randomNumber);
                }
                catch (SqlException e)
                {
                    Console.Write(e);
                }
            }
        }

        [Test]
        public void DoReadsWorkCorrectly()
        {
//            Console.WriteLine("\n\n\n********");
//            Console.WriteLine("*** DoReadsWorkCorrectly");    
//            Console.WriteLine("********\n\n\n");

            currentCacheManager = CacheFactory.GetCacheManager("InDatabasePerformanceTest");

            for (int i = -100; i < 100; i++)
            {
                currentCacheManager.Add(i.ToString(), i.ToString());
            }

            RunTest(new ThreadStart(ReadRandomWorkerMethod));
        }

        [Test]
        public void RunThreadsInMemory()
        {
//            Console.WriteLine("\n\n\n********");
//            Console.WriteLine("*** RunThreadsInMemory");    
//            Console.WriteLine("********\n\n\n");
            currentCacheManager = CacheFactory.GetCacheManager("InMemoryPerformanceTest");

            RunTest(new ThreadStart(WorkerMethod));
        }

        [Test]
        public void RunRandomThreadsInMemory()
        {
//            Console.WriteLine("\n\n\n********");
//            Console.WriteLine("*** RunRandomThreadsInMemory");    
//            Console.WriteLine("********\n\n\n");
            currentCacheManager = CacheFactory.GetCacheManager("InMemoryPerformanceTest");

            RunTest(new ThreadStart(RandomWorkerMethod));
        }

        [Test]
        public void RunThreadsInDatabase()
        {
//            Console.WriteLine("\n\n\n********");
//            Console.WriteLine("*** RunThreadsInDatabase");    
//            Console.WriteLine("********\n\n\n");
            currentCacheManager = CacheFactory.GetCacheManager("InDatabasePerformanceTest");
            RunTest(new ThreadStart(WorkerMethod));
        }

        [Test]
        public void RunRandomThreadsInDatabase()
        {
//            Console.WriteLine("\n\n\n********");
//            Console.WriteLine("*** RunRandomThreadsInDatabase");    
//            Console.WriteLine("********\n\n\n");
            currentCacheManager = CacheFactory.GetCacheManager("InDatabasePerformanceTest");
            RunTest(new ThreadStart(RandomWorkerMethod));
        }

        private void RunTest(ThreadStart workerMethod)
        {
            for (int i = 0; i < MAX_THREADS; i++)
            {
                Thread t = new Thread(workerMethod);
                t.Name = i.ToString();
                threads.Add(t);
            }

            //DateTime startTime = DateTime.Now;
            for (int i = 0; i < MAX_THREADS; i++)
            {
                ((Thread)threads[i]).Start();
            }

            for (int i = 0; i < MAX_THREADS; i++)
            {
                ((Thread)threads[i]).Join();
            }
            //DateTime stopTime = DateTime.Now;

            //TimeSpan diff = stopTime - startTime;
            //Console.Out.WriteLine("Time difference is " + diff.ToString());
        }

        private int[] GenerateKeyList(int count, int keySpan)
        {
            byte[] byteArray = new byte[count * 4];
            MemoryStream stream = new MemoryStream(byteArray);
            BinaryReader reader = new BinaryReader(stream);

            lock (this)
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetBytes(byteArray);
            }

            int[] keyList = new int[count];
            for (int i = 0; i < count; i++)
            {
                keyList[i] = reader.ReadInt32() % keySpan;
            }

            return keyList;
        }
    }
}

#endif