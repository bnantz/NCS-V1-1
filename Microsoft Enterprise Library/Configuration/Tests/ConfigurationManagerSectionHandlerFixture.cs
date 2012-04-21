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
using SysConfig = System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationManagerSectionHandlerFixture
    {
        [Test]
        public void GetConfig()
        {
            object configObj = System.Configuration.ConfigurationSettings.GetConfig("enterpriselibrary.configurationSettings");
            Assert.IsNotNull(configObj);
            ConfigurationSettings configurationSettings = configObj as ConfigurationSettings;
            Assert.IsNotNull(configurationSettings);
            Assert.IsTrue(configurationSettings.ConfigurationSections.Count > 0);
        }

        [Test]
        public void CreateTest()
        {
            ConfigurationManagerSectionHandler handler = new ConfigurationManagerSectionHandler();
            object obj = handler.Create(null, null, null);
            Assert.IsNotNull(obj);
            Assert.AreEqual(typeof(ConfigurationSettings), obj.GetType());
        }
    }
}

#endif