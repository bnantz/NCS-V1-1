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

#if       UNIT_TESTS
using System;
using System.Reflection;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ReferencePathAttributeFixture
    {
        private PathTest test;

        [TestFixtureSetUp]
        public void Init()
        {
            test = new PathTest();
        }

        [Test]
        public void ReflectionTest()
        {
            Type t = test.GetType();
            PropertyInfo property = t.GetProperty("Path");
            ReferenceTypeAttribute[] attributes = (ReferenceTypeAttribute[])property.GetCustomAttributes(typeof(ReferenceTypeAttribute), true);
            Assert.AreEqual(1, attributes.Length);
            Assert.AreEqual(typeof(ApplicationConfigurationNode), attributes[0].ReferenceType);
        }

        private class PathTest
        {
            private string path = "My path";

            [ReferenceType(typeof(ApplicationConfigurationNode))]
            public string Path
            {
                get { return path; }
            }
        }
    }
}

#endif