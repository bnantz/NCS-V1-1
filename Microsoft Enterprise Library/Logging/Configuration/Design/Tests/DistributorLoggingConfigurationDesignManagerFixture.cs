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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestFixture]
    //[Ignore("")]
    public class DistributorLoggingConfigurationDesignManagerFixture : ConfigurationDesignHostTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            DistributorLoggingConfigurationDesignManager loggingConfigurationDesignManager = new DistributorLoggingConfigurationDesignManager();
            loggingConfigurationDesignManager.Register(Host);
        }

        [Test]
        public void InitTest()
        {
            new DistributorLoggingConfigurationDesignManager();
        }

        
        [Test]
        public void OpenAndSaveTest()
        {
            GeneratedHierarchy.Open();
            Assert.AreEqual(0, ConfigurationErrorsCount);

            ConfigurationNode rootNode = GeneratedHierarchy.FindNodeByType(typeof(DistributorSettingsNode));

            Assert.IsNotNull(rootNode);
            Assert.AreEqual(typeof(DistributorSettingsNode), rootNode.GetType());

            GeneratedHierarchy.Save();

            ApplicationConfigurationNode node = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            HierarchyService.SelectedHierarchy = hierarchy;
            hierarchy.Open();
            Assert.AreEqual(0, ConfigurationErrorsCount);

            Assert.IsTrue(hierarchy.ConfigurationContext.IsValidSection(DistributorSettings.SectionName));
        }

        [Test]
        public void RuntimeTest()
        {
            GeneratedHierarchy.Open();
            Assert.AreEqual(0, ConfigurationErrorsCount);

            ConfigurationContext builder = GeneratedHierarchy.ConfigurationContext;
            if (builder.IsValidSection(DistributorSettings.SectionName))
            {
                DistributorSettings distributorSettings = builder.GetConfiguration(DistributorSettings.SectionName) as DistributorSettings;
                if (distributorSettings != null)
                {
                    DistributorSettingsNode distributorSettingsNode = new DistributorSettingsNode(distributorSettings);
                    foreach (ConfigurationNode node in distributorSettingsNode.Nodes)
                    {
                        if (node is SinkCollectionNode)
                        {
                            SinkCollectionNode sinkCollectionNode = (SinkCollectionNode) node;
                            Assert.AreEqual(4, sinkCollectionNode.SinkDataCollection.Count);
                        }
                        else if (node is FormatterCollectionNode)
                        {
                            FormatterCollectionNode formatterCollectionNode = (FormatterCollectionNode) node;
                            Assert.AreEqual(4, formatterCollectionNode.FormatterDataCollection.Count);
                        }
                        else if (node is CategoryCollectionNode)
                        {
                            CategoryCollectionNode categoryCollectionNode = (CategoryCollectionNode) node;
                            Assert.AreEqual(2, categoryCollectionNode.CategoryDataCollection.Count);
                        }
                    }

                    MsmqDistributorServiceNode msmqNode =
                        new MsmqDistributorServiceNode(distributorSettings.DistributorService);
                    Assert.IsNotNull(msmqNode.MsmqPath);

                }
                else
                {
                    Assert.Fail(String.Format("Can not load section: {0}", DistributorSettings.SectionName));
                }
            }
        }

        [Test]
        public void DistributorSettingsPropertiesTest()
        {
            DistributorSettings settings = ConfigurationManager.GetConfiguration(DistributorSettings.SectionName) as DistributorSettings;

            DistributorSettingsNode settingsNode = new DistributorSettingsNode(settings);
            GeneratedApplicationNode.Nodes.Add(settingsNode);
            settingsNode.ResolveNodeReferences();
            Assert.AreEqual(settings.DefaultCategory, settingsNode.DefaultCategory.Name);
            Assert.AreEqual(settings.DefaultFormatter, settingsNode.DefaultFormatter.Name);
        }

        [Test]
        public void CustomFormatterNodeInitTest()
        {
            CustomFormatterData data = new CustomFormatterData();
            CustomFormatterNode node = new CustomFormatterNode(data);
            GeneratedApplicationNode.Nodes.Add(node);
            Assert.AreSame(data, node.FormatterData);
        }

        [Test]
        public void CustomSinkDataTest()
        {
            NameValueItem item = new NameValueItem("TEST", "VALUE");
            string name = "testName";
            string type = "testType";

            CustomSinkData data = new CustomSinkData();
            data.Attributes.Add(item);
            data.Name = name;
            data.TypeName = type;

            CustomSinkNode node = new CustomSinkNode(data);
            GeneratedApplicationNode.Nodes.Add(node);
            CustomSinkData nodeData = (CustomSinkData) node.SinkData;

            Assert.AreEqual(item, nodeData.Attributes[0]);
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.TypeName);

        }

        [Test]
        public void CustomSinkNodeTest()
        {
            NameValueItem item = new NameValueItem("TEST", "VALUE");
            string name = "testName";
            string type = "testType";

            CustomSinkNode node = new CustomSinkNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.Attributes.Add(item);
            node.Name = name;
            node.TypeName = type;

            Assert.AreEqual(item, node.Attributes[0]);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type, node.TypeName);

            Assert.AreEqual(1, node.Attributes.Count);
        }

        [Test]
        public void CustomFormatterDataTest()
        {
            NameValueItem testItem = new NameValueItem("TEST", "VALUE");
            string name = "testName";
            string type = "testType";

            CustomFormatterData data = new CustomFormatterData();
            data.Attributes.Add(testItem);
            data.TypeName = type;
            data.Name = name;

            CustomFormatterNode node = new CustomFormatterNode(data);
            GeneratedApplicationNode.Nodes.Add(node);
            CustomFormatterData nodeData = (CustomFormatterData) node.FormatterData;

            Assert.AreEqual(testItem, nodeData.Attributes[0]);
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.TypeName);
        }

        [Test]
        public void CustomFormatterNodeTest()
        {
            NameValueItem testItem = new NameValueItem("TEST", "VALUE");
            string name = "testName";
            string type = "testType";

            CustomFormatterNode node = new CustomFormatterNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.Name = name;
            node.TypeName = type;
            node.Attributes.Add(testItem);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type, node.TypeName);
            Assert.AreEqual(testItem, node.Attributes[0]);
            Assert.AreEqual(1, node.Attributes.Count);
        }

        [Test]
        public void FormattersAddChildNodesTest()
        {
            FormatterCollectionNode node = new FormatterCollectionNode();
            GeneratedApplicationNode.Nodes.AddWithDefaultChildren(node);
            Assert.AreEqual(1, node.Nodes.Count);
            Assert.AreEqual(typeof(TextFormatterNode), node.Nodes[0].GetType());
            Assert.AreEqual(SR.DefaultFormatter, node.Nodes[0].Name);
            Assert.AreEqual(SR.DefaultTextFormat, ((TextFormatterNode) node.Nodes[0]).Template.Text);
        }

        [Test]
        public void CategoriesDataTest()
        {
            CategoryDataCollection data = new CategoryDataCollection();
            CategoryData categoryData = new CategoryData();
            categoryData.Name = "Test";

            data.Add(categoryData);
            CategoryCollectionNode node = new CategoryCollectionNode(data);
            GeneratedApplicationNode.Nodes.Add(node);
            CategoryDataCollection nodeData = node.CategoryDataCollection;

            Assert.AreEqual(1, nodeData.Count);
            Assert.AreEqual(categoryData.Name, nodeData[categoryData.Name].Name);
        }

        [Test]
        public void EmailSinkDataTest()
        {
            string fromAddress = "testFromAddress";
            string name = "testName";
            string smtpServer = "testSmtpServer";
            string subjectLineEnder = "testSubjectLineEnder";
            string subjectLineStarter = "testSubjectLineStarter";
            string toAddress = "testToAddress";
            string typeName = typeof(EmailSink).AssemblyQualifiedName;

            EmailSinkData data = new EmailSinkData();
            data.FromAddress = fromAddress;
            data.Name = name;
            data.SmtpServer = smtpServer;
            data.SubjectLineEnder = subjectLineEnder;
            data.SubjectLineStarter = subjectLineStarter;
            data.ToAddress = toAddress;
            data.TypeName = typeName;

            EmailSinkNode node = new EmailSinkNode(data);
            GeneratedApplicationNode.Nodes.Add(node);
            EmailSinkData nodeData = (EmailSinkData) node.SinkData;

            Assert.AreEqual(fromAddress, nodeData.FromAddress);
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(smtpServer, nodeData.SmtpServer);
            Assert.AreEqual(subjectLineEnder, nodeData.SubjectLineEnder);
            Assert.AreEqual(subjectLineStarter, nodeData.SubjectLineStarter);
            Assert.AreEqual(toAddress, nodeData.ToAddress);
            Assert.AreEqual(typeName, nodeData.TypeName);
        }

        [Test]
        public void EmailSinkNodeTest()
        {
            string fromAddress = "testFromAddress";
            string name = "testName";
            string smtpServer = "testSmtpServer";
            string subjectLineEnder = "testSubjectLineEnder";
            string subjectLineStarter = "testSubjectLineStarter";
            string toAddress = "testToAddress";
            string typeName = typeof(EmailSink).AssemblyQualifiedName;

            EmailSinkNode node = new EmailSinkNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.FromAddress = fromAddress;
            node.Name = name;
            node.SmtpServer = smtpServer;
            node.SubjectLineEnder = subjectLineEnder;
            node.SubjectLineStarter = subjectLineStarter;
            node.ToAddress = toAddress;

            Assert.AreEqual(fromAddress, node.FromAddress);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(smtpServer, node.SmtpServer);
            Assert.AreEqual(subjectLineEnder, node.SubjectLineEnder);
            Assert.AreEqual(subjectLineStarter, node.SubjectLineStarter);
            Assert.AreEqual(toAddress, node.ToAddress);
            Assert.AreEqual(typeName, node.TypeName);
        }

        [Test]
        public void DestinationsNodeTest()
        {
            FormatterNode formatter = new TextFormatterNode();
            SinkNode sink = new CustomSinkNode();

            DestinationNode node = new DestinationNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.Formatter = formatter;
            node.Sink = sink;

            Assert.AreEqual(formatter, node.Formatter);
            Assert.AreEqual(sink, node.Sink);
        }

        [Test]
        public void EventLogSinkNodeTest()
        {
            string eventSourceName = "Application";
            string name = "testName";
            string typeName = typeof(EventLogSink).AssemblyQualifiedName;

            EventLogSinkNode node = new EventLogSinkNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.EventSourceName = eventSourceName;
            node.Name = name;

            Assert.AreEqual(eventSourceName, node.EventSourceName);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(typeName, node.TypeName);
        }

        [Test]
        public void FlatFileSinkDataTest()
        {
            string fileName = "testFlatFileName";
            string footer = "testFooter";
            string header = "testHeader";
            string name = "testName";
            string type = typeof(FlatFileSink).AssemblyQualifiedName;

            FlatFileSinkData data = new FlatFileSinkData();
            data.FileName = fileName;
            data.Footer = footer;
            data.Header = header;
            data.Name = name;
            data.TypeName = type;

            FlatFileSinkNode node = new FlatFileSinkNode(data);
            GeneratedApplicationNode.Nodes.Add(node);
            FlatFileSinkData nodeData = (FlatFileSinkData) node.SinkData;

            Assert.AreEqual(fileName, nodeData.FileName);
            Assert.AreEqual(footer, nodeData.Footer);
            Assert.AreEqual(header, nodeData.Header);
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.TypeName);
        }

        [Test]
        public void FlatFileSinkNodeTest()
        {
            string fileName = "testFlatFileName";
            string footer = "testFooter";
            string header = "testHeader";
            string name = "testName";
            string type = typeof(FlatFileSink).AssemblyQualifiedName;

            FlatFileSinkNode node = new FlatFileSinkNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.Filename = fileName;
            node.Footer = footer;
            node.Header = header;
            node.Name = name;

            Assert.AreEqual(fileName, node.Filename);
            Assert.AreEqual(footer, node.Footer);
            Assert.AreEqual(header, node.Header);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type, node.TypeName);
        }

        [Test]
        public void MsmqSinkDataTest()
        {
            string name = "testName";
            string queuePath = "testPath";
            string type = typeof(MsmqSink).AssemblyQualifiedName;

            MsmqSinkData data = new MsmqSinkData();
            data.Name = name;
            data.QueuePath = queuePath;
            data.TypeName = type;

            MsmqSinkNode node = new MsmqSinkNode(data);
            GeneratedApplicationNode.Nodes.Add(node);
            MsmqSinkData nodeData = (MsmqSinkData) node.SinkData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(queuePath, nodeData.QueuePath);
            Assert.AreEqual(type, nodeData.TypeName);
        }

        [Test]
        public void MsmqSinkNodeTest()
        {
            string name = "testName";
            string queuePath = "testPath";
            string type = typeof(MsmqSink).AssemblyQualifiedName;

            MsmqSinkNode node = new MsmqSinkNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.Name = name;
            node.QueuePath = queuePath;

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(queuePath, node.QueuePath);
            Assert.AreEqual(type, node.TypeName);
        }

        [Test]
        public void TextFormatterDataTest()
        {
            string name = "testName";
            string template = "<test>template</test>";
            string type = typeof(TextFormatter).AssemblyQualifiedName;

            TextFormatterData data = new TextFormatterData();
            data.Name = name;
            data.Template.Value = template;
            data.TypeName = type;

            TextFormatterNode node = new TextFormatterNode(data);
            GeneratedApplicationNode.Nodes.Add(node);
            TextFormatterData nodeData = (TextFormatterData) node.FormatterData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(template, nodeData.Template.Value);
            Assert.AreEqual(type, nodeData.TypeName);
        }

        [Test]
        public void TextFormatterNodeTest()
        {
            string name = "testName";
            string template = "<test>template</test>";
            string type = typeof(TextFormatter).AssemblyQualifiedName;

            TextFormatterNode node = new TextFormatterNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.Name = name;
            node.Template = new Template(template);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(template, node.Template.Text);
            Assert.AreEqual(type, node.TypeName);
        }
    }
}

#endif