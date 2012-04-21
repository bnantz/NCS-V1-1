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
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class TypeSelectorFixture
    {
        private TreeView treeView;
        private TypeSelector selector;

        [TestFixtureSetUp]
        public void Initialize()
        {
            treeView = new TreeView();
            treeView.ImageList = new TypeSelectorUI().TypeImageList;
            selector = new TypeSelector(typeof(TestNode), typeof(BaseTestNode), treeView);
        }

        [SetUp]
        public void SetUp()
        {
            treeView.Nodes.Clear();
        }

        [Test]
        public void IsAssiableFromTest()
        {
            Assert.IsTrue(typeof(BaseTestNode).IsAssignableFrom(typeof(TestNode)));
        }

        [Test]
        public void DefaultIncludeTest()
        {
            bool valid = selector.IsTypeValid(typeof(TestNode));
            Assert.IsTrue(valid);
        }

        [Test]
        public void IncludeBaseTypeTest()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.BaseType;
            TypeSelector selector = new TypeSelector(null, typeof(Exception), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(Exception));
            Assert.IsTrue(valid);
        }

        [Test]
        public void ExcludeBaseTypeTest()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.Default;
            TypeSelector selector = new TypeSelector(null, typeof(Exception), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(Exception));
            Assert.IsFalse(valid);
        }

        [Test]
        public void IncludeAbstractTypesTest()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.AbstractTypes;
            TypeSelector selector = new TypeSelector(null, typeof(ITest), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(AbstractTest));
            Assert.IsTrue(valid);
            valid = selector.IsTypeValid(typeof(ITest2));
            Assert.IsTrue(valid);
        }

        [Test]
        public void ExcludeAbstractTypesTest()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.Default;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(Stream));
            Assert.IsFalse(valid);
        }

        [Test]
        public void IncludeAllInterfacesTest()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.Interfaces;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(IComparable));
            Assert.IsTrue(valid);
        }

        [Test]
        public void ExcludeAllInterfacesTest()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.Default;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(IComparable));
            Assert.IsFalse(valid);
        }

        [Test]
        public void IncludeNonPublicTypes()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.NonpublicTypes;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(NonPublicClass));
            Assert.IsTrue(valid);
        }

        [Test]
        public void ExcludeNonPublicTypes()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.Default;
            TypeSelector selector = new TypeSelector(null, typeof(MarshalByRefObject), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(NonPublicClass));
            Assert.IsFalse(valid);
        }

        [Test]
        public void FlagsTest()
        {
            TypeSelectorIncludeFlags flags = TypeSelectorIncludeFlags.AbstractTypes |
                TypeSelectorIncludeFlags.Interfaces |
                TypeSelectorIncludeFlags.BaseType;
            TypeSelector selector = new TypeSelector(null, typeof(ITest), flags, treeView);
            bool valid = selector.IsTypeValid(typeof(AbstractTest));
            Assert.IsTrue(valid);
            valid = selector.IsTypeValid(typeof(ITest));
            Assert.IsTrue(valid);
        }

        [Test]
        public void IncludeNestedPublic()
        {
            TypeSelector selector = new TypeSelector(null, typeof(EventArgs), TypeSelectorIncludeFlags.Default, treeView);
            bool valid = selector.IsTypeValid(typeof(MockInnerTypeTest.InnerInner));
            Assert.IsTrue(valid);
        }

        public class MockInnerTypeTest
        {
            public class InnerInner : EventArgs
            {
            }
        }
    }
}

#endif