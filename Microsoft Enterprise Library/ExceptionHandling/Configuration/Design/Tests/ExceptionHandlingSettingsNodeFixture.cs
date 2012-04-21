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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestFixture]
    public class ExceptionHandlingSettingsNodeFixture : ConfigurationDesignHostTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            ExceptionHandlingConfigurationDesignManager manager = new ExceptionHandlingConfigurationDesignManager();
            CreateHierarchyAndAddToHierarchyService(new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain()), CreateDefaultConfiguration());
            manager.Register(Host);
        }

        [Test]
        public void RuntimeTest()
        {
            ExceptionPolicyData policyData = new ExceptionPolicyData();
            policyData.Name = "Default Policy";

            ExceptionTypeData typeData = new ExceptionTypeData();
            typeData.Name = "ApplicationException";

            CustomHandlerData handlerData = new CustomHandlerData();
            handlerData.Name = "MockExceptionHandler";

            ExceptionHandlingSettings settings = new ExceptionHandlingSettings();
            settings.ExceptionPolicies.Add(policyData);
            //settings.ExceptionPolicies[policyData.Name].ExceptionTypes = new ExceptionTypeDataCollection();
            settings.ExceptionPolicies[policyData.Name].ExceptionTypes.Add(typeData);
            //settings.ExceptionPolicies[policyData.Name].ExceptionTypes[typeData.Name].ExceptionHandlers = new ExceptionHandlerDataCollection();
            settings.ExceptionPolicies[policyData.Name].ExceptionTypes[typeData.Name].ExceptionHandlers.Add(handlerData);

            ExceptionHandlingSettingsNode settingsNode = new ExceptionHandlingSettingsNode(settings);
            HierarchyService.SelectedHierarchy.RootNode.Nodes.Add(settingsNode);
            Assert.AreEqual(policyData.Name, settingsNode.Nodes[0].Name);
            Assert.AreEqual(typeData.Name, settingsNode.Nodes[0].Nodes[0].Name);
            Assert.AreEqual(handlerData.Name, settingsNode.Nodes[0].Nodes[0].Nodes[0].Name);
        }
    }
}

#endif