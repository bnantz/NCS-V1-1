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

#if      UNIT_TESTS
using System;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Tools.ConfigurationConsole.Tests
{
    [TestFixture]
    public class FileManagerFixture
    {
        private const string configFile = "ConfigurationConsole.exe.config";
        private ApplicationConfigurationNode appNode;
        private FileManager fileManager;

        [TestFixtureSetUp]
        public void Initialize()
        {
            SolutionConfigurationNode node = new SolutionConfigurationNode();
            ApplicationData applicationData = ApplicationData.FromCurrentAppDomain();
            appNode = new ApplicationConfigurationNode(applicationData.Name, applicationData);
            appNode.Load(null);
            fileManager = new FileManager(node);
        }

        [Test]
        public void CommandTest()
        {
            Assert.AreEqual(7, appNode.NewCommand.SubCommands.Count);
            appNode.NewCommand.SubCommands[1].Execute();
        }

        [Test]
        public void FileExistsTest()
        {
            Assert.IsFalse(fileManager.FileExists("foo", appNode.ApplicationData));
            Assert.IsTrue(fileManager.FileExists(configFile, appNode.ApplicationData));
        }

        [Test]
        public void GetReadonlyFilesTest()
        {
            StringCollection files = fileManager.GetReadOnlyFiles(appNode.ApplicationData);
            Assert.AreEqual(0, files.Count);
        }

        [Test]
        public void ResolveFileNameTest()
        {
            string file = fileManager.ResolveFileName("dataConfiguration.config", appNode.ApplicationData);
            Assert.AreEqual(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dataConfiguration.config"), file);
        }

        [Test]
        public void IsFileReadOnlyTest()
        {
            string file = fileManager.ResolveFileName(configFile, appNode.ApplicationData);
            FileAttributes attributes = File.GetAttributes(file);
            FileAttributes newAttributes = attributes | FileAttributes.ReadOnly;
            File.SetAttributes(file, newAttributes);
            Assert.IsTrue(fileManager.IsFileReadOnly(configFile, appNode.ApplicationData));
            File.SetAttributes(file, attributes);
            Assert.IsFalse(fileManager.IsFileReadOnly(configFile, appNode.ApplicationData));
        }

        [Test]
        public void OverwriteFileTest()
        {
            string file = fileManager.ResolveFileName(configFile, appNode.ApplicationData);
            FileAttributes attributes = File.GetAttributes(file);
            FileAttributes newAttributes = attributes | FileAttributes.ReadOnly;
            File.SetAttributes(file, newAttributes);
            Assert.IsTrue(fileManager.IsFileReadOnly(configFile, appNode.ApplicationData));
            fileManager.OverwriteFile(file, appNode.ApplicationData);
            Assert.IsFalse(fileManager.IsFileReadOnly(configFile, appNode.ApplicationData));
        }

    }
}

#endif