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
    public class UIHiearchyServiceFixture
    {
        private bool addEventCalled;
        private int addEventCount;
        private bool removedEventCalled;
        private int removedEventCount;
        private IUIHierarchy eventHierarchy;

        [TearDown]
        public void TearDown()
        {
            addEventCalled = false;
            addEventCount = 0;
            removedEventCalled = false;
            removedEventCount = 0;
            eventHierarchy = null;
        }

        [Test]
        public void AddHierarchyAndEventFiredTest()
        {
            ConfigurationDesignHost host = new ConfigurationDesignHost();
            UIHierarchyService hierarchyService = host.GetService(typeof(IUIHierarchyService)) as UIHierarchyService;
            hierarchyService.HierarchyAdded += new HierarchyAddedEventHandler(OnHierarchyAdded);
            Assert.IsNotNull(hierarchyService);
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            IUIHierarchy hierarchy = new UIHierarchy(appNode, host, new ConfigurationContext(appNode.ConfigurationFile));
            hierarchyService.AddHierarchy(hierarchy);
            Assert.IsTrue(addEventCalled);
            Assert.AreEqual(1, addEventCount);
            Assert.AreSame(hierarchy, eventHierarchy);
            ConfigurationSectionCollectionNode node = new ConfigurationSectionCollectionNode();
            appNode.Nodes.Add(node);
            node.Nodes.Add(new ConfigurationSectionNode());
            Assert.AreEqual(1, hierarchyService.hierarchies.Count);
        }

        [Test]
        public void CanFindHierarchyTest()
        {
            ConfigurationDesignHost host = new ConfigurationDesignHost();
            UIHierarchyService hierarchyService = host.GetService(typeof(IUIHierarchyService)) as UIHierarchyService;
            Assert.IsNotNull(hierarchyService);
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            UIHierarchy hierarchy = new UIHierarchy(appNode, host, new ConfigurationContext(appNode.ConfigurationFile));
            ConfigurationSectionCollectionNode node = new ConfigurationSectionCollectionNode();
            appNode.Nodes.Add(node);
            node.Nodes.Add(new ConfigurationSectionNode());
            hierarchyService.AddHierarchy(hierarchy);
            IUIHierarchy foundHierarchy = hierarchyService.GetHierarchy(appNode.Id);
            Assert.AreSame(hierarchy, foundHierarchy);
        }

        [Test]
        public void RemoveHierarchyAndEventFiredTest()
        {
            ConfigurationDesignHost host = new ConfigurationDesignHost();
            UIHierarchyService hierarchyService = host.GetService(typeof(IUIHierarchyService)) as UIHierarchyService;
            hierarchyService.HierarchyRemoved += new HierarchyRemovedEventHandler(OnHierarchyRemoved);
            Assert.IsNotNull(hierarchyService);
            ApplicationConfigurationNode appNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            IUIHierarchy hierarchy = new UIHierarchy(appNode, host, new ConfigurationContext(appNode.ConfigurationFile));
            hierarchyService.AddHierarchy(hierarchy);
            ConfigurationSectionCollectionNode node = new ConfigurationSectionCollectionNode();
            appNode.Nodes.Add(node);
            node.Nodes.Add(new ConfigurationSectionNode());
            hierarchyService.RemoveHierarchy(appNode.Id);
            IUIHierarchy foundHierarchy = hierarchyService.GetHierarchy(appNode.Id);
            Assert.IsTrue(removedEventCalled);
            Assert.AreEqual(1, removedEventCount);
            Assert.AreSame(hierarchy, eventHierarchy);
            Assert.IsNull(foundHierarchy);
        }

        private void OnHierarchyAdded(object sender, HierarchyAddedEventArgs args)
        {
            addEventCalled = true;
            addEventCount++;
            eventHierarchy = args.UIHierarchy;
        }

        private void OnHierarchyRemoved(object sender, HierarchyRemovedEventArgs args)
        {
            removedEventCalled = true;
            removedEventCount++;
            eventHierarchy = args.UIHierarchy;
        }
    }

}

#endif