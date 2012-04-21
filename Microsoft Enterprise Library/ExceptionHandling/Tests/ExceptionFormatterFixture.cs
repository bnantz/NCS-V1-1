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
using System.Security.Principal;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestFixture]
    public class ExceptionFormatterFixture
    {
        [Test]
        public void AdditionalInfoTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb, TestUtility.DefaultCulture);

            Exception exception = new FileNotFoundException("The file can't be found", "theFile");
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);

            formatter.Format();

            Assert.AreEqual(Environment.MachineName, formatter.AdditionalInfo["MachineName"]);

            DateTime minimumTime = DateTime.Now.AddMinutes(-1);
            DateTime loggedTime = DateTime.Parse(formatter.AdditionalInfo["TimeStamp"]);
            if (DateTime.Compare(minimumTime, loggedTime) > 0)
            {
                Assert.Fail("Logged TimeStamp is not within a one minute time window");
            }

            Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, formatter.AdditionalInfo["AppDomainName"]);
            Assert.AreEqual(Thread.CurrentPrincipal.Identity.Name, formatter.AdditionalInfo["ThreadIdentity"]);
            Assert.AreEqual(WindowsIdentity.GetCurrent().Name, formatter.AdditionalInfo["WindowsIdentity"]);
        }

        [Test]
        public void ReflectionFormatterReadTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb, TestUtility.DefaultCulture);

            MockTextExceptionFormatter formatter = new MockTextExceptionFormatter(writer, new MockException());

            formatter.Format();

            Assert.AreEqual(formatter.fields["FieldString"], "MockFieldString");
            Assert.AreEqual(formatter.properties["PropertyString"], "MockPropertyString");
            // The message should be null because the reflection formatter should ignore this property
            Assert.AreEqual(null, formatter.properties["Message"]);
        }
    }
}

#endif