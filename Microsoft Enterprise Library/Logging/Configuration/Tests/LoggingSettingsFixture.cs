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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestFixture]
    public class LoggingSettingsFixture
    {
        private LoggingSettings loggingSettings;

        [TestFixtureSetUp]
        public void Init()
        {
            loggingSettings = SchemaBuilder.GetLoggingSettings();
        }

        [Test]
        public void DeserializeTest()
        {
            Assert.IsNotNull(loggingSettings);
        }

        [Test]
        public void Settings()
        {
            string distStrategy = "EnterpriseLibrary.Logging.Distributor.InProcLogDistributionStrategy";

            Assert.IsTrue(loggingSettings.LoggingEnabled);
            Assert.IsTrue(loggingSettings.TracingEnabled);
            Assert.IsTrue(loggingSettings.DistributionStrategy.IndexOf(distStrategy) > -1);
            Assert.AreEqual(5, loggingSettings.MinimumPriority);
        }

        [Test]
        public void CategoryFilters()
        {
            Assert.AreEqual(3, loggingSettings.CategoryFilters.Count);
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, loggingSettings.CategoryFilterMode);

            int counter = 0;
            foreach (CategoryFilterData filter in loggingSettings.CategoryFilters)
            {
                if (counter == 0)
                {
                    Assert.AreEqual("CategoryOne", filter.Name);
                }
                else if (counter == 1)
                {
                    Assert.AreEqual("CategoryTwo", filter.Name);
                }
                else if (counter == 2)
                {
                    Assert.AreEqual("CategoryThree", filter.Name);
                }
                counter++;
            }
        }

        [Test]
        public void DistributionStrategiesFilters()
        {
            Assert.AreEqual(2, loggingSettings.DistributionStrategies.Count);

            int counter = 0;
            foreach (DistributionStrategyData filter in loggingSettings.DistributionStrategies)
            {
                if (counter == 0)
                {
                    Assert.AreEqual("InProc", filter.Name);
                }
                else if (counter == 1)
                {
                    Assert.AreEqual("Msmq", filter.Name);
                }
                counter++;
            }
        }
    }
}

#endif