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

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestFixture]
    public class ExceptionHandlingConfigurationDesignManagerFixture : ConfigurationDesignHostTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            CreateHierarchyAndAddToHierarchyService(new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain()), CreateDefaultConfiguration());
            GeneratedHierarchy.Load();
        }

        [Test]
        public void OpenAndSaveTest()
        {
            HierarchyService.SelectedHierarchy.Open();
            Assert.AreEqual(0, base.ConfigurationErrorsCount);

            ConfigurationNode rootNode = HierarchyService.SelectedHierarchy.FindNodeByType(typeof(ExceptionHandlingSettingsNode));

            Assert.IsNotNull(rootNode);
            Assert.AreEqual(typeof(ExceptionHandlingSettingsNode), rootNode.GetType());

            ExceptionPolicyNode policy = rootNode.Nodes[0] as ExceptionPolicyNode;

            Assert.AreEqual(policy.Name, "Default Policy");

            HierarchyService.SelectedHierarchy.Save();
            Assert.AreEqual(0, base.ConfigurationErrorsCount);
        }

        [Test]
        public void BuildContextTest()
        {
            HierarchyService.SelectedHierarchy.Open();
            ExceptionHandlingConfigurationDesignManager b = new ExceptionHandlingConfigurationDesignManager();
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            b.BuildContext(Host, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.Contains("exceptionHandlingConfiguration"));
        }
    }
}

#endif