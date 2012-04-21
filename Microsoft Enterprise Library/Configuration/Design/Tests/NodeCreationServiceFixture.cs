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
using System;
using System.Collections.Specialized;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture] 
    public class NodeCreationServiceFixture : ConfigurationDesignHostTestBase
    {

        [Test]
        public void CreateNodeTest()
        {
            Type t = typeof(XmlFileStorageProviderNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host,t ), t, typeof(XmlFileStorageProviderData), SR.XmlFileStorageProviderNodeTypeNameDescription);
            NodeCreationService.AddNodeCreationEntry(entry);

            t = typeof(XmlSerializerTransformerNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host,t ), t, typeof(XmlSerializerTransformerData), SR.XmlSerializerTransformerNodeFriendlyName);
            NodeCreationService.AddNodeCreationEntry(entry);
            
            XmlFileStorageProviderNode node = NodeCreationService.CreateNode(typeof(XmlFileStorageProviderData)) as XmlFileStorageProviderNode;
            Assert.IsNotNull(node);
        }

        [Test]
        public void CanGetANodeWithNotADirectInheritanceChain()
        {
            string name = "Foo";

            Type nodeType = typeof(MyDerivedNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(MyDerivedData), name);
            NodeCreationService.AddNodeCreationEntry(entry);
            
            StringCollection names = NodeCreationService.GetDisplayNames(typeof(StorageProviderNode));
            Assert.IsTrue(names.Contains(name));
        }
    }
}

#endif