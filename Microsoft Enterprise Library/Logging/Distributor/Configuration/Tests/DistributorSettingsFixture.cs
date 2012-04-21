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
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration.Tests
{
    [TestFixture]
    public class DistributorSettingsFixture
    {
        private DistributorSettings distributorSettings;

        [TestFixtureSetUp]
        public void Init()
        {
            distributorSettings = DistributorSchemaBuilder.GetDistributorSettings();
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(distributorSettings);
        }

        [Test]
        public void Settings()
        {
            string msmqPath = @".\Private$\entlib";

            Assert.AreEqual("AppError", distributorSettings.DefaultCategory);
            Assert.AreEqual(1000, distributorSettings.DistributorService.QueueTimerInterval);
            Assert.AreEqual("EntLib Distributor", distributorSettings.DistributorService.ServiceName);
            Assert.IsTrue(distributorSettings.DistributorService.MsmqPath.IndexOf(msmqPath) > -1);
        }

        [Test]
        public void Sinks()
        {
            string sinkType = typeof(MockLogSink).FullName;
            Assert.AreEqual(2, distributorSettings.SinkDataCollection.Count);
            Assert.IsTrue(distributorSettings.SinkDataCollection["MockSink"].TypeName.IndexOf(sinkType) > -1);
        }

        [Test]
        public void Categories()
        {
            Assert.AreEqual(3, distributorSettings.CategoryDataCollection.Count);

            Assert.AreEqual(1, distributorSettings.CategoryDataCollection["MockCategoryOne"].DestinationDataCollection.Count);

            Assert.AreEqual(2, distributorSettings.CategoryDataCollection["MockCategoryMany"].DestinationDataCollection.Count);

            Assert.AreEqual(0, distributorSettings.CategoryDataCollection["NoDestinations"].DestinationDataCollection.Count);
        }
    }
}

#endif