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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationChangingEventArgsFixture
    {
        [Test]
        public void ConstructorTest()
        {
            const string configurationFile = "myconfig.config";
            const string sectionName = "mySection";
            object oldValue = new object();
            object newValue = new object();

            ConfigurationChangingEventArgs args = new ConfigurationChangingEventArgs(configurationFile, sectionName, oldValue, newValue);
            Assert.AreEqual(configurationFile, args.ConfigurationFile);
            Assert.AreEqual(sectionName, args.SectionName);
            Assert.IsFalse(args.Cancel);
            Assert.AreSame(oldValue, args.OldValue);
            Assert.AreSame(newValue, args.NewValue);
        }
    }
}

#endif