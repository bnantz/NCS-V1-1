//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

//---------------------------------------------------------------------
// <copyright file="ConfigurationDesignManagerFixture.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Defines the ConfigurationDesignManagerFixture class.
// </summary>
//---------------------------------------------------------------------

#if UNIT_TESTS
using System;
using NUnit.Framework;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests 
{
    [TestFixture]
    public class ConfigurationDesignManagerFixture 
    {
        [SetUp]
        public void Setup()
        {
           ConfigurationDesignManager manager = new ConfigurationDesignManager();
           ApplicationData data = ApplicationData.FromCurrentAppDomain();
           manager.Register(new ApplicationConfigurationNode(data.Name, data), null);
        }

        [Test]
        public void Register() 
        {
            ConfigurationDesignManager manager = new ConfigurationDesignManager();
            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            ApplicationConfigurationNode applicationNode = new ApplicationConfigurationNode(data.Name, data);

            manager.Register(applicationNode, null);

            Assert.AreEqual(1, applicationNode.NewCommand.SubCommands.Count);
        }

        [Test]
        public void BuildContext() 
        {
            ApplicationData data = ApplicationData.FromCurrentAppDomain();
            ApplicationConfigurationNode applicationNode = new ApplicationConfigurationNode(data.Name, data);
            ConfigurationDesignManager manager = new ConfigurationDesignManager();
            manager.Register(applicationNode, null);
            foreach (ConfigurationNodeCommand command in applicationNode.NewCommand.SubCommands)
            {
                command.Execute();
            }

            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            manager.BuildContext(applicationNode, dictionary);
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.Contains(CryptographySettings.SectionName));
        }
    }
}
#endif