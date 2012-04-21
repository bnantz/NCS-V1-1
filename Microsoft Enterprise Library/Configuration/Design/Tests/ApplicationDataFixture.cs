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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ApplicationDataFixture
    {
        private static readonly string name = "MyApp";
        private static readonly string baseDirectory = @"C:\";
        private static readonly string configurationFilePath = @"C:\Foo.config";

        private ApplicationData data;

        [SetUp]
        public void SetUp()
        {
            data = new ApplicationData(name, baseDirectory, configurationFilePath);
        }

        [Test]
        public void NameTest()
        {
            Assert.AreEqual(name, data.Name);
        }

        [Test]
        public void BaseDirectoryTest()
        {
            Assert.AreEqual(baseDirectory, data.BaseDirectory);
        }

        [Test]
        public void ConfigurationFilePathTest()
        {
            Assert.AreEqual(configurationFilePath, data.ConfigurationFilePath);
        }

        [Test]
        public void FromCurrentAppDomain()
        {
            ApplicationData appDomainData = ApplicationData.FromCurrentAppDomain();
            Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, appDomainData.Name);
            Assert.AreEqual(AppDomain.CurrentDomain.BaseDirectory, appDomainData.BaseDirectory);
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, appDomainData.ConfigurationFilePath);
        }
    }
}

#endif