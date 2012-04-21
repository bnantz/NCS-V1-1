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
using System.Configuration;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationFileFixture
    {
        private ConfigurationFile configFile;
        private static readonly string myConfigTestFile = @"MyConfigTest.xml";
        private static readonly string removeConfigTestFile = @"RemoveConfigTest.xml";
        private int eventFiredCount;

        [SetUp]
        public void SetUp()
        {
            configFile = new ConfigurationFile();
            eventFiredCount = 0;
        }

        [TearDown]
        public void TearDown()
        {
            configFile = null;
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void DoubleLoadMachineConfigTest()
        {
            ConfigurationFile machineConfig = new ConfigurationFile();
            machineConfig.Load(ConfigurationBuilder.MachineConfigurationFile);
            ConfigurationFile machineConfig2 = new ConfigurationFile(machineConfig);
            machineConfig2.Load(ConfigurationBuilder.MachineConfigurationFile);
        }

        [Test]
        public void FileNameTest()
        {
            string filename = Path.GetFullPath(myConfigTestFile);
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);
            Assert.AreEqual(filename, configFile.FileName);
        }

        [Test]
        public void UnResolvedSectionsTest()
        {
            string filename = Path.GetFullPath(myConfigTestFile);
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);
            Assert.AreEqual(1, configFile.UnevaluatedSections.Count);
        }

        [Test]
        public void ResolveSectionTest()
        {
            string filename = Path.GetFullPath(myConfigTestFile);
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);
            configFile.ResolveConfig("enterpriselibrary.configurationSettings");
            Assert.AreEqual(0, configFile.UnevaluatedSections.Count);
        }

        [Test]
        public void GetConfigTest()
        {
            string filename = Path.GetFullPath(myConfigTestFile);
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);
            object configuration = configFile.GetConfig("enterpriselibrary.configurationSettings");
            Assert.IsNotNull(configuration);
            ConfigurationSettings configurationSettings = configuration as ConfigurationSettings;
            Assert.IsNotNull(configurationSettings);
        }

        [Test]
        public void LoadTest()
        {
            string filename = Path.GetFullPath(myConfigTestFile);
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);
        }

        [Test]
        public void StorageFiresEventTest()
        {
            string filename = Path.GetFullPath(myConfigTestFile);
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);

            using (IConfigurationChangeWatcher watcher = configFile.CreateFileWatcher())
            {
                ((ConfigurationChangeFileWatcher)watcher).SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);
                watcher.StartWatching();
                Thread.Sleep(150);

                FileUtility.OpenAndChangeFile(myConfigTestFile);
                for (int wait = 0; wait < 10 && eventFiredCount == 0; ++wait)
                {
                    Thread.Sleep(500);
                }
            }

            Assert.AreEqual(1, eventFiredCount);
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs args)
        {
            Assert.AreEqual(Path.GetFileName(myConfigTestFile).ToLower(), Path.GetFileName(args.ConfigurationFile).ToLower());
            Assert.AreEqual(ConfigurationSettings.SectionName, args.SectionName);
            ++eventFiredCount;
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void LoadBadConfigTest()
        {
            string filename = Path.GetFullPath(@"MyBadConfigTest.xml");
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void LoadBadXmlConfigTest()
        {
            string filename = Path.GetFullPath(@"MyBadXmlConfigTest.xml");
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void ResolveBadXmlConfigTest()
        {
            string filename = Path.GetFullPath(@"MyBadConfigSectionTest.xml");
            bool fileExists = File.Exists(filename);
            Assert.IsTrue(fileExists);
            bool loaded = configFile.Load(filename);
            Assert.IsTrue(loaded);
            Assert.IsNotNull(configFile.ResolveConfig("enterpriselibrary.configurationSettings"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AppendSectionNullSectiionTest()
        {
            configFile.AppendSection(null, null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AppendSectionNullTypeTest()
        {
            configFile.AppendSection("name", null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AppendSectionNullNodeTest()
        {
            configFile.AppendSection("name", "myType", null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AppendSectionNoFileLoadedTest()
        {
            ConfigurationFile file = new ConfigurationFile();
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateElement("tempElement");
            file.AppendSection("name", "MyType", node);
        }

        [Test]
        [ExpectedException(typeof(SecurityException))]
        public void OpenConfigFileWithoutPermissions()
        {
            FileIOPermission denyPermission =
                new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath(myConfigTestFile));

            PermissionSet permissions = new PermissionSet(PermissionState.None);
            permissions.AddPermission(denyPermission);
            permissions.Deny();
            ConfigurationFile configFile = new ConfigurationFile();
            configFile.Load(Path.GetFullPath(myConfigTestFile));

        }

        [Test]
        public void RemoveSectionFromConfigurationFile()
        {
            ConfigurationFile file = new ConfigurationFile();
            file.Load(Path.GetFullPath(removeConfigTestFile));
            file.RemoveSection(ConfigurationSettings.SectionName);

            XmlDocument doc = new XmlDocument();
            doc.Load(file.FileName);

            XmlNode configSections = doc.DocumentElement.SelectSingleNode("//configuration/configSections");
            Assert.IsNotNull(configSections);
            XmlNode section = configSections.SelectSingleNode(string.Concat("//section[@name='", ConfigurationSettings.SectionName, "']"));
            Assert.IsNull(section);
            XmlNode sectionInformationNode = doc.DocumentElement.SelectSingleNode(string.Concat("//entlib:", ConfigurationSettings.SectionName), ConfigurationFile.GetNamespaceManager(doc));
            Assert.IsNull(sectionInformationNode);
        }

        [Test]
        public void MakeSureWhidbeyCofigurationFileCanBeRead()
        {
            string machineConfig = Path.GetFullPath("WhidbeyMachine.config");
            string fileName = Path.GetFullPath("Whidbey.config");
            ConfigurationFile machineFile = new ConfigurationFile();
            machineFile.Load(machineConfig);
            ConfigurationFile file = new ConfigurationFile(machineFile);
            file.Load(fileName);
        }
    }
}

#endif