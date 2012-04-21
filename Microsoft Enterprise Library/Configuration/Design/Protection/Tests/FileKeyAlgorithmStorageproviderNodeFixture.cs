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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection.Tests
{
    [TestFixture]
    public class FileKeyAlgorithmStorageproviderNodeFixture
    {
        [Test]
        public void NodeTest()
        {
            DpapiSettings settings = new DpapiSettings(null);
            settings.Entropy = new byte[16];

            string file = "etije98ts";

            FileKeyAlgorithmPairStorageProviderNode node = new FileKeyAlgorithmPairStorageProviderNode();
            node.DpapiSettings = settings;
            node.File = file;

            Assert.AreEqual(settings.Entropy.Length, node.DpapiSettings.Entropy.Length);
            Assert.AreEqual(file, node.File);
        }

        [Test]
        public void DataTest()
        {
            FileKeyAlgorithmPairStorageProviderData data = new FileKeyAlgorithmPairStorageProviderData();
            data.Path = "ret8ye587nygr";
            data.Name = "estojerte";

            FileKeyAlgorithmPairStorageProviderNode node = new FileKeyAlgorithmPairStorageProviderNode(data);
            FileKeyAlgorithmPairStorageProviderData nodeData = (FileKeyAlgorithmPairStorageProviderData)node.KeyAlgorithmStorageProviderData;

            Assert.AreEqual(data.Path, nodeData.Path);
            Assert.AreEqual(data.Name, nodeData.Name);
        }
    }
}

#endif