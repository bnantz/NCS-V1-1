////===============================================================================
//// Microsoft patterns & practices Enterprise Library
//// Exception Handling Application Block
////===============================================================================
//// Copyright © Microsoft Corporation.  All rights reserved.
//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
//// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
//// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//// FITNESS FOR A PARTICULAR PURPOSE.
////===============================================================================
//
//#if  UNIT_TESTS
//using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
//using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories;
//using NUnit.Framework;
//
//namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Tests
//{
//    [TestFixture]
//    public class DesignerTests
//    {
//        private ApplicationConfigurationNode applicationNode;
//
//        [SetUp]
//        public void Setup()
//        {
//            ConfigurationDesignManager manager = new ConfigurationDesignManager();
//            ApplicationData data = ApplicationData.FromCurrentAppDomain();
//            applicationNode = new ApplicationConfigurationNode(data.Name, data);
//            manager.Register(applicationNode, null);
//        }
//
//        [Test]
//        public void LoggingExceptionHandlerNodeOpenTest()
//        {
//            LoggingExceptionHandlerData data = new LoggingExceptionHandlerData();
//            int defaultEventID = 5;
//            Severity defaultSeverity = Severity.Error;
//            string defaultTitle = "default title";
//            string formatterTypeName = "typeName";
//            int minimumPriority = 2;
//
//            data.DefaultEventID = defaultEventID;
//            data.DefaultSeverity = defaultSeverity;
//            data.DefaultTitle = defaultTitle;
//            data.FormatterTypeName = formatterTypeName;
//            data.MinimumPriority = minimumPriority;
//
//            LoggingExceptionHandlerNode node = new LoggingExceptionHandlerNode(data);
//
//            Assert.AreEqual(typeof(LoggingExceptionHandler).AssemblyQualifiedName, node.TypeName);
//            Assert.AreEqual(defaultEventID, node.EventID);
//            Assert.AreEqual(defaultSeverity, node.Severity);
//            Assert.AreEqual(defaultTitle, node.Title);
//            Assert.AreEqual(formatterTypeName, node.FormatterTypeName);
//            Assert.AreEqual(minimumPriority, node.Priority);
//        }
//
//        [Test]
//        public void LoggingExceptionHandlerNodeSaveTest()
//        {
//            LoggingExceptionHandlerNode node = new LoggingExceptionHandlerNode();
//            int defaultEventID = 5;
//            Severity defaultSeverity = Severity.Error;
//            CategoryNode defaultCat = new CategoryNode();
//            defaultCat.Name = "default title";
//            string defaultTitle = "default Title";
//            string formatterTypeName = "typeName";
//            int minimumPriority = 2;
//
//            node.EventID = 5;
//            node.LogCategory = defaultCat;
//            node.Severity = defaultSeverity;
//            node.Title = defaultTitle;
//            node.FormatterTypeName = formatterTypeName;
//            node.Priority = minimumPriority;
//
//            LoggingExceptionHandlerData data = (LoggingExceptionHandlerData)node.GetExceptionHandlerData();
//
//            Assert.AreEqual(defaultEventID, data.DefaultEventID);
//            Assert.AreEqual(defaultCat.Name, data.DefaultLogCategory);
//            Assert.AreEqual(defaultSeverity, data.DefaultSeverity);
//            Assert.AreEqual(defaultTitle, data.DefaultTitle);
//            Assert.AreEqual(formatterTypeName, data.FormatterTypeName);
//            Assert.AreEqual(minimumPriority, data.MinimumPriority);
//        }
//    }
//}
//
//#endif