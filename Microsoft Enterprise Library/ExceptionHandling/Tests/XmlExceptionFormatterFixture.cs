//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if    UNIT_TESTS
using System;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestFixture]
    public class XmlExceptionFormatterFixture
    {
        private Exception ex;

        [SetUp]
        public void Setup()
        {
            ex = new MockException();
        }

        [Test]
        public void CreateXmlWriterTest()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb, TestUtility.DefaultCulture));

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);

            Assert.AreSame(writer, formatter.Writer);
            Assert.AreSame(ex, formatter.Exception);
        }

        [Test]
        public void CreateTextWriterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb, TestUtility.DefaultCulture);

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);

            // TextWriter won't be the same so we can only test the Exception
            Assert.AreSame(ex, formatter.Exception);
        }

        [Test]
        public void SimpleXmlWriterFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb, TestUtility.DefaultCulture));

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);

            Assert.IsTrue(sb.Length == 0);

            formatter.Format();

            Assert.IsTrue(sb.Length > 0);
        }

        [Test]
        public void SimpleTextWriterFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb, TestUtility.DefaultCulture);

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);

            Assert.IsTrue(sb.Length == 0);

            formatter.Format();

            Assert.IsTrue(sb.Length > 0);
        }

        [Test]
        public void WellFormedTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb, TestUtility.DefaultCulture);

            XmlExceptionFormatter formatter = new XmlExceptionFormatter(writer, ex);
            formatter.Format();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
        }
    }
}

#endif