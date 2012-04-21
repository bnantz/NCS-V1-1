//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if    UNIT_TESTS
using System;
using System.Collections;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class NameValueItemCollectionFixture
    {
        private NameValueItemCollection items;

        [SetUp]
        public void SetUp()
        {
            items = new NameValueItemCollection();
        }

        [Test]
        public void CollectionHoldsZeroElementsAtCreation()
        {
            NameValueItemCollection collection = new NameValueItemCollection();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void CanAddConfigSectionToCollection()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "Bill";
            NameValueItemCollection collection = new NameValueItemCollection();
            collection.Add(nameValueItem);

            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void CanAddTwoConfigSectionsToCollection()
        {
            NameValueItem sectionOne = new NameValueItem();
            sectionOne.Name = "SectionOne";

            NameValueItem sectionTwo = new NameValueItem();
            sectionTwo.Name = "SectionTwo";

            NameValueItemCollection collection = new NameValueItemCollection();
            collection.Add(sectionOne);
            collection.Add(sectionTwo);

            Assert.AreEqual(2, collection.Count);
        }

        [Test]
        public void CanGetFirstElementFromCollection()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "Bill";
            NameValueItemCollection collection = new NameValueItemCollection();
            collection.Add(nameValueItem);

            IEnumerator enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual("Bill", ((NameValueItem)enumerator.Current).Name);
        }

        [Test]
        public void IndexerTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "Bill";
            NameValueItemCollection collection = new NameValueItemCollection();
            collection.Add(nameValueItem);
            NameValueItem billSection = collection.GetNameValueItem("Bill");
            Assert.AreSame(nameValueItem, billSection);
        }

        [Test]
        public void IndexerIntTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "Bill";
            NameValueItemCollection collection = new NameValueItemCollection();
            collection.Add(nameValueItem);
            NameValueItem billSection = collection[0];
            Assert.AreSame(nameValueItem, billSection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerIntOutOfRangeTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "Bill";
            NameValueItemCollection collection = new NameValueItemCollection();
            collection.Add(nameValueItem);
            NameValueItem billSection = collection[1];
            Assert.AreSame(nameValueItem, billSection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerIntNullTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "Bill";
            NameValueItemCollection collection = new NameValueItemCollection();
            collection.Add(nameValueItem);
            NameValueItem billSection = collection[-1];
            Assert.AreSame(nameValueItem, billSection);
        }

        [Test]
        public void AddRemoveTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "MyName";
            items.Add(nameValueItem);
            Assert.AreEqual(1, items.Count);
            items.Remove(nameValueItem.Name);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTwiceTest()
        {
            NameValueItem item = new NameValueItem();
            item.Name = "MyName";
            items.Add(item);
            items.Add(item);
        }

        [Test]
        public void RenameTest()
        {
            NameValueItem item = new NameValueItem();
            item.Name = "MyName";
            item.Value = "Value1";
            items.Add(item);
            item.Name = "hello";
            Assert.AreSame(item, items.GetNameValueItem("hello"));
        }

        [Test]
        public void AddAsObjectTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "MyName";
            items.Add((Object)nameValueItem);
        }

        [Test]
        public void AddItemTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "MyName";
            items.Add(nameValueItem);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullObjectTest()
        {
            items.Remove(null);
        }

        [Test]
        public void ItemTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "MyName";
            items.Add(nameValueItem);
            Assert.AreEqual(1, items.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddObjectTest()
        {
            items.Add(new object());
        }

        [Test]
        public void EnumeratorTest()
        {
            NameValueItem nameValueItem = new NameValueItem();
            nameValueItem.Name = "MyName";
            items.Add(nameValueItem);
            IEnumerator enumerator = items.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Assert.IsNotNull(enumerator.Current);
            }
        }

        [Test]
        public void ClearTest()
        {
            NameValueItemCollection collection = new NameValueItemCollection();
            collection.Add("key1", "value1");
            collection.Add("key2", "value2");
            Assert.AreEqual(2, collection.Count);
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void CopyToTest()
        {
            items.Add(new NameValueItem("myName", "myValue"));
            NameValueItem[] itemArray = new NameValueItem[items.Count];
            items.CopyTo(itemArray, 0);
            Assert.AreEqual(items.Count, itemArray.Length);
            Assert.AreSame(items["myName"], itemArray[0].Value);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToNullTest()
        {
            items.Add(new NameValueItem("myName", "myValue"));
            items.CopyTo(null, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyToNegativeIndexTest()
        {
            items.Add(new NameValueItem("myName", "myValue"));
            NameValueItem[] itemArray = new NameValueItem[items.Count];
            items.CopyTo(itemArray, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToNonExistenceIndexTest()
        {
            items.Add(new NameValueItem("myName", "myValue"));
            NameValueItem[] itemArray = new NameValueItem[items.Count];
            items.CopyTo(itemArray, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToSourceGreatherThanDestinationTest()
        {
            items.Add(new NameValueItem("myName", "myValue"));
            NameValueItem[] itemArray = new NameValueItem[0];
            items.CopyTo(itemArray, 0);
        }
    }
}

#endif