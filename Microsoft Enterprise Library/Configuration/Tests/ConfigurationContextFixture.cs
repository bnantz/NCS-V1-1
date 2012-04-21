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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationContextFixture
    {
        [Test]
        public void CurrentTest()
        {
            ConfigurationContext context = ConfigurationManager.CreateContext();
            object data = context.GetConfiguration("ReadOnlyConfig");
            Assert.IsNull(data);
            ConfigurationContext context2 = ConfigurationManager.CreateContext();
            Assert.IsTrue(!ReferenceEquals(context, context2));
        }

        [Test]
        public void ConstructorWithConfigurationDictionaryTest()
        {
            ConfigurationDictionary dictionary = new ConfigurationDictionary();
            object expected = new object();
            dictionary.Add("section1", expected);
            ConfigurationContext context = ConfigurationManager.CreateContext(dictionary);
            object actual = context.GetConfiguration("section1");
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void ConfigurationFileTest()
        {
            ConfigurationContext context = ConfigurationManager.CreateContext();
            string file = context.ConfigurationFile;
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, file);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigurationDictionaryConstructorArgumentNullTest()
        {
            ConfigurationDictionary dictionary = null;
            ConfigurationManager.CreateContext(dictionary);
        }
    }
}

#endif