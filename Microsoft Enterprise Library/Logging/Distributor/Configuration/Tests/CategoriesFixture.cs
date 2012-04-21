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
    public class CategoriesFixture
    {
        private CategoryDataCollection cats;

        [SetUp]
        public void Setup()
        {
            cats = new CategoryDataCollection();
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddInvalidTypeObject()
        {
            cats.Add("foo");
        }

        [Test]
        public void EnumeratorCurrentTest()
        {
            CategoryData catA = new CategoryData();
            catA.Name = "MyName";
            cats.Add(catA);

            CategoryData catB = new CategoryData();
            catB.Name = "MyName2";
            cats.Add(catB);
            int count = 0;

            foreach (CategoryData cat1 in cats)
            {
                Assert.IsNotNull(cat1);
                count++;
                foreach (CategoryData cat2 in cats)
                {
                    Assert.IsNotNull(cat2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }
    }
}

#endif