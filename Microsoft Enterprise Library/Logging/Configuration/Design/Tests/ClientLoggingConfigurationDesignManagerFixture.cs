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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.CategoryFilters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestFixture]
    public class ClientLoggingConfigurationDesignManagerFixture : ConfigurationDesignHostTestBase
    {

        public override void SetUp()
        {
            base.SetUp();
            ClientLoggingConfigurationDesignManager loggingConfigurationDesignManager = new ClientLoggingConfigurationDesignManager();
            loggingConfigurationDesignManager.Register(Host);
        }

        [Test]
        public void InitTest()
        {
            new ClientLoggingConfigurationDesignManager();
        }

        [Test]
        public void OpenAndSaveTest()
        {
            GeneratedHierarchy.Open();
            Assert.AreEqual(0, ConfigurationErrorsCount);

            ConfigurationNode rootNode = GeneratedHierarchy.FindNodeByType(typeof(ClientSettingsNode));

            Assert.IsNotNull(rootNode);
            Assert.AreEqual(typeof(ClientSettingsNode), rootNode.GetType());

            GeneratedHierarchy.Save();

            ApplicationConfigurationNode node = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            IUIHierarchy hierarchy = CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            HierarchyService.SelectedHierarchy = hierarchy;
            Assert.IsTrue(Object.ReferenceEquals(HierarchyService.SelectedHierarchy, hierarchy));
            Assert.IsFalse(Object.ReferenceEquals(GeneratedHierarchy.StorageTable, hierarchy.StorageTable));
            hierarchy.Open();
            Assert.AreEqual(0, ConfigurationErrorsCount);

            Assert.IsTrue(hierarchy.ConfigurationContext.IsValidSection(LoggingSettings.SectionName));
        }


        [Test]
        public void ClientSettingsNodeTest()
        {
            DistributionStrategyNode distributionStrategy = new InProcDistributionStrategyNode();
            bool loggingEnabled = true;
            int minimumPriority = 1;
            bool tracingEnabled = true;

            ClientSettingsNode node = new ClientSettingsNode();
            GeneratedApplicationNode.Nodes.Add(node);
            node.CategoryFilterSettings = new CategoryFilterSettings(CategoryFilterMode.DenyAllExceptAllowed, new CategoryFilterDataCollection());
            node.DistributionStrategy = distributionStrategy;
            node.LoggingEnabled = loggingEnabled;
            node.MinimumPriority = minimumPriority;
            node.TracingEnabled = tracingEnabled;

            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, node.CategoryFilterSettings.CategoryFilterMode);
            Assert.AreEqual(distributionStrategy, node.DistributionStrategy);
            Assert.AreEqual(loggingEnabled, node.LoggingEnabled);
            Assert.AreEqual(minimumPriority, node.MinimumPriority);
            Assert.AreEqual(tracingEnabled, tracingEnabled);
        }

        [Test]
        public void ClientSettingsDataTest()
        {
            DistributionStrategyDataCollection distStrategies = new DistributionStrategyDataCollection();
            DistributionStrategyData distData = new InProcDistributionStrategyData();
            distData.Name = "Test";
            distStrategies.Add(distData);

            LoggingSettings settings = new LoggingSettings();
            settings.CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed;
            settings.CategoryFilters = new CategoryFilterDataCollection();
            foreach (DistributionStrategyData distributionStrategyData in distStrategies)
            {
                settings.DistributionStrategies.Add(distributionStrategyData);    
            }
            settings.LoggingEnabled = true;
            settings.MinimumPriority = 0;
            settings.TracingEnabled = true;

            ClientSettingsNode node = new ClientSettingsNode(settings);
            GeneratedApplicationNode.Nodes.Add(node);
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, node.CategoryFilterSettings.CategoryFilterMode);
            Assert.AreEqual(0, node.CategoryFilterSettings.CategoryFilters.Count);
            Assert.AreEqual(true, node.LoggingEnabled);
            Assert.AreEqual(0, node.MinimumPriority);
            Assert.AreEqual(true, node.TracingEnabled);

            LoggingSettings nodeData = node.LoggingSettings;
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, nodeData.CategoryFilterMode);
            Assert.AreEqual(0, nodeData.CategoryFilters.Count);
            Assert.AreEqual("Test", nodeData.DistributionStrategies["Test"].Name);
            Assert.AreEqual(true, nodeData.LoggingEnabled);
            Assert.AreEqual(0, nodeData.MinimumPriority);
            Assert.AreEqual(true, nodeData.TracingEnabled);
        }

       

        [Test]
        public void ClientSettingsDefaultChildNodesTest()
        {
            ClientSettingsNode node = new ClientSettingsNode();
            GeneratedApplicationNode.Nodes.AddWithDefaultChildren(node);
            Assert.AreEqual(1, node.Nodes.Count);
            Assert.AreEqual(typeof(DistributionStrategyCollectionNode), node.Nodes[0].GetType());
        }

        [Test]
        public void RuntimeTest()
        {
            GeneratedHierarchy.Open();
            Assert.AreEqual(0, ConfigurationErrorsCount);

            ConfigurationContext configurationContext = GeneratedHierarchy.ConfigurationContext;
            if (configurationContext.IsValidSection(LoggingSettings.SectionName))
            {
                LoggingSettings clientSettings = configurationContext.GetConfiguration(LoggingSettings.SectionName) as LoggingSettings;
                if (clientSettings != null)
                {
                    ClientSettingsNode settingsNode = new ClientSettingsNode(clientSettings);
                    Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, settingsNode.CategoryFilterSettings.CategoryFilterMode);
                    Assert.AreEqual(false, settingsNode.LoggingEnabled);
                    Assert.AreEqual(1, settingsNode.MinimumPriority);
                    Assert.AreEqual(false, settingsNode.TracingEnabled);

                }
            }
            else
            {
                Assert.Fail(String.Format("Can not load section: {0}", LoggingSettings.SectionName));
            }
        }

        [Test]
        public void DesigntimeTest()
        {
            LoggingSettings settings = ConfigurationManager.GetConfiguration(LoggingSettings.SectionName) as LoggingSettings;
            Assert.IsNotNull(settings);

            ClientSettingsNode settingsNode = new ClientSettingsNode(settings);
            GeneratedApplicationNode.Nodes.Add(settingsNode);
            Assert.AreEqual(settings.CategoryFilterMode, settingsNode.CategoryFilterSettings.CategoryFilterMode);
            Assert.AreEqual(settings.LoggingEnabled, settingsNode.LoggingEnabled);
            Assert.AreEqual(settings.MinimumPriority, settingsNode.MinimumPriority);
            Assert.AreEqual(settings.Name, settingsNode.Name);
            Assert.AreEqual(settings.TracingEnabled, settingsNode.TracingEnabled);
        }

        [Test]
        public void BuildContextTest()
        {
            ClientLoggingConfigurationDesignManager b = new ClientLoggingConfigurationDesignManager();
            GeneratedHierarchy.Open();
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            b.BuildContext(Host, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.Contains(LoggingSettings.SectionName));
        }
    }
}

#endif