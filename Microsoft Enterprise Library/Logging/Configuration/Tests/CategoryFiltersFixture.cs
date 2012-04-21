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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestFixture]
    public class CategoryFiltersFixture
    {
        private CategoryFilterDataCollection filters;

        [SetUp]
        public void Setup()
        {
            filters = new CategoryFilterDataCollection();
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddInvalidTypeObject()
        {
            filters.Add("foo");
        }

        [Test]
        public void EnumeratorCurrentTest()
        {
            CategoryFilterData filterA = new CategoryFilterData();
            filterA.Name = "MyName";
            filters.Add(filterA);

            CategoryFilterData filterB = new CategoryFilterData();
            filterB.Name = "MyName2";
            filters.Add(filterB);
            int count = 0;

            foreach (CategoryFilterData filter1 in filters)
            {
                Assert.IsNotNull(filter1);
                count++;
                foreach (CategoryFilterData filter2 in filters)
                {
                    Assert.IsNotNull(filter2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }
    }
}

#endif