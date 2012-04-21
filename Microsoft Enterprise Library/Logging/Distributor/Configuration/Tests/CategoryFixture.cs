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
    public class CategoryFixture
    {
        [Test]
        public void Properties()
        {
            string name = "category1";
            CategoryData c = new CategoryData();
            c.Name = name;
            Assert.AreEqual(name, c.Name);

            DestinationData dest1 = new DestinationData();
            dest1.Name = "dest1";
            dest1.Sink = "sink1";

            DestinationData dest2 = new DestinationData();
            dest2.Name = "dest2";
            dest2.Sink = "sink2";

//            DestinationDataCollection destinations = new DestinationDataCollection();
//            destinations.Add(dest1);
//            destinations.Add(dest2);

            c.DestinationDataCollection.Add(dest1);
            c.DestinationDataCollection.Add(dest2);

            Assert.AreEqual(2, c.DestinationDataCollection.Count);
            Assert.AreEqual("dest1", c.DestinationDataCollection["dest1"].Name);
            Assert.AreEqual("dest2", c.DestinationDataCollection["dest2"].Name);
        }
    }
}

#endif