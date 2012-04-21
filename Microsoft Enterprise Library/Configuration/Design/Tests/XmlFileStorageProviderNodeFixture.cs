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
    public class XmlFileStorageProviderNodeFixture : ConfigurationDesignHostTestBase
    {
        private const string ProviderName = "XmlFileStorageProviderNode";
//        private static readonly string MyFile = "c:\\mytestfile.xml";
        private XmlFileStorageProviderNode node;
        private XmlFileStorageProviderData data;

        public override void SetUp()
        {
            base.SetUp();
            data = new XmlFileStorageProviderData(ProviderName);
            node = new XmlFileStorageProviderNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(data.Name, node.Name);
        }

        [Test]
        public void TypeNameTest()
        {
            Assert.AreEqual(data.TypeName, node.TypeName);
        }

        [Test]
        public void StorageProviderDataTest()
        {
            Assert.AreSame(data, node.StorageProvider);
        }

//        [Test]
//        public void SaveTest()
//        {
//            node.FileName = MyFile;
//            Assert.AreEqual(MyFile, node.FileName);
//            ConfigurationErrorCollection errors = new ConfigurationErrorCollection();
//            node.Save(errors);
//            Assert.AreEqual(0, errors.Count);
//            Assert.IsTrue(File.Exists(node.FileName));
//            FileStream file = File.OpenRead(node.FileName);
//            StreamReader reader = new StreamReader(file);
//            string line = reader.ReadToEnd();
//            Assert.AreEqual(SR.FileWriteMessage, line);
//            file.Close();
//            File.Delete(node.FileName);
//        }        
    }
}

#endif