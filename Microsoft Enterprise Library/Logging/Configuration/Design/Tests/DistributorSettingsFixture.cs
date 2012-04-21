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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestFixture]
    public class DistributorSettingsFixture : ConfigurationDesignHostTestBase
    { 
        private ApplicationConfigurationNode applicationNode;

        
        public override void SetUp()
        {
            base.SetUp();
            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            applicationNode = new ApplicationConfigurationNode(data);
            CreateHierarchyAndAddToHierarchyService(applicationNode, CreateDefaultConfiguration());
            DistributorLoggingConfigurationDesignManager loggingConfigurationDesignManager = new DistributorLoggingConfigurationDesignManager();
            loggingConfigurationDesignManager.Register(Host);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void YouCanOnlyAddTheFormattersCollectionOnceToTheDistributorsSettingsNode()
        {
            DistributorSettingsNode distributorSettings = new DistributorSettingsNode();
            applicationNode.Nodes.AddWithDefaultChildren(distributorSettings);
            distributorSettings.Nodes.Add(new FormatterCollectionNode());
        }

        [Test]
        public void DistributorSettingsPropertiesTest()
        {
            CategoryData categoryData = new CategoryData();
            categoryData.Name = SR.DefaultCategory;
            CategoryNode defaultCategory = new CategoryNode(categoryData);
            TextFormatterData formatterData = new TextFormatterData();
            formatterData.Name = SR.DefaultFormatter;
            TextFormatterNode defaultFormatter = new TextFormatterNode(formatterData);

            DistributorSettingsNode distributorSettings = new DistributorSettingsNode();
            applicationNode.Nodes.Add(distributorSettings);
            distributorSettings.DefaultFormatter = defaultFormatter;
            distributorSettings.DefaultCategory = defaultCategory;

            Assert.AreEqual(defaultCategory.Name, distributorSettings.DefaultCategory.Name);
            Assert.AreEqual(defaultFormatter, distributorSettings.DefaultFormatter);
        }

        [Test]
        public void AddChildNodesTest()
        {
            DistributorSettingsNode node = new DistributorSettingsNode();
            applicationNode.Nodes.AddWithDefaultChildren(node);
            Assert.AreEqual(3, node.Nodes.Count);
            bool categories = false;
            bool formatters = false;
            bool sinks = false;

            foreach (ConfigurationNode configNode in node.Nodes)
            {
                if (configNode is CategoryCollectionNode)
                {
                    categories = true;
                }
                else if (configNode is FormatterCollectionNode)
                {
                    formatters = true;
                }
                else if (configNode is SinkCollectionNode)
                {
                    sinks = true;
                }
            }

            Assert.IsTrue(categories && formatters && sinks);
        }
    }
}

#endif