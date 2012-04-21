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
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer;
using NUnit.Framework;
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationBuilderFixture
    {
        private ConfigurationSettings configurationSettings;
        private MockConfigurationData data;
        private string configFile;
        private ConfigurationBuilder builder;
        private bool eventFired;

        private static readonly string xmlSerializerTransformer = "XmlSerializerTransformer";
        private static readonly string xmlFileStorageProvider = "XmlFileStorageProvider";
        private static readonly string mockConfigSection = "MockConfig";
        private static readonly string sectionAName = "SectionA";
        private static readonly string fooName = "Foo";
        private static readonly string fooConfig = "foo.config";
        private static readonly string noStorageConfig = "NoStorageConfig";
        private static readonly string myBadSection = "MyBadSection";
        private static readonly string noTransformerSection = "NoTransformer";
        private static readonly string xmlSerializerSection = "xmlSerializerSection";
        private static readonly string myConfigTestFile = @"MyConfigTest.xml";
        private static readonly string myConfigFile = "myconfig.xml";
        private static readonly string webConfigFile = "WebConfigTest.xml";
        private object changedNewValue;
        private object changedOldValue;
        private string changedSectionName;
        private string changedConfigurationFile;
        private int eventCount;
        private string eventString = "";

        [TestFixtureSetUp]
        public void Initialize()
        {
            ConfigurationChangeFileWatcher.SetDefaultPollDelayInMilliseconds(1000);

            configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            configurationSettings = new ConfigurationSettings();
            ConfigurationSectionData sectionA = new ConfigurationSectionData(sectionAName);
            configurationSettings.ConfigurationSections.Add(sectionA);

            sectionA.Transformer = new XmlSerializerTransformerData(sectionAName);
            sectionA.Transformer.TypeName = typeof(MockTransformer).AssemblyQualifiedName;

            XmlFileStorageProviderData storageProviderData = new XmlFileStorageProviderData(fooName, fooConfig);
            sectionA.StorageProvider = storageProviderData;

            ConfigurationSectionData sectionB = new ConfigurationSectionData("SectionB");
            configurationSettings.ConfigurationSections.Add(sectionB);

            sectionB.Transformer = new XmlSerializerTransformerData(sectionAName);
            sectionB.Transformer.TypeName = typeof(XmlSerializerTransformer).AssemblyQualifiedName;

            XmlFileStorageProviderData fooStorageProviderData = new XmlFileStorageProviderData(fooName, fooConfig);
            sectionB.StorageProvider = fooStorageProviderData;
        }

        [SetUp]
        public void SetUp()
        {
            builder = new ConfigurationBuilder(configFile);

            data = new MockConfigurationData();
            data.Color = "Red";
            data.Size = 5;
            data.SomeText = "Some Text";
            eventFired = false;
            eventString = "";
            eventCount = 0;
            changedNewValue = null;
            changedOldValue = null;
            changedSectionName = string.Empty;
            changedConfigurationFile = string.Empty;
        }

        [TearDown]
        public void TearDown()
        {
            builder.Dispose();
        }

        [TestFixtureTearDown]
        public void ResetStuff()
        {
            ConfigurationChangeFileWatcher.ResetDefaultPollDelay();
        }

        [Test]
        public void ReadWriteWebConfigTest()
        {
            string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, webConfigFile);
            using (ConfigurationBuilder builder = new ConfigurationBuilder(configFile))
            {
                ConfigurationSettings configData = new ConfigurationSettings(mockConfigSection);
                configData.ConfigurationSections.Add(GetConfigurationSection());
                builder.WriteMetaConfiguration(configData);
                builder.WriteConfiguration(mockConfigSection, data);
                MockConfigurationData readData = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.IsNotNull(readData);
                Assert.AreEqual(data.ToString(), readData.ToString());
            }
        }

        [Test]
        public void ReadWriteTest()
        {
            builder.WriteConfiguration(mockConfigSection, data);
            MockConfigurationData readData = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
            Assert.IsNotNull(readData);
            Assert.AreEqual(data.ToString(), readData.ToString());
        }

        [Test]
        public void ReadWriteNoTransformerTest()
        {
            string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            using (ConfigurationBuilder builder = new ConfigurationBuilder(configFile))
            {
                XmlNode writeData = SerializeData(data);
                builder.WriteConfiguration(noTransformerSection, writeData);
                XmlNode readData = builder.ReadConfiguration(noTransformerSection) as XmlNode;
                Assert.IsNotNull(readData);
                Assert.AreEqual(writeData.OuterXml, readData.OuterXml);
            }
        }

        private XmlNode SerializeData(MockConfigurationData data)
        {
            XmlSerializer xs = new XmlSerializer(data.GetType());
            StringWriter sw = new StringWriter(CultureInfo.CurrentUICulture);
            XmlTextWriter xmlTw = new XmlTextWriter(sw);
            try
            {
                xmlTw.WriteStartElement(xmlSerializerSection);
                xmlTw.WriteAttributeString("type", data.GetType().AssemblyQualifiedName);
                xs.Serialize(xmlTw, data);
                xmlTw.WriteEndElement();
                xmlTw.Flush();
            }
            finally
            {
                xmlTw.Close();
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(sw.ToString());
            }
            finally
            {
                sw.Close();
            }
            return doc.DocumentElement;
        }

        [Test]
        public void SpaceInPathConfigWriteTest()
        {
            builder.WriteConfiguration(mockConfigSection, data);
            MockConfigurationData readData = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
            Assert.IsNotNull(readData);
            Assert.AreEqual(data.ToString(), readData.ToString());
        }

        [Test]
        public void CasesDoNotNeedToMatchToRetrieveConfigSection()
        {
            builder.WriteConfiguration(mockConfigSection, data);
            Assert.IsNotNull(builder.ReadConfiguration(mockConfigSection.ToLower()));
        }

        [Test]
        public void IsReadOnlyTest()
        {
            Assert.IsFalse(builder.IsReadOnly(mockConfigSection));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsReadOnlyNullSectionTest()
        {
            Assert.IsFalse(builder.IsReadOnly(null));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ReadOnlyBadSection()
        {
            Assert.IsFalse(builder.IsReadOnly(fooName));
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoSectionNameReadTest()
        {
            ConfigurationManager.GetConfiguration(string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoSectionNameWriteTest()
        {
            builder.WriteConfiguration(string.Empty, null);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoStorageTest()
        {
            ConfigurationManager.GetConfiguration(noStorageConfig);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ReadBadSectionTest()
        {
            builder.ReadConfiguration(myBadSection);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void WriteBadSectionTest()
        {
            builder.WriteConfiguration(myBadSection, new Object());
        }

        [Test]
        public void WriteMetaConfigTest()
        {
            string fileName = Path.GetFullPath(@"MyWriteConfigTest.xml");
            XmlDocument beforeChange = new XmlDocument();
            beforeChange.Load(fileName);
            XmlDocument change = new XmlDocument();
            using (ConfigurationBuilder builder2 = new ConfigurationBuilder(fileName))
            {
                builder2.WriteMetaConfiguration(configurationSettings);
            }
            change.Load(fileName);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(change.NameTable);
            nsmgr.AddNamespace("entlib", ConfigurationSettings.ConfigurationNamespace);
            XmlNode lookForNode = change.DocumentElement.SelectSingleNode("//configuration/entlib:enterpriselibrary.configurationSettings/entlib:configurationSections/entlib:configurationSection[@name='SectionA']", nsmgr);
            Assert.IsNotNull(lookForNode);
        }

        [Test]
        public void WriteMetaConfigEnsureEventsGetRaisedTest()
        {
            string fileName = Path.GetFullPath(@"MyWriteConfigTest.xml");
            XmlDocument beforeChange = new XmlDocument();
            beforeChange.Load(fileName);
            using (ConfigurationBuilder builder2 = new ConfigurationBuilder(fileName))
            {
                builder2.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);

                builder2.WriteMetaConfiguration(configurationSettings);
                Thread.Sleep(500);

                builder2.ConfigurationChanged -= new ConfigurationChangedEventHandler(OnConfigurationChanged);
            }

            Assert.IsTrue(eventFired);
        }      
        

        [Test]
        public void WriteMetaConfigSectionOnlyTest()
        {
            string fileName = Path.GetFullPath(@"MyWriteConfigTest.xml");
            XmlDocument beforeChange = new XmlDocument();
            beforeChange.Load(fileName);
            XmlDocument change = new XmlDocument();
            using (ConfigurationBuilder builder2 = new ConfigurationBuilder(fileName))
            {
                builder2.WriteMetaConfig(CreateNewConfigurationSection());
            }
            change.Load(fileName);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(change.NameTable);
            nsmgr.AddNamespace("entlib", ConfigurationSettings.ConfigurationNamespace);
            XmlNode lookForNode = change.DocumentElement.SelectSingleNode("//configuration/entlib:enterpriselibrary.configurationSettings/entlib:configurationSections/entlib:configurationSection[@name='config']", nsmgr);
            Assert.IsNotNull(lookForNode);
        }

        [Test]
        public void ReadMetaConfigTest()
        {
            string filename = Path.GetFullPath(myConfigTestFile);
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            using (ConfigurationBuilder builder = new ConfigurationBuilder(filename))
            {
                ConfigurationSettings configurationSettings = builder.ReadMetaConfiguration();

                Assert.IsNotNull(configurationSettings);
                Assert.AreEqual(4, configurationSettings.ConfigurationSections.Count);
            }
        }

        [Test]
        public void ReadMetaConfigSectionTest()
        {
            string filename = Path.GetFullPath(myConfigTestFile);
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            using (ConfigurationBuilder builder = new ConfigurationBuilder(filename))
            {
                ConfigurationSectionData data = builder.ReadMetaConfiguration("ApplConfig1");
                Assert.IsNotNull(data);
            }
        }

        [Test]
        public void CreateWithConfigurationDataTest()
        {
            ConfigurationSettings data = CreateConfigurationSettings();
            using (ConfigurationBuilder builder = new ConfigurationBuilder(data))
            {
                Assert.AreSame(data, builder.ConfigurationSettings);
            }
        }

        [Test]
        public void InMemoryConfigurationManagerTest()
        {
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            object configurationData = new object();
            dictionary.Add("mySection", configurationData);
            using (ConfigurationBuilder builder = new ConfigurationBuilder(dictionary))
            {
                object actual = builder.ReadConfiguration("mySection");
                Assert.AreSame(configurationData, actual);
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void InMemoryInvalidSectionTest()
        {
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            using (ConfigurationBuilder builder = new ConfigurationBuilder(dictionary))
            {
                builder.ReadConfiguration("invalidSection");
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void NoConfigFileTest()
        {
            using (ConfigurationBuilder builder = new ConfigurationBuilder("c:\\mynonexistentfile.config"))
            {
                builder.ReadConfiguration("config");
            }
        }

        [Test]
        public void ClearCacheByNameTest()
        {
            using (ConfigurationBuilder builder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                builder.WriteConfiguration(mockConfigSection, data);
                MockConfigurationData readData = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
                MockConfigurationData readData2 = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.AreSame(readData, readData2);
                builder.ClearSectionCache(mockConfigSection);
                readData2 = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.IsFalse(Object.Equals(readData, readData2));
            }
        }

        [Test]
        public void ClearCacheTest()
        {
            using (ConfigurationBuilder builder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                builder.WriteConfiguration(mockConfigSection, data);
                MockConfigurationData readData = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
                MockConfigurationData readData2 = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.AreSame(readData, readData2);
                builder.ClearSectionCache();
                readData2 = builder.ReadConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.IsFalse(Object.Equals(readData, readData2));
            }
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void WritingToEmptyConfigFileWithEncryptionOn()
        {
            using (ConfigurationBuilder builder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                builder.WriteConfiguration("EncryptedProblem", data);
            }

        }

        [Test]
        public void ClearingSectionFromCacheShouldPreventAnyChangeEventsForThatSection()
        {
            using (ConfigurationBuilder builder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                builder.WriteConfiguration(mockConfigSection, data);
                builder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnMockConfigConfigurationChanged);

                builder.ClearSectionCache(mockConfigSection);
            }

            FileUtility.OpenAndChangeFile(Path.GetFullPath(myConfigFile));

            Thread.Sleep(500);
            Assert.IsFalse(eventFired);
        }

        [Test]
        public void ClearingEntireSectionCachePreventsAnyChangeEventsForThatSection()
        {
            using (ConfigurationBuilder builder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                builder.WriteConfiguration(mockConfigSection, data);
                builder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnMockConfigConfigurationChanged);

                builder.ClearSectionCache();
            }

            FileUtility.OpenAndChangeFile(Path.GetFullPath(myConfigFile));

            Thread.Sleep(500);
            Assert.IsFalse(eventFired);
        }

        [Test]
        public void StaticClearCacheByNameTest()
        {
            using (ConfigurationBuilder builder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                builder.WriteConfiguration(mockConfigSection, data);
                MockConfigurationData readData = ConfigurationManager.GetConfiguration(mockConfigSection) as MockConfigurationData;
                MockConfigurationData readData2 = ConfigurationManager.GetConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.AreSame(readData, readData2);
                ConfigurationManager.ClearSingletonSectionCache(mockConfigSection);
                readData2 = ConfigurationManager.GetConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.IsFalse(Object.Equals(readData, readData2));
            }
        }

        [Test]
        public void StaticClearCacheTest()
        {
            using (ConfigurationBuilder builder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                builder.WriteConfiguration(mockConfigSection, data);
                MockConfigurationData readData = ConfigurationManager.GetConfiguration(mockConfigSection) as MockConfigurationData;
                MockConfigurationData readData2 = ConfigurationManager.GetConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.AreSame(readData, readData2);
                ConfigurationManager.ClearSingletonSectionCache();
                readData2 = ConfigurationManager.GetConfiguration(mockConfigSection) as MockConfigurationData;
                Assert.IsFalse(Object.Equals(readData, readData2));
            }
        }

        [Test]
        [ExpectedException(typeof(SecurityException))]
        public void OpenMachineConfigWithoutPermissions()
        {
            FileIOPermission denyPermission = new FileIOPermission(FileIOPermissionAccess.Read, ConfigurationBuilder.MachineConfigurationFile);
            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();
            using (new ConfigurationBuilder(ConfigurationBuilder.MachineConfigurationFile))
            {
            }
        }

        [Test]
        public void MakeSureTheChangingEventFires()
        {
            builder.WriteConfiguration(mockConfigSection, data);
            using (ConfigurationBuilder newBuilder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                newBuilder.ConfigurationChanging += new ConfigurationChangingEventHandler(OnConfigurationChanging);
                newBuilder.WriteConfiguration(mockConfigSection, data);
                for (int wait = 0; !eventFired && (wait < 10); ++wait)
                {
                    Thread.Sleep(500);
                }
                newBuilder.ConfigurationChanging -= new ConfigurationChangingEventHandler(OnConfigurationChanging);
            }
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, changedConfigurationFile);
            Assert.AreEqual(mockConfigSection, changedSectionName);
            Assert.IsNotNull(changedOldValue);
            Assert.IsNotNull(changedNewValue);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void MakeSureTheChangedEventFires()
        {
            builder.WriteConfiguration(mockConfigSection, data);
            using (ConfigurationBuilder newBuilder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                newBuilder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);
                newBuilder.WriteConfiguration(mockConfigSection, data);
            }
            for (int wait = 0; (wait < 10); ++wait)
            {
                Thread.Sleep(500);
            }
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, changedConfigurationFile);
            Assert.AreEqual(mockConfigSection, changedSectionName);
            Assert.IsTrue(eventFired);
            Assert.AreEqual(1, eventCount);
        }

        [Test]
        public void MakeSureTheStaticChangingEventFires()
        {
            builder.WriteConfiguration(mockConfigSection, data);
            ConfigurationManager.ConfigurationChanging += new ConfigurationChangingEventHandler(OnConfigurationChanging);
            ConfigurationManager.WriteConfiguration(mockConfigSection, data);
            for (int wait = 0; (!eventFired) && (wait < 10); ++wait)
            {
                Thread.Sleep(500);
            }
            ConfigurationManager.ConfigurationChanging -= new ConfigurationChangingEventHandler(OnConfigurationChanging);
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, changedConfigurationFile);
            Assert.AreEqual(mockConfigSection, changedSectionName);
            Assert.IsNotNull(changedOldValue);
            Assert.IsNotNull(changedNewValue);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void MakeSureTheStaticChangedEventFires()
        {
            builder.WriteConfiguration(mockConfigSection, data);
            ConfigurationManager.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);
            ConfigurationManager.WriteConfiguration(mockConfigSection, data);
            for (int wait = 0; (!eventFired) && (wait < 10); ++wait)
            {
                Thread.Sleep(500);
            }
            ConfigurationManager.ConfigurationChanged -= new ConfigurationChangedEventHandler(OnConfigurationChanged);
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, changedConfigurationFile);
            Assert.AreEqual(mockConfigSection, changedSectionName);
            Assert.IsTrue(eventFired);
        }

        [Test]
        public void MakeSureTheChangedEventFiresWhenTheMetaDataFileChangesExternally()
        {
            //myconfig.xml
            builder.WriteConfiguration(mockConfigSection, data);
            using (ConfigurationBuilder newBuilder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                newBuilder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnMetaDataConfigurationChanged);
                Thread.Sleep(150);
                FileUtility.OpenAndChangeFile(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                for (int wait = 0; (!eventFired) && (wait < 10); ++wait)
                {
                    Thread.Sleep(500);
                }
                newBuilder.ConfigurationChanged -= new ConfigurationChangedEventHandler(OnMetaDataConfigurationChanged);
            }
            Assert.IsTrue(eventFired);
            Assert.AreEqual(ConfigurationSettings.SectionName, changedSectionName);
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToLower(), changedConfigurationFile.ToLower());
        }

        [Test]
        public void MakeSureTheChangedEventFiresWhenTheDataFileChangesExternally()
        {
            using (ConfigurationBuilder newBuilder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                newBuilder.WriteConfiguration(mockConfigSection, data);
                newBuilder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnDataConfigurationChanged);
                Thread.Sleep(100);
                FileUtility.OpenAndChangeFile(Path.GetFullPath(myConfigFile));
                for (int wait = 0; (!eventFired) && (wait < 10); ++wait)
                {
                    Thread.Sleep(500);
                }
                newBuilder.ConfigurationChanged -= new ConfigurationChangedEventHandler(OnDataConfigurationChanged);

            }
            Assert.AreEqual(mockConfigSection, changedSectionName);
            Assert.AreEqual(myConfigFile, changedConfigurationFile);
            Assert.IsTrue(eventFired);

        }

        [Test]
        public void ShouldNotGetConfigurationChangedEventIfChangeIsCanceled()
        {
            using (ConfigurationBuilder newBuilder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                newBuilder.ConfigurationChanging += new ConfigurationChangingEventHandler(CancelingHandler);
                newBuilder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);
                newBuilder.WriteMetaConfig(CreateNewConfigurationSection());
                Thread.Sleep(250);
            }
            Assert.AreEqual("ChangeCanceled", eventString);
        }

        [Test]
        public void CanReAddMetaConfigurationSectionAfterChangeIsCanceled()
        {
            ConfigurationSettings existingSettings = null;
            using (ConfigurationBuilder originalConfigReader = new ConfigurationBuilder(configFile))
            {
                existingSettings = originalConfigReader.ReadMetaConfiguration();
            }

            try
            {
                builder.ConfigurationChanging += new ConfigurationChangingEventHandler(CancelingHandler);
                builder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnMetaDataConfigurationChanged);
                Thread.Sleep(100);
                builder.WriteMetaConfig(CreateNewConfigurationSection());
                Thread.Sleep(250);

                builder.ConfigurationChanging -= new ConfigurationChangingEventHandler(CancelingHandler);
                builder.ConfigurationChanging += new ConfigurationChangingEventHandler(AcceptingHandler);
                builder.WriteMetaConfig(CreateNewConfigurationSection());
                Thread.Sleep(250);

                Assert.AreEqual("ChangeCanceledChangeAcceptedMetaDataChanged", eventString);
            }
            finally
            {
                builder.WriteMetaConfiguration(existingSettings);
            }
        }

        [Test]
        public void ChangingMetaConfigAfterClearSectionCacheStillCausesEvent()
        {
            //myconfig.xml
            builder.WriteConfiguration(mockConfigSection, data);
            using (ConfigurationBuilder newBuilder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                newBuilder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnMetaDataConfigurationChanged);
                Thread.Sleep(150);
                newBuilder.ClearSectionCache();
                Thread.Sleep(150);

                FileUtility.OpenAndChangeFile(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                for (int wait = 0; (!eventFired) && (wait < 10); ++wait)
                {
                    Thread.Sleep(500);
                }
                newBuilder.ConfigurationChanged -= new ConfigurationChangedEventHandler(OnMetaDataConfigurationChanged);
            }
            Assert.IsTrue(eventFired);
            Assert.AreEqual(ConfigurationSettings.SectionName, changedSectionName);
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToLower(), changedConfigurationFile.ToLower());
        }

        [Test]
        public void ChangingMetaConfigAfterDisposeDoesNotCauseEvent()
        {
            //myconfig.xml
            builder.WriteConfiguration(mockConfigSection, data);
            using (ConfigurationBuilder newBuilder = new ConfigurationBuilder(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile))
            {
                newBuilder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnMetaDataConfigurationChanged);
                Thread.Sleep(150);
            }

            FileUtility.OpenAndChangeFile(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            Thread.Sleep(300);

            Assert.IsFalse(eventFired);
        }

        [Test]
        public void WhenCreatingAContextMakeSureThatAllSectionsAreAddedToTheCache()
        {
            const string sectionA = "SectionA";
            const string sectionB = "SectionB";
            const string sectionAData = "SectionAData";
            const string sectionBData = "SectionBData";

            ConfigurationDictionary configurationDictionary = new ConfigurationDictionary();
            configurationDictionary.Add(sectionA, sectionAData);
            configurationDictionary.Add(sectionB, sectionBData);
            configurationDictionary.Add(ConfigurationSettings.SectionName, new ConfigurationSettings());
            ConfigurationContext context = ConfigurationManager.CreateContext(configurationDictionary);
            Assert.IsFalse(context.disposableWrapper.ConfigurationBuilder.CacheContains(ConfigurationSettings.SectionName), "Cache should not " + ConfigurationSettings.SectionName);
            Assert.IsTrue(context.disposableWrapper.ConfigurationBuilder.CacheContains(sectionA));
            Assert.IsTrue(context.disposableWrapper.ConfigurationBuilder.CacheContains(sectionB));
            Assert.AreEqual(sectionAData, context.GetConfiguration(sectionA));
            Assert.AreEqual(sectionBData, context.GetConfiguration(sectionB));
        }

        [Test]
        public void ChangingConfigurationSectionAfterDisposeDoesNotCauseEvent()
        {
            builder.WriteConfiguration(mockConfigSection, data);

            using (ConfigurationBuilder newBuilder = new ConfigurationBuilder(configFile))
            {
                newBuilder.WriteConfiguration(mockConfigSection, data);
                Thread.Sleep(300);

                newBuilder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);
            }

            builder.WriteConfiguration(mockConfigSection, data);
            Thread.Sleep(300);

            Assert.IsFalse(eventFired);
        }

        [Test]
        public void OnlyOneCallbackHappensWhenWritingToConfigurationSection()
        {
            builder.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);
            builder.WriteConfiguration(mockConfigSection, data);
            Thread.Sleep(3000);
            Assert.AreEqual(1, eventCount);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NoConfigurationFileProducesAnAgumentException()
        {
            using(ConfigurationBuilder builder = new ConfigurationBuilder(string.Empty))
            {
                builder.ReadMetaConfiguration();
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullConfigurationFileProducesAnArgumentNullException()
        {
            using(ConfigurationBuilder builder = new ConfigurationBuilder((string)null))
            {
                builder.ReadMetaConfiguration();
            }
        }

        [Test]
        public void ReadAndWriteToEncryptedConfiguration()
        {
            const string sectionName = "MyEncryptedConfig";

            KeyAlgorithmPair keyAlgorithmPair = new KeyAlgorithmPair();
            keyAlgorithmPair.Key = new SymmetricAlgorithmKeyCreator(typeof(RijndaelManaged).AssemblyQualifiedName).GenerateKey();
            keyAlgorithmPair.AlgorithmTypeName = typeof(RijndaelManaged).AssemblyQualifiedName;

            FileKeyAlgorithmPairStorageProvider provider = new FileKeyAlgorithmPairStorageProvider();
            provider.ConfigurationName = "FileKeyAlgorithmPairStorageProviderData";
            
            using (ConfigurationBuilder builder = new ConfigurationBuilder())
            {
                // change wher the file is
                RuntimeConfigurationView view = new RuntimeConfigurationView(new ConfigurationContext(new NonDisposingWrapper(builder)));

                FileKeyAlgorithmPairStorageProviderData fileData = (FileKeyAlgorithmPairStorageProviderData)view.GetKeyAlgorithmPairStorageProviderData();
                fileData.Path = Path.GetTempFileName();
                provider.Initialize(view);
                provider.Save(keyAlgorithmPair);
                builder.WriteConfiguration(sectionName, data);
                MockConfigurationData myData = (MockConfigurationData)builder.ReadConfiguration(sectionName);
                Assert.AreEqual(myData.ToString(), data.ToString());
                builder.ClearSectionCache();
                myData = (MockConfigurationData)builder.ReadConfiguration(sectionName);
                Assert.AreEqual(myData.ToString(), data.ToString());
            }
        }
           
        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void MissingSectionProducesCorrectExceptionMessage()
        {
            string file = Path.GetFullPath("MissingMetaDataConfigTest.xml");
            using(ConfigurationBuilder builder = new ConfigurationBuilder(file))
            {
                builder.ReadConfiguration("MySection");
            }
        }

        private ConfigurationSettings CreateConfigurationSettings()
        {
            ConfigurationSettings data = new ConfigurationSettings("defaultName");
            return data;
        }

        private ConfigurationSectionData GetConfigurationSection()
        {
            return new ConfigurationSectionData(mockConfigSection, false, GetStorageProvider(), GetTransformer());
        }

        private StorageProviderData GetStorageProvider()
        {
            return new XmlFileStorageProviderData(xmlFileStorageProvider, "testwebdata.xml");
        }

        private TransformerData GetTransformer()
        {
            return new XmlSerializerTransformerData(xmlSerializerTransformer);
        }

        private ConfigurationSectionData CreateNewConfigurationSection()
        {
            XmlFileStorageProviderData xmlFSData = new XmlFileStorageProviderData("name", fooConfig);
            XmlSerializerTransformerData xmlSerializerTransformer = new XmlSerializerTransformerData("name");
            return new ConfigurationSectionData("config", false, xmlFSData, xmlSerializerTransformer);
        }

        private void OnConfigurationChanging(object sender, ConfigurationChangingEventArgs args)
        {
            changedConfigurationFile = args.ConfigurationFile;
            changedSectionName = args.SectionName;
            changedOldValue = args.OldValue;
            changedNewValue = args.NewValue;
            eventFired = true;
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            changedConfigurationFile = args.ConfigurationFile;
            changedSectionName = args.SectionName;
            eventFired = true;
            eventCount++;
        }

        private void OnMetaDataConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            changedConfigurationFile = args.ConfigurationFile;
            changedSectionName = args.SectionName;
            eventFired = true;
            eventString += "MetaDataChanged";
        }

        private void OnDataConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            changedConfigurationFile = Path.GetFileName(args.ConfigurationFile);
            changedSectionName = args.SectionName;
            eventFired = true;
        }

        private void OnMockConfigConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            changedSectionName = args.SectionName;
            eventFired = true;
        }

        private void CancelingHandler(object sender, ConfigurationChangingEventArgs args)
        {
            args.Cancel = true;
            eventString += "ChangeCanceled";
        }

        private void AcceptingHandler(object sender, ConfigurationChangingEventArgs args)
        {
            eventString += "ChangeAccepted";
        }

        private class MockTransformer : TransformerProvider
        {
            private readonly ProviderData providerData;
            private readonly ConfigurationContext context;

            public MockTransformer(ProviderData providerData, ConfigurationContext context)
            {
                this.context = context;
                this.providerData = providerData;
            }

            public ProviderData ProviderData
            {
                get { return providerData; }
            }

            public ConfigurationContext Context
            {
                get { return context; }
            }

            public override void Initialize(ConfigurationView configurationView)
            {
            }

            public override object Serialize(object value)
            {
                return null;
            }

            public override object Deserialize(object section)
            {
                return null;
            }
        }
    }
}
#endif