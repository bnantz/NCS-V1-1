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
    public class XmlIncludeTypeDataCollectionFixture
    {
        private XmlIncludeTypeDataCollection xmlIncludeTypes;

        [SetUp]
        public void SetUp()
        {
            xmlIncludeTypes = new XmlIncludeTypeDataCollection();
        }

        [Test]
        public void CollectionHoldsZeroElementsAtCreation()
        {
            XmlIncludeTypeDataCollection collection = new XmlIncludeTypeDataCollection();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void CanAddConfigSectionToCollection()
        {
            XmlIncludeTypeData xmlIncludeType = new XmlIncludeTypeData("Bill", typeof(Object).AssemblyQualifiedName);
            XmlIncludeTypeDataCollection collection = new XmlIncludeTypeDataCollection();
            collection.Add(xmlIncludeType);

            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void CanAddTwoConfigSectionsToCollection()
        {
            XmlIncludeTypeData sectionOne = new XmlIncludeTypeData("SectionOne", typeof(Object).AssemblyQualifiedName);
            XmlIncludeTypeData sectionTwo = new XmlIncludeTypeData("SectionTwo", typeof(Object).AssemblyQualifiedName);
            XmlIncludeTypeDataCollection collection = new XmlIncludeTypeDataCollection();
            collection.Add(sectionOne);
            collection.Add(sectionTwo);

            Assert.AreEqual(2, collection.Count);
        }

        [Test]
        public void CanGetFirstElementFromCollection()
        {
            XmlIncludeTypeData xmlIncludeType = new XmlIncludeTypeData("Bill", typeof(Object).AssemblyQualifiedName);
            XmlIncludeTypeDataCollection collection = new XmlIncludeTypeDataCollection();
            collection.Add(xmlIncludeType);

            IEnumerator enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual("Bill", ((XmlIncludeTypeData)enumerator.Current).Name);
        }

        [Test]
        public void IndexerTest()
        {
            XmlIncludeTypeData xmlIncludeType = new XmlIncludeTypeData("Bill", typeof(Object).AssemblyQualifiedName);
            XmlIncludeTypeDataCollection collection = new XmlIncludeTypeDataCollection();
            collection.Add(xmlIncludeType);

            XmlIncludeTypeData billSection = collection["Bill"];
            Assert.AreSame(xmlIncludeType, billSection);
        }

        [Test]
        public void AddRemoveTest()
        {
            XmlIncludeTypeData xmlIncludeType = new XmlIncludeTypeData("MyName", typeof(Object).AssemblyQualifiedName);
            xmlIncludeTypes.Add(xmlIncludeType);
            Assert.AreEqual(1, xmlIncludeTypes.Count);
            xmlIncludeTypes.Remove(xmlIncludeType.Name);
        }

        [Test]
        public void AddItemTest()
        {
            XmlIncludeTypeData xmlIncludeType = new XmlIncludeTypeData("MyName", typeof(Object).AssemblyQualifiedName);
            xmlIncludeTypes[xmlIncludeType.Name] = xmlIncludeType;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullObjectTest()
        {
            xmlIncludeTypes.Remove(null);
        }

        [Test]
        public void ItemTest()
        {
            XmlIncludeTypeData xmlIncludeType = new XmlIncludeTypeData("MyName", typeof(Object).AssemblyQualifiedName);
            xmlIncludeTypes.Add(xmlIncludeType);
            Assert.AreEqual(1, xmlIncludeTypes.Count);
        }

        [Test]
        public void EnumeratorTest()
        {
            XmlIncludeTypeData xmlIncludeType = new XmlIncludeTypeData("MyName", typeof(Object).AssemblyQualifiedName);
            xmlIncludeTypes.Add(xmlIncludeType);
            IEnumerator enumerator = xmlIncludeTypes.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Assert.IsNotNull(enumerator.Current);
            }
        }
    }
}

#endif