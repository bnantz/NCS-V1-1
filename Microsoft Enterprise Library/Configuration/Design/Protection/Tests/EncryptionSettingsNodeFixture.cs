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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection.Tests
{
    [TestFixture]
    public class EncryptionSettingsNodeFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void GetNullData()
        {
            EncryptionSettingsNode node = new EncryptionSettingsNode();
            Assert.IsNull(node.KeyAlgorithmPairStorageProviderData);
        }

        [Test]
        public void GetFileKeyAlgorithmData()
        {
            EncryptionSettingsNode node = new EncryptionSettingsNode();
            INodeCreationService nodeCreationService = GetService(typeof(INodeCreationService)) as INodeCreationService;
            Assert.IsNotNull(nodeCreationService);
            
            Type nodeType = typeof(FileKeyAlgorithmPairStorageProviderNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddFileKeyAlgorithmPairNodeCommand(Host, nodeType), nodeType, typeof(FileKeyAlgorithmPairStorageProviderData), SR.FileKeyAlgorithmStorageProviderNodeFriendlyName);
            nodeCreationService.AddNodeCreationEntry(entry);

            GeneratedApplicationNode.Nodes.Add(node);
            FileKeyAlgorithmPairStorageProviderNode pairStorageNode = new FileKeyAlgorithmPairStorageProviderNode();
            pairStorageNode.File = "testeithbeuhyr";
            node.Nodes.Add(pairStorageNode);
            FileKeyAlgorithmPairStorageProviderData storageData = (FileKeyAlgorithmPairStorageProviderData)node.KeyAlgorithmPairStorageProviderData;
            Assert.AreEqual(pairStorageNode.File, storageData.Path);
        }
    }
}

#endif