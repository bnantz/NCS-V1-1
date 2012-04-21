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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestFixture]
    public class TextExceptionFormatterFixture
    {
        [Test]
        public void CreateTest()
        {
            TextWriter writer = new StringWriter(TestUtility.DefaultCulture);
            Exception exception = new Exception();
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);

            Assert.AreSame(writer, formatter.Writer);
            Assert.AreSame(exception, formatter.Exception);
        }

        [Test]
        public void SimpleFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb, TestUtility.DefaultCulture);

            Exception exception = new MockException();

            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);

            // Nothing should be written until Format() is called
            Assert.IsTrue(sb.Length == 0);

            // Format the exception
            formatter.Format();

            // Not much of a test, but at least we can tell if _something_ got written
            // to the underlying StringBuilder
            Assert.IsTrue(sb.Length > 0);
        }
    }
}

#endif