//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestFixture]
    public class ContextItemsFixture
    {
        [SetUp]
        public void Setup()
        {
            Logger.FlushContextItems();
        }

        [TearDown]
        public void Teardown()
        {
            Logger.FlushContextItems();
            MockLogSink.Clear();
        }

        [Test]
        public void AddItemToCallContext()
        {
            ContextItems item = new ContextItems();
            item.SetContextItem("AppVersion", "1234");

            Hashtable hash = (Hashtable)CallContext.GetData(ContextItems.CallContextSlotName);
            Assert.IsNotNull(hash);
            Assert.AreEqual(1, hash.Count);
            Assert.AreEqual("1234", hash["AppVersion"]);
        }

        [Test]
        public void FlushItemsFromCallContext()
        {
            ContextItems item = new ContextItems();
            item.SetContextItem("AppVersion", "1234");

            Hashtable hash = (Hashtable)CallContext.GetData(ContextItems.CallContextSlotName);
            Assert.IsNotNull(hash);
            Assert.AreEqual(1, hash.Count);

            item.FlushContextItems();
            Hashtable hash2 = (Hashtable)CallContext.GetData(ContextItems.CallContextSlotName);
            Assert.IsNull(hash2);
        }

        [Test]
        public void AddItemThenLog()
        {
            Logger.SetContextItem("AppVersion", "1234");
            LogEntry log = new LogEntry();
            log.Category = "MockCategoryOne";

            Logger.Write(log);

            Assert.AreEqual(1, MockLogSink.GetLastEntry().ExtendedProperties.Count);
            Assert.AreEqual("1234", MockLogSink.GetLastEntry().ExtendedProperties["AppVersion"]);
        }

        [Test]
        public void AddTwoItemsThenLog()
        {
            Logger.SetContextItem("AppVersion", "1234");
            Logger.SetContextItem("BuildNumber", "5678");
            LogEntry log = new LogEntry();
            log.Category = "MockCategoryOne";
            Logger.Write(log);

            Assert.AreEqual(2, MockLogSink.GetLastEntry().ExtendedProperties.Count);
            Assert.AreEqual("1234", MockLogSink.GetLastEntry().ExtendedProperties["AppVersion"]);
            Assert.AreEqual("5678", MockLogSink.GetLastEntry().ExtendedProperties["BuildNumber"]);
        }

        [Test]
        public void AddItemsAndDictionaryThenLog()
        {
            Logger.SetContextItem("AppVersion", "1234");
            Logger.SetContextItem("BuildNumber", "5678");
            LogEntry log = new LogEntry();
            log.Category = "MockCategoryOne";
            log.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            Logger.Write(log);

            Assert.AreEqual(5, MockLogSink.GetLastEntry().ExtendedProperties.Count);
            Assert.AreEqual("1234", MockLogSink.GetLastEntry().ExtendedProperties["AppVersion"]);
            Assert.AreEqual("5678", MockLogSink.GetLastEntry().ExtendedProperties["BuildNumber"]);
        }

        [Test]
        public void FlushItems()
        {
            Logger.SetContextItem("AppVersion", "1234");
            Logger.SetContextItem("BuildNumber", "5678");
            LogEntry log = new LogEntry();
            log.Category = "MockCategoryOne";
            log.ExtendedProperties = CommonUtil.GetPropertiesHashtable();

            Logger.FlushContextItems();
            Logger.Write(log);

            Assert.AreEqual(3, MockLogSink.GetLastEntry().ExtendedProperties.Count);
            Assert.IsNull(MockLogSink.GetLastEntry().ExtendedProperties["AppVersion"]);
            Assert.IsNull(MockLogSink.GetLastEntry().ExtendedProperties["BuildNumber"]);
        }

        [Test]
        public void AddObjectAsContextItem()
        {
            ContextObject obj = new ContextObject();
            Logger.SetContextItem("object", obj);

            LogEntry log = CommonUtil.GetDefaultLogEntry();
            log.Category = "MockCategoryOne";
            Logger.Write(log);

            string result = MockLogSink.GetLastEntry().ExtendedProperties["object"].ToString();
            string expected = obj.ToString();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CustomContextByteArrayObject()
        {
            Guid expectedGuid = Guid.NewGuid();
            byte[] byteArray = expectedGuid.ToByteArray();

            Logger.SetContextItem("bytes", byteArray);

            LogEntry log = CommonUtil.GetDefaultLogEntry();
            log.Category = "MockCategoryOne";
            Logger.Write(log);

            // get body and parse out the byte array and go back to the guid form
            string guidArray = MockLogSink.GetLastEntry().ExtendedProperties["bytes"].ToString();
            byteArray = Convert.FromBase64String(guidArray);

            Guid resultGuid = new Guid(byteArray);
            Assert.AreEqual(expectedGuid, resultGuid);
        }

        internal class ContextObject
        {
            public override string ToString()
            {
                return "Scooby Doo Loves You";
            }
        }
    }
}

#endif