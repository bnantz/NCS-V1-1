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
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    [TestFixture]
    [Serializable]
    public class ConfigurationManagerFixture
    {
        [Test]
        public void TestNoConfigFile()
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            setup.PrivateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            setup.PrivateBinPathProbe = AppDomain.CurrentDomain.SetupInformation.PrivateBinPathProbe;
            AppDomain domain = AppDomain.CreateDomain("MyDomain", AppDomain.CurrentDomain.Evidence, setup);
            try
            {
                domain.DoCallBack(new CrossAppDomainDelegate(ConfigWithNoConfigFileTest));
            }
            catch (ConfigurationException e)
            {
                Match match = Regex.Match(e.Message, "machine.config");
                Assert.IsTrue(match.Success);
            }
            finally
            {
                AppDomain.Unload(domain);
            }

        }

        public void ConfigWithNoConfigFileTest()
        {
            ConfigurationManager.GetConfiguration("foo");
        }

    }
}

#endif