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
    public class DatabaseTypeDataCollectionFixture
    {
        private DatabaseTypeDataCollection databaseTypes;

        [SetUp]
        public void SetUp()
        {
            databaseTypes = new DatabaseTypeDataCollection();
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddInvalidTypeObject()
        {
            databaseTypes.Add("foo");
        }

        [Test]
        public void EnumeratorCurrentTest()
        {
            DatabaseTypeData databaseType = new DatabaseTypeData();
            databaseType.Name = "MyName";
            databaseType.TypeName = "MyType";
            databaseTypes.Add(databaseType);
            DatabaseTypeData databaseType2 = new DatabaseTypeData();
            databaseType2.Name = "MyName2";
            databaseType2.TypeName = "MyType2";
            databaseTypes.Add(databaseType2);
            int count = 0;
            foreach (DatabaseTypeData dbType in databaseTypes)
            {
                Assert.IsNotNull(dbType);
                count++;
                foreach (DatabaseTypeData dbType2 in databaseTypes)
                {
                    Assert.IsNotNull(dbType2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }

        [Test]
        public void AddRemoveTest()
        {
            DatabaseTypeData databaseType = new DatabaseTypeData();
            databaseType.Name = "MyName";
            databaseTypes.Add(databaseType);
            Assert.AreEqual(1, databaseTypes.Count);
            databaseTypes.Remove(databaseType.Name);
        }

        [Test]
        public void AddAsObjectTest()
        {
            DatabaseTypeData databaseType = new DatabaseTypeData();
            databaseType.Name = "MyName";
            databaseTypes.Add((Object)databaseType);
        }

        [Test]
        public void AddItemTest()
        {
            DatabaseTypeData databaseType = new DatabaseTypeData();
            databaseType.Name = "MyName";
            databaseTypes[databaseType.Name] = databaseType;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullObjectTest()
        {
            databaseTypes.Remove(null);
        }

        [Test]
        public void ItemTest()
        {
            DatabaseTypeData databaseType = new DatabaseTypeData();
            databaseType.Name = "MyName";
            databaseTypes.Add(databaseType);
            Assert.AreEqual(1, databaseTypes.Count);
        }

        [Test]
        public void ClearTest()
        {
            DatabaseTypeData databaseType = new DatabaseTypeData();
            databaseType.Name = "MyName";
            databaseTypes.Add(databaseType);
            Assert.AreEqual(1, databaseTypes.Count);
            databaseTypes.Clear();
            Assert.AreEqual(0, databaseTypes.Count);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddObjectTest()
        {
            databaseTypes.Add(new object());
        }

        [Test]
        public void EnumeratorTest()
        {
            DatabaseTypeData databaseType = new DatabaseTypeData();
            databaseType.Name = "MyName";
            databaseTypes.Add(databaseType);
            IEnumerator enumerator = databaseTypes.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Assert.IsNotNull(enumerator.Current);
            }
        }
    }
}

#endif