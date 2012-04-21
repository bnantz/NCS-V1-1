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
    public class InstanceDataCollectionFixture
    {
        private InstanceDataCollection instances;

        [SetUp]
        public void SetUp()
        {
            instances = new InstanceDataCollection();
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddInvalidTypeObject()
        {
            instances.Add("foo");
        }

        [Test]
        public void EnumeratorCurrentTest()
        {
            InstanceData instance = new InstanceData();
            instance.Name = "MyName";
            instance.DatabaseTypeName = "MyType";
            instances.Add(instance);
            InstanceData instance2 = new InstanceData();
            instance2.Name = "MyName2";
            instance2.DatabaseTypeName = "MyType2";
            instances.Add(instance2);
            int count = 0;
            foreach (InstanceData inst in instances)
            {
                Assert.IsNotNull(inst);
                count++;
                foreach (InstanceData inst2 in instances)
                {
                    Assert.IsNotNull(inst2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }

        [Test]
        public void AddRemoveTest()
        {
            InstanceData instance = new InstanceData();
            instance.Name = "MyName";
            instances.Add(instance);
            Assert.AreEqual(1, instances.Count);
            instances.Remove(instance.Name);
        }

        [Test]
        public void AddAsObjectTest()
        {
            InstanceData instance = new InstanceData();
            instance.Name = "MyName";
            instances.Add((Object)instance);
        }

        [Test]
        public void AddItemTest()
        {
            InstanceData instance = new InstanceData();
            instance.Name = "MyName";
            instances[instance.Name] = instance;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullObjectTest()
        {
            instances.Remove(null);
        }

        [Test]
        public void ItemTest()
        {
            InstanceData instance = new InstanceData();
            instance.Name = "MyName";
            instances.Add(instance);
            Assert.AreEqual(1, instances.Count);
        }

        [Test]
        public void ClearTest()
        {
            InstanceData instance = new InstanceData();
            instance.Name = "MyName";
            instances.Add(instance);
            Assert.AreEqual(1, instances.Count);
            instances.Clear();
            Assert.AreEqual(0, instances.Count);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddObjectTest()
        {
            instances.Add(new object());
        }

        [Test]
        public void EnumeratorTest()
        {
            InstanceData instance = new InstanceData();
            instance.Name = "MyName";
            instances.Add(instance);
            IEnumerator enumerator = instances.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Assert.IsNotNull(enumerator.Current);
            }
        }
    }
}

#endif