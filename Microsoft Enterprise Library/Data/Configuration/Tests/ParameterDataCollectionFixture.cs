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
    public class ParameterDataCollectionFixture
    {
        private ParameterDataCollection parameters;

        [SetUp]
        public void SetUp()
        {
            parameters = new ParameterDataCollection();
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddInvalidTypeObject()
        {
            parameters.Add("foo");
        }

        [Test]
        public void EnumeratorCurrentTest()
        {
            ParameterData parameter = new ParameterData();
            parameter.Name = "MyName";
            parameter.Value = "MyValue";
            parameters.Add(parameter);
            ParameterData parameter2 = new ParameterData();
            parameter2.Name = "MyName2";
            parameter2.Value = "MyValue2";
            parameters.Add(parameter2);
            int count = 0;
            foreach (ParameterData param1 in parameters)
            {
                Assert.IsNotNull(param1);
                count++;
                foreach (ParameterData param2 in parameters)
                {
                    Assert.IsNotNull(param2);
                    count++;
                }
            }
            Assert.AreEqual(6, count);
        }

        [Test]
        public void AddRemoveTest()
        {
            ParameterData parameter = new ParameterData();
            parameter.Name = "MyName";
            parameters.Add(parameter);
            Assert.AreEqual(1, parameters.Count);
            parameters.Remove(parameter.Name);
        }

        [Test]
        public void AddAsObjectTest()
        {
            ParameterData parameter = new ParameterData();
            parameter.Name = "MyName";
            parameters.Add((Object)parameter);
        }

        [Test]
        public void AddItemTest()
        {
            ParameterData parameter = new ParameterData();
            parameter.Name = "MyName";
            parameters[parameter.Name] = parameter;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveNullObjectTest()
        {
            parameters.Remove(null);
        }

        [Test]
        public void ItemTest()
        {
            ParameterData parameter = new ParameterData();
            parameter.Name = "MyName";
            parameters.Add(parameter);
            Assert.AreEqual(1, parameters.Count);
        }

        [Test]
        public void ClearTest()
        {
            ParameterData parameter = new ParameterData();
            parameter.Name = "MyName";
            parameters.Add(parameter);
            Assert.AreEqual(1, parameters.Count);
            parameters.Clear();
            Assert.AreEqual(0, parameters.Count);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void AddObjectTest()
        {
            parameters.Add(new object());
        }

        [Test]
        public void EnumeratorTest()
        {
            ParameterData parameter = new ParameterData();
            parameter.Name = "MyName";
            parameters.Add(parameter);
            IEnumerator enumerator = parameters.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Assert.IsNotNull(enumerator.Current);
            }
        }
    }
}

#endif