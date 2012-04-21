//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if   UNIT_TESTS
using System;
using System.Collections;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
    [TestFixture]
    public class ConnectionStringDataCollectionFixture
    {
        private ConnectionStringDataCollection connectionStrings;

        [SetUp]
        public void SetUp()
        {
            connectionStrings = new ConnectionStringDataCollection();
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddInvalidTypeObject()
        {
            connectionStrings.Add("foo");
        }

        [Test]
        public void EnumeratorCurrentTest()
        {
            ConnectionStringData connectionString = new ConnectionStringData();
            connectionString.Name = "MyName";
            connectionStrings.Add(connectionString);
            ConnectionStringData connectionString2 = new ConnectionStringData();
            connectionString2.Name = "MyName2";
            connectionStrings.Add(connectionString2);
            int count = 0;
            foreach (ConnectionStringData cs in connectionStrings)
            {
                Assert.IsNotNull(cs);
                count++;
                foreach (ConnectionStringData cs2 in connectionStrings)
                {
                    Assert.IsNotNull(cs2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }

        [Test]
        public void AddRemoveTest()
        {
            ConnectionStringData connectionString = new ConnectionStringData();
            connectionString.Name = "MyName";
            connectionStrings.Add(connectionString);
            Assert.AreEqual(1, connectionStrings.Count);
            connectionStrings.Remove(connectionString.Name);
        }

        [Test]
        public void AddAsObjectTest()
        {
            ConnectionStringData connectionString = new ConnectionStringData();
            connectionString.Name = "MyName";
            connectionStrings.Add((Object)connectionString);
        }

        [Test]
        public void AddItemTest()
        {
            ConnectionStringData connectionString = new ConnectionStringData();
            connectionString.Name = "MyName";
            connectionStrings[connectionString.Name] = connectionString;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullObjectTest()
        {
            connectionStrings.Remove(null);
        }

        [Test]
        public void ItemTest()
        {
            ConnectionStringData connectionString = new ConnectionStringData();
            connectionString.Name = "MyName";
            connectionStrings.Add(connectionString);
            Assert.AreEqual(1, connectionStrings.Count);
        }

        [Test]
        public void ClearTest()
        {
            ConnectionStringData connectionString = new ConnectionStringData();
            connectionString.Name = "MyName";
            connectionStrings.Add(connectionString);
            Assert.AreEqual(1, connectionStrings.Count);
            connectionStrings.Clear();
            Assert.AreEqual(0, connectionStrings.Count);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddObjectTest()
        {
            connectionStrings.Add(new object());
        }

        [Test]
        public void EnumeratorTest()
        {
            ConnectionStringData connectionString = new ConnectionStringData();
            connectionString.Name = "MyName";
            connectionStrings.Add(connectionString);
            IEnumerator enumerator = connectionStrings.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Assert.IsNotNull(enumerator.Current);
            }
        }
    }
}

#endif