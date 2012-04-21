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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;
[assembly : ConfigurationDesignManager(typeof(ConfigurationDesignManagerAttributeFixture.MyConfigManager))]

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestFixture]
    public class ConfigurationDesignManagerAttributeFixture
    {
        [Test]
        public void GetCustomAttribute()
        {
            ConfigurationDesignManagerAttribute attribute = FindManagerTypeAttribute(typeof(MyConfigManager));
            Assert.IsNotNull(attribute);
        }

        [Test]
        public void BlockManagerAttributeTest()
        {
            ConfigurationDesignManagerAttribute attribute = FindManagerTypeAttribute(typeof(ConfigurationDesignManager));
            Assert.IsNotNull(attribute);
        }

        private ConfigurationDesignManagerAttribute FindManagerTypeAttribute(Type t)
        {
            object[] attributes = this.GetType().Assembly.GetCustomAttributes(typeof(ConfigurationDesignManagerAttribute), false);
            Assert.IsTrue(attributes.Length > 0);
            foreach (ConfigurationDesignManagerAttribute attribute in attributes)
            {
                if (attribute.ConfigurationDesignManager == t)
                {
                    return attribute;
                }
            }
            return null;
        }

        public class MyConfigManager : IConfigurationDesignManager
        {
            public void Register(IServiceProvider serviceProvider)
            {
            }

            public void Save(IServiceProvider serviceProvider)
            {
            }

            public void Open(IServiceProvider serviceProvider)
            {
            }

            public void BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary configurationDictionary)
            {
            }
        }
    }
}

#endif