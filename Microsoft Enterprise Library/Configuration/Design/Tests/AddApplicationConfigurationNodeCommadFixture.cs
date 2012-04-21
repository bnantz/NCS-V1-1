//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class AddApplicationConfigurationNodeCommadFixture : ConfigurationDesignHostTestBase
    {
        private AddApplicationConfigurationNodeCommand addApplicationConfigurationNodeCommand;
        private bool hierarchyAdded;
        private ConfigurationNode nodeAdded;

        public override void SetUp()
        {
            base.SetUp();
            MySolutionNode node = new MySolutionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            addApplicationConfigurationNodeCommand = new AddApplicationConfigurationNodeCommand(Host);
            base.HierarchyService.HierarchyAdded += new HierarchyAddedEventHandler(OnHierarchyAdded);
        }

        public override void TearDown()
        {
            hierarchyAdded = false;
            addApplicationConfigurationNodeCommand.Dispose();
            base.TearDown();
        }

        [Test]
        public void ExecuteTest()
        {
            addApplicationConfigurationNodeCommand.Execute(null);
            Assert.IsTrue(hierarchyAdded);
            Assert.AreEqual(typeof(ApplicationConfigurationNode), nodeAdded.GetType());
        }

        private class MySolutionNode : ConfigurationNode
        {
        }

        private void OnHierarchyAdded(object sender, HierarchyAddedEventArgs args)
        {
            hierarchyAdded = true;
            nodeAdded = args.UIHierarchy.RootNode;
        }
    }
}

#endif