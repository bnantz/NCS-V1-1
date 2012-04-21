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
    public class DestinationFixture
    {
        [Test]
        public void Properties()
        {
            string name = "dest1";
            string sink = "sink1";
            string format = "format1";

            DestinationData dest = new DestinationData();
            dest.Name = name;
            dest.Sink = sink;
            dest.Format = format;

            Assert.AreEqual(name, dest.Name);
            Assert.AreEqual(sink, dest.Sink);
            Assert.AreEqual(format, dest.Format);
        }
    }
}

#endif