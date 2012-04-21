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
    public class DistributionStrategyDataCollectionFixture
    {
        private DistributionStrategyDataCollection strategies;

        [SetUp]
        public void Setup()
        {
            strategies = new DistributionStrategyDataCollection();
        }

        //		[Test]
        //		[ExpectedException(typeof(InvalidCastException))]
        //		public void AddInvalidTypeObject() 
        //		{
        //			strategies.Add("foo");
        //		}

        [Test]
        public void EnumeratorCurrentTest()
        {
            DistributionStrategyData strategyA = new CustomDistributionStrategyData();
            strategyA.Name = "MyName";
            strategyA.TypeName = "MyType";
            strategies.Add(strategyA);

            DistributionStrategyData strategyB = new CustomDistributionStrategyData();
            strategyB.Name = "MyName2";
            strategyB.TypeName = "MyType2";
            strategies.Add(strategyB);
            int count = 0;

            foreach (DistributionStrategyData strategy1 in strategies)
            {
                Assert.IsNotNull(strategy1);
                count++;
                foreach (DistributionStrategyData strategy2 in strategies)
                {
                    Assert.IsNotNull(strategy2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }
    }
}

#endif