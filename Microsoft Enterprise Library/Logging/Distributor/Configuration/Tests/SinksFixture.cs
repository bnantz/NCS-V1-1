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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration.Tests
{
    [TestFixture]
    public class SinksFixture
    {
        private SinkDataCollection sinkDataCollection;

        [SetUp]
        public void Setup()
        {
            sinkDataCollection = new SinkDataCollection();
        }

        //		[Test]
        //		[ExpectedException(typeof(InvalidCastException))]
        //		public void AddInvalidTypeObject() 
        //		{
        //			sinkDataCollection.Add("foo");
        //		}

        [Test]
        public void EnumeratorCurrentTest()
        {
            EmailSinkData sinkA = new EmailSinkData();
            sinkA.Name = "MyName";
            sinkA.TypeName = "MyType";
            sinkDataCollection.Add(sinkA);

            EmailSinkData sinkB = new EmailSinkData();
            sinkB.Name = "MyName2";
            sinkB.TypeName = "MyType2";
            sinkDataCollection.Add(sinkB);
            int count = 0;

            foreach (SinkData sink1 in sinkDataCollection)
            {
                Assert.IsNotNull(sink1);
                count++;
                foreach (SinkData sink2 in sinkDataCollection)
                {
                    Assert.IsNotNull(sink2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }
    }
}

#endif