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

#if UNIT_TESTS
using System.Reflection;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestFixture]
    public class AssertRangeAttributeFixture
    {
        private MyTestClass rangeClass;
        private PropertyInfo int32Info;
        private PropertyInfo doubleInfo;
        private PropertyInfo floatInfo;
        private PropertyInfo charInfo;
        private PropertyInfo stringInfo;

        [TestFixtureSetUp]
        public void Init()
        {
            rangeClass = new MyTestClass();
            int32Info = rangeClass.GetType().GetProperty("Int32Value");
            Assert.IsNotNull(int32Info);
            doubleInfo = rangeClass.GetType().GetProperty("DoubleValue");
            Assert.IsNotNull(doubleInfo);
            floatInfo = rangeClass.GetType().GetProperty("FloatValue");
            Assert.IsNotNull(floatInfo);
            charInfo = rangeClass.GetType().GetProperty("CharValue");
            Assert.IsNotNull(charInfo);
            stringInfo = rangeClass.GetType().GetProperty("StringValue");
            Assert.IsNotNull(stringInfo);
        }

        [Test]
        public void Int32OutOfRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute(3, RangeBoundaryType.Inclusive, 43, RangeBoundaryType.Exclusive);
            rangeClass.Int32Value = 2;
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, int32Info, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void Int32InRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute(3, RangeBoundaryType.Inclusive, 43, RangeBoundaryType.Exclusive);
            rangeClass.Int32Value = 25;
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, int32Info, errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void DoubleOutOfRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute(3D, RangeBoundaryType.Inclusive, 43D, RangeBoundaryType.Exclusive);
            rangeClass.DoubleValue = 2;
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, doubleInfo, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void DoubleInRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute(3D, RangeBoundaryType.Inclusive, 43D, RangeBoundaryType.Exclusive);
            rangeClass.DoubleValue = 25;
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, doubleInfo, errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void FloatOutOfRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute(3f, RangeBoundaryType.Inclusive, 43f, RangeBoundaryType.Exclusive);
            rangeClass.FloatValue = 2;
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, floatInfo, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void FloatInRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute(3f, RangeBoundaryType.Inclusive, 43f, RangeBoundaryType.Exclusive);
            rangeClass.FloatValue = 25;
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, floatInfo, errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void CharOutOfRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute('a', RangeBoundaryType.Inclusive, 'd', RangeBoundaryType.Exclusive);
            rangeClass.CharValue = 'd';
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, charInfo, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void CharInRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute('a', RangeBoundaryType.Inclusive, 'd', RangeBoundaryType.Exclusive);
            rangeClass.CharValue = 'c';
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, charInfo, errors);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void StringOutOfRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute("Anna", RangeBoundaryType.Inclusive, "Scott", RangeBoundaryType.Exclusive);
            rangeClass.StringValue = "Zeek";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, stringInfo, errors);
            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void StringInRangeTest()
        {
            AssertRangeAttribute assertRange = new AssertRangeAttribute("Anna", RangeBoundaryType.Inclusive, "Grace", RangeBoundaryType.Exclusive);
            rangeClass.StringValue = "Dee";
            ValidationErrorCollection errors = new ValidationErrorCollection();
            assertRange.Validate(rangeClass, stringInfo, errors);
            Assert.AreEqual(0, errors.Count);
        }

        private class MyTestClass
        {
            private int int32Value;
            private double doubleValue;
            private float floatValue;
            private char charValue;
            private string stringValue;

            public int Int32Value
            {
                get { return int32Value; }
                set { int32Value = value; }
            }

            public double DoubleValue
            {
                get { return doubleValue; }
                set { doubleValue = value; }
            }

            public float FloatValue
            {
                get { return floatValue; }
                set { floatValue = value; }
            }

            public char CharValue
            {
                get { return charValue; }
                set { charValue = value; }
            }

            public string StringValue
            {
                get { return stringValue; }
                set { stringValue = value; }
            }
        }
    }
}

#endif