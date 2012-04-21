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
    public class ConfigurationDesignManagerProxyFixture : ConfigurationDesignHostTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            CallCheck.Reset();
        }

        [Test]
        public void CreateTest()
        {
            ConfigurationDesignManagerProxy proxy = new ConfigurationDesignManagerProxy(typeof(MyManager));
            Assert.IsNotNull(proxy.ConfigurationDesignManager);
            Assert.AreEqual(typeof(MyManager), proxy.ConfigurationDesignManager.GetType());
        }

        [Test]
        public void RegisterTest()
        {
            ConfigurationDesignManagerProxy proxy = new ConfigurationDesignManagerProxy(typeof(MyManager));
            proxy.Register(Host);
            Assert.IsTrue(CallCheck.RegisterCalled);
            Assert.AreSame(Host, CallCheck.ServiceProvider);
        }

        [Test]
        public void OpenTest()
        {
            ConfigurationDesignManagerProxy proxy = new ConfigurationDesignManagerProxy(typeof(MyManager));
            proxy.Open(Host);
            Assert.IsTrue(CallCheck.OpenCalled);
            Assert.AreSame(Host, CallCheck.ServiceProvider);
        }

        [Test]
        public void SaveTest()
        {
            ConfigurationDesignManagerProxy proxy = new ConfigurationDesignManagerProxy(typeof(MyManager));
            proxy.Save(Host);
            Assert.IsTrue(CallCheck.SaveCalled);
            Assert.AreSame(Host, CallCheck.ServiceProvider);
        }

        [Test]
        public void BuildTest()
        {
            ConfigurationDesignManagerProxy proxy = new ConfigurationDesignManagerProxy(typeof(MyManager));
            proxy.BuildContext(Host, new ConfigurationDictionary());
            Assert.IsTrue(CallCheck.BuildCalled);
            Assert.AreSame(Host, CallCheck.ServiceProvider);
        }
        
        private sealed class CallCheck
        {
            private static bool registerCalled;
            private static bool saveCalled;
            private static bool openCalled;
            private static bool buildCalled;
            private static IServiceProvider serviceProvider;

            private CallCheck()
            {
            }

            public static bool RegisterCalled
            {
                get { return registerCalled; }
                set { registerCalled = value; }
            }

            public static bool SaveCalled
            {
                get { return saveCalled; }
                set { saveCalled = value; }
            }

            public static bool OpenCalled
            {
                get { return openCalled; }
                set { openCalled = value; }
            }

            public static bool BuildCalled
            {
                get { return buildCalled; }
                set { buildCalled = value; }
            }

            public static IServiceProvider ServiceProvider
            {
                get { return serviceProvider; }
                set { serviceProvider = value; }
            }

            public static void Reset()
            {
                RegisterCalled = false;
                SaveCalled = false;
                OpenCalled = false;
                BuildCalled = false;
                serviceProvider = null;
            }
        }

        private class MyManager : IConfigurationDesignManager
        {
            public MyManager()
            {
            }

            public void Register(IServiceProvider serviceProvider)
            {
                CallCheck.RegisterCalled = true;
                CallCheck.ServiceProvider = serviceProvider;
            }

            public void Save(IServiceProvider serviceProvider)
            {
                CallCheck.SaveCalled = true;
                CallCheck.ServiceProvider = serviceProvider;
            }

            public void Open(IServiceProvider serviceProvider)
            {
                CallCheck.OpenCalled = true;
                CallCheck.ServiceProvider = serviceProvider;
            }

            public void BuildContext(IServiceProvider serviceProvider, ConfigurationDictionary configurationDictionary)
            {
                CallCheck.BuildCalled = true;
                CallCheck.ServiceProvider = serviceProvider;
            }
        }

        private class MyOtherManager : IConfigurationDesignManager
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