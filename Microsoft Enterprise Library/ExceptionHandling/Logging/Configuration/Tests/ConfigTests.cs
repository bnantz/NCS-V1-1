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

#if UNIT_TESTS

using Microsoft.Practices.EnterpriseLibrary.Logging;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Tests
{
    [TestFixture]
    public class ConfigTests
    {
        [Test]
        public void LoggingExceptionHandlerDataPropertiesTest()
        {
            LoggingExceptionHandlerData data = new LoggingExceptionHandlerData();
            string defaultLogCategory = "default log category";
            int defaultEventID = 5;
            Severity defaultSeverity = Severity.Error;
            int minimumPriority = 2;
            string defaultTitle = "default title";
            string formatterTypeName = "typeName";
            string typeName = typeof(LoggingExceptionHandler).AssemblyQualifiedName;

            data.DefaultLogCategory = defaultLogCategory;
            data.DefaultEventID = defaultEventID;
            data.DefaultSeverity = defaultSeverity;
            data.DefaultTitle = defaultTitle;
            data.FormatterTypeName = formatterTypeName;
            data.MinimumPriority = minimumPriority;

            Assert.AreEqual(defaultLogCategory, data.DefaultLogCategory);
            Assert.AreEqual(defaultEventID, data.DefaultEventID);
            Assert.AreEqual(defaultSeverity, data.DefaultSeverity);
            Assert.AreEqual(defaultTitle, data.DefaultTitle);
            Assert.AreEqual(formatterTypeName, data.FormatterTypeName);
            Assert.AreEqual(typeName, data.TypeName);
            Assert.AreEqual(minimumPriority, data.MinimumPriority);
        }
    }
}

#endif