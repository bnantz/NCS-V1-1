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

#if    UNIT_TESTS
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Storage.Tests
{
    [TestFixture]
    public class XmlFileStorageProviderFixture
    {
        private static string storageTypeName = typeof(XmlFileStorageProvider).AssemblyQualifiedName;
        private static string transformerName = typeof(XmlSerializerTransformerData).AssemblyQualifiedName;
        private static readonly string fileStorage = typeof(FileKeyAlgorithmPairStorageProvider).AssemblyQualifiedName;
        private string configFile;
        private ConfigurationContext context;
        private int eventFiredCount;
        private static readonly string applConfig1 = "ApplConfig1";
        private static readonly string testConfigFile = "mytestconfig.xml";
        private static readonly string noSectionTestConfig = "mynosecitontestconfig.xml";
        private static readonly string badPathConfig = @"c:\\mybadpath.xml";

        private static readonly string XmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"" + applConfig1 + "\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"" + testConfigFile + "\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string XmlStringWithEncryption =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"" + applConfig1 + "\" encrypt=\"true\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"" + testConfigFile + "\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "<keyAlgorithmStorageProvider xsi:type=\"FileKeyAlgorithmPairStorageProviderData\" name=\"FileStore\" type=\"" + fileStorage + "\" path=\"foo.data\">" +
                "</keyAlgorithmStorageProvider>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string XmlStringWithEncryptionAndDpapi =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"" + applConfig1 + "\" encrypt=\"true\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"" + testConfigFile + "\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "<keyAlgorithmStorageProvider xsi:type=\"FileKeyAlgorithmPairStorageProviderData\" name=\"FileStore\" type=\"" + fileStorage + "\" path=\"foo.data\">" +
                "<dpapiSettings mode=\"Machine\" entropy=\"AAECAwQFBgcICQABAgMFBg==\" />" +
                "</keyAlgorithmStorageProvider>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string NoSectionXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"" + applConfig1 + "\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"" + noSectionTestConfig + "\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string XmlBadFileString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"" + applConfig1 + "\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"" + badPathConfig + "\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string XmlNoFileString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"" + applConfig1 + "\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string XmlBadFilePathString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"" + applConfig1 + "\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" " +
                "path=\"" + badPathConfig + "\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string NoPathXmlString =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<enterpriselibrary.configurationSettings xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/configuration\" >" +
                "<configurationSections>" +
                "<configurationSection name=\"" + applConfig1 + "\">" +
                "<storageProvider " +
                "xsi:type=\"XmlFileStorageProviderData\" " +
                "name=\"XmlStorage\" " +
                "type=\"" + storageTypeName + "\" />" +
                "<dataTransformer " +
                "xsi:type=\"XmlSerializerTransformerData\" " +
                "name=\"MockConfig\" " +
                "type=\"" + transformerName + "\" />" +
                "</configurationSection>" +
                "</configurationSections>" +
                "</enterpriselibrary.configurationSettings>";

        private static readonly string xmlData =
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                "<Data>" +
                "<val>Some foobar stuff</val>" +
                "</Data>";

        [TestFixtureSetUp]
        public void Initialize()
        {
            configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            context = new ConfigurationContext(new DisposingWrapper(new ConfigurationBuilder(configFile)));
            ConfigurationChangeFileWatcher.SetDefaultPollDelayInMilliseconds(100);
        }

        [TestFixtureTearDown]
        public void Reset()
        {
            context.Dispose();
            ConfigurationChangeFileWatcher.ResetDefaultPollDelay();
        }

        [Test]
        public void StorageTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderReader storage = factory.Create(applConfig1) as IStorageProviderReader;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                ((IStorageProviderWriter)storage).Write(GetData());
                XmlNode data = storage.Read() as XmlNode;
                Assert.AreEqual(GetData().OuterXml, data.OuterXml);
            }
        }

        [Test]
        public void StorageFiresEventTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderReader storage = factory.Create(applConfig1) as IStorageProviderReader;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));

                using (IConfigurationChangeWatcher watcher = storage.CreateConfigurationChangeWatcher())
                {
                    watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);

                    watcher.StartWatching();
                    Thread.Sleep(100);
                    ((IStorageProviderWriter)storage).Write(GetData());

                    for (int wait = 0; wait < 10 && eventFiredCount < 2; ++wait)
                    {
                        Thread.Sleep(500);
                    }

                    watcher.Dispose();
                }

                Assert.AreEqual(1, eventFiredCount);
            }
        }

        [Test]
        public void StorageTestWithEncryption()
        {
            SaveKeyAlgorithmPair(XmlStringWithEncryption);
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlStringWithEncryption))
            {
				using( FileStream s = new FileStream(Path.GetFullPath(testConfigFile), FileMode.Create) )
				{
					for (int i=0; i<1000; i++)
					{
						byte b = 0;
						s.WriteByte(b);
					}
				}

                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderReader storage = factory.Create(applConfig1) as IStorageProviderReader;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                ((IStorageProviderWriter)storage).Write(GetData());
                XmlNode data = storage.Read() as XmlNode;
                Assert.AreEqual(GetData().OuterXml, data.OuterXml);
            }
        }

        private void SaveKeyAlgorithmPair(string xmlToUse)
        {
            using (ConfigurationContext context = CreateConfigurationContext(xmlToUse))
            {
                FileKeyAlgorithmPairStorageProvider provider = new FileKeyAlgorithmPairStorageProvider();
                provider.Initialize(new RuntimeConfigurationView(context));
                provider.Save(DefaultKey);
            }
        }

        private KeyAlgorithmPair DefaultKey
        {
            get
            {
                KeyAlgorithmPair key = new KeyAlgorithmPair();
                key.AlgorithmTypeName = typeof(RijndaelManaged).AssemblyQualifiedName;
                key.Key = new SymmetricAlgorithmKeyCreator(key.AlgorithmTypeName).GenerateKey();// new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6};
                return key;
            }
        }

        [Test]
        public void StorageTestWithEncryptionAndDpapi()
        {
            SaveKeyAlgorithmPair(XmlStringWithEncryptionAndDpapi);
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlStringWithEncryptionAndDpapi))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderReader storage = factory.Create(applConfig1) as IStorageProviderReader;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                ((IStorageProviderWriter)storage).Write(GetData());
                XmlNode data = storage.Read() as XmlNode;
                Assert.AreEqual(GetData().OuterXml, data.OuterXml);
            }
        }

        [Test]
        public void StorageTestChangeEncryption()
        {
            SaveKeyAlgorithmPair(XmlStringWithEncryption);
            
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlStringWithEncryption))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderReader storage = factory.Create(applConfig1) as IStorageProviderReader;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                ((IStorageProviderWriter)storage).Write(GetData());
                
				configurationContext.GetMetaConfiguration().ConfigurationSections[0].Encrypt = false;
                factory = new StorageProviderFactory(configurationContext);
                IStorageProviderWriter storageWriter = factory.Create(applConfig1) as IStorageProviderWriter;
                storageWriter.Write(GetData());
                
				XmlNode data = storageWriter.Read() as XmlNode;
                Assert.AreEqual(GetData().OuterXml, data.OuterXml);
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoSectionStorageTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(NoSectionXmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderReader storage = factory.Create(applConfig1) as IStorageProviderReader;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                storage.Read();
                Assert.Fail("Should never get here since the section is not specified in the external configuration file.");
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoFileStorageTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlNoFileString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderWriter storage = factory.Create(applConfig1) as IStorageProviderWriter;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                storage.Write(GetData());
                Assert.Fail("Should never get here since the file is not specified in configuration.");
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void BadFilePathStorageTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlBadFilePathString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderWriter storage = factory.Create(applConfig1) as IStorageProviderWriter;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                storage.Write(GetData());
                Assert.Fail("Should never get here since the file is not specified in configuration.");
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void StorageBadFileTest()
        {
            using (ConfigurationContext configurationContext  = CreateConfigurationContext(XmlBadFileString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderWriter storage = factory.Create(applConfig1) as IStorageProviderWriter;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                storage.Write(GetData());
            }
        }

        [Test]
        public void SectionNameTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderWriter storage = factory.Create(applConfig1) as IStorageProviderWriter;
                Assert.AreEqual(applConfig1, ((XmlFileStorageProvider)storage).CurrentSectionName);
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoPathReadWriteTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(NoPathXmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                factory.Create(applConfig1);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteNullValueTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderWriter storage = factory.Create(applConfig1) as IStorageProviderWriter;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                storage.Write(null);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteNonXmlNodeValueTest()
        {
            using (ConfigurationContext configurationContext = CreateConfigurationContext(XmlString))
            {
                StorageProviderFactory factory = new StorageProviderFactory(configurationContext);
                IStorageProviderWriter storage = factory.Create(applConfig1) as IStorageProviderWriter;
                Assert.AreSame(storage.GetType(), typeof(XmlFileStorageProvider));
                storage.Write(new object());
            }
        }

        [Test]
        public void InitializeTest()
        {
            ConfigurationSettings settings = GetConfigurationSettings(XmlString);
            ConfigurationSectionData sectionData = settings.ConfigurationSections[applConfig1];
            XmlFileStorageProviderData providerData = (XmlFileStorageProviderData)sectionData.StorageProvider;
            providerData.SectionName = applConfig1;
            XmlFileStorageProvider provider = new XmlFileStorageProvider();
            provider.CurrentSectionName = applConfig1;
        	RuntimeConfigurationView configurationView = new RuntimeConfigurationView(context);
            provider.Initialize(configurationView);
        }

        private XmlNode GetData()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);
            return doc.DocumentElement;
        }

        private ConfigurationContext CreateConfigurationContext(string xml)
        {
            ConfigurationSettings settings = GetConfigurationSettings(xml);
            ConfigurationBuilder builder = new ConfigurationBuilder(settings);
            return new ConfigurationContext(new DisposingWrapper(builder));
        }

        private ConfigurationSettings GetConfigurationSettings(string xml)
        {
            try
            {
                XmlTextReader xmlReader = new XmlTextReader(new StringReader(xml));
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSettings));
                ConfigurationSettings settings = xmlSerializer.Deserialize(xmlReader) as ConfigurationSettings;
                return settings;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            Assert.AreEqual(testConfigFile, Path.GetFileName(args.ConfigurationFile));
            Assert.AreEqual(applConfig1, args.SectionName);
            ++eventFiredCount;
        }
    }
}

#endif