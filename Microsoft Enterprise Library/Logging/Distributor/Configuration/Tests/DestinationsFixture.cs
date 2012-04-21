//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration.Tests
{
    [TestFixture]
    public class DestinationsFixture
    {
        private DestinationDataCollection dests;

        [SetUp]
        public void Setup()
        {
            dests = new DestinationDataCollection();
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddInvalidTypeObject()
        {
            dests.Add("foo");
        }

        [Test]
        public void EnumeratorCurrentTest()
        {
            DestinationData destA = new DestinationData();
            destA.Name = "sink type 1";
            dests.Add(destA);

            DestinationData destB = new DestinationData();
            destB.Name = "sink type 2";
            dests.Add(destB);
            int count = 0;

            foreach (DestinationData dest1 in dests)
            {
                Assert.IsNotNull(dest1);
                count++;
                foreach (DestinationData dest2 in dests)
                {
                    Assert.IsNotNull(dest2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }
    }
}

#endif