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
    public class ConfigurationSectionDataCollectionFixture
    {
        private ConfigurationSectionDataCollection configurationSections;

        [SetUp]
        public void SetUp()
        {
            configurationSections = new ConfigurationSectionDataCollection();
        }

        [Test]
        public void CollectionHoldsZeroElementsAtCreation()
        {
            ConfigurationSectionDataCollection collection = new ConfigurationSectionDataCollection();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void CanAddConfigSectionToCollection()
        {
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("Bill");
            ConfigurationSectionDataCollection collection = new ConfigurationSectionDataCollection();
            collection.Add(configurationSection);
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void CanAddTwoConfigSectionsToCollection()
        {
            ConfigurationSectionData sectionOne = new ConfigurationSectionData("SectionOne");
            ConfigurationSectionData sectionTwo = new ConfigurationSectionData("SectionTwo");
            ConfigurationSectionDataCollection collection = new ConfigurationSectionDataCollection();
            collection.Add(sectionOne);
            collection.Add(sectionTwo);

            Assert.AreEqual(2, collection.Count);
        }

        [Test]
        public void CanGetFirstElementFromCollection()
        {
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("Bill");
            ConfigurationSectionDataCollection collection = new ConfigurationSectionDataCollection();
            collection.Add(configurationSection);
            IEnumerator enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual("Bill", ((ConfigurationSectionData)enumerator.Current).Name);
        }

        [Test]
        public void IndexerTest()
        {
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("Bill");
            ConfigurationSectionDataCollection collection = new ConfigurationSectionDataCollection();
            collection.Add(configurationSection);

            ConfigurationSectionData billSection = collection["Bill"];
            Assert.AreSame(configurationSection, billSection);
        }

        [Test]
        public void AddRemoveTest()
        {
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("MyName");
            configurationSections.Add(configurationSection);
            Assert.AreEqual(1, configurationSections.Count);
            configurationSections.Remove(configurationSection.Name);
        }

        [Test]
        public void AddItemTest()
        {
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("MyName");
            configurationSections[configurationSection.Name] = configurationSection;
        }

        [Test]
        public void ClearTest()
        {
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("MyName");
            configurationSections[configurationSection.Name] = configurationSection;
            configurationSections.Clear();
            Assert.AreEqual(0, configurationSections.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullObjectTest()
        {
            configurationSections.Remove(null);
        }

        [Test]
        public void ItemTest()
        {
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("MyName");
            configurationSections.Add(configurationSection);
            Assert.AreEqual(1, configurationSections.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullObjectTest()
        {
            configurationSections.Add(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddNullNameTest()
        {
            ConfigurationSectionData data = new ConfigurationSectionData(null);
            configurationSections.Add(data);
        }

        [Test]
        public void EnumeratorTest()
        {
            ConfigurationSectionData configurationSection = new ConfigurationSectionData("MyName");
            configurationSections.Add(configurationSection);
            IEnumerator enumerator = configurationSections.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Assert.IsNotNull(enumerator.Current);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullNameGetTest()
        {
            object foo = configurationSections[null];
            Assert.IsNull(foo);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullNameSetTest()
        {
            configurationSections[null] = new ConfigurationSectionData("foo");
        }
    }
}

#endif